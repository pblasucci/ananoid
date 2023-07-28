(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open System
open System.Text.RegularExpressions
open pblasucci.Ananoid


type IAlphabet =
  abstract Letters : string
  abstract WillPermit : value : string -> bool


module CharSets =
  [<Literal>]
  let UrlSafe = Core.Defaults.Alphabet

  [<Literal>]
  let Numbers = "0123456789"

  [<Literal>]
  let HexadecimalLowercase = "0123456789abcdef"

  [<Literal>]
  let HexadecimalUppercase = "0123456789ABCDEF"

  [<Literal>]
  let Lowercase = "abcdefghijklmnopqrstuvwxyz"

  [<Literal>]
  let Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

  [<Literal>]
  let Alphanumeric =
    "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

  [<Literal>]
  let NoLookalikes = "346789ABCDEFGHJKLMNPQRTUVWXYabcdefghijkmnpqrtwxyz"

  [<Literal>]
  let NoLookalikesSafe = "6789BCDFGHJKLMNPQRTWbcdfghjkmnpqrtwz"


module Patterns =
  [<Literal>]
  let UrlSafe = "^[a-zA-Z0-9_-]+$"

  [<Literal>]
  let Numbers = "^[0-9]+$"

  [<Literal>]
  let HexadecimalLowercase = "^[0-9abcdef]+$"

  [<Literal>]
  let HexadecimalUppercase = "^[0-9ABCDEF]+$"

  [<Literal>]
  let Lowercase = "^[a-z]+$"

  [<Literal>]
  let Uppercase = "^[A-Z]+$"

  [<Literal>]
  let Alphanumeric = "^[a-zA-Z0-9]+$"

  [<Literal>]
  let NoLookalikes = "^[346789ABCDEFGHJKLMNPQRTUVWXYabcdefghijkmnpqrtwxyz]+$"

  [<Literal>]
  let NoLookalikesSafe = "^[6789BCDFGHJKLMNPQRTWbcdfghjkmnpqrtwz]+$"


[<NoComparison>]
type AlphabetError =
  | AlphabetTooLarge of Alphabet : IAlphabet
  | AlphabetTooSmall of Alphabet : IAlphabet
  | IncoherentAlphabet of Alphabet : IAlphabet

  member me.Source =
    match me with
    | AlphabetTooLarge alphabet
    | AlphabetTooSmall alphabet
    | IncoherentAlphabet alphabet -> alphabet

  member me.Message =
    match me with
    | AlphabetTooLarge _ -> "Alphabet may not contain more than 255 letters."
    | AlphabetTooSmall _ -> "Alphabet must contain at least one letter."
    | IncoherentAlphabet _ -> "Alphabet cannot validate its own letters."

  override me.ToString() =
    let case =
      match me with
      | AlphabetTooLarge _ -> nameof AlphabetTooLarge
      | AlphabetTooSmall _ -> nameof AlphabetTooSmall
      | IncoherentAlphabet _ -> nameof IncoherentAlphabet
    $"{nameof AlphabetError}.{case} '{me.Message}'"


[<Sealed>]
type AlphabetException(reason : AlphabetError) =
  inherit Exception($"Invalid alphabet, reason: %s{reason.Message}")

  member me.Source = reason.Source
  member me.Reason = reason


type AlphabetError with
  member me.Promote() = raise (AlphabetException me)


type Alphabet =
  | Alphanumeric
  | HexadecimalLowercase
  | HexadecimalUppercase
  | Lowercase
  | NoLookalikes
  | NoLookalikesSafe
  | Numbers
  | Uppercase
  | UrlSafe

  override me.ToString() = (me :> IAlphabet).Letters

  interface IAlphabet with
    member me.Letters =
      match me with
      | Alphanumeric -> CharSets.Alphanumeric
      | UrlSafe -> CharSets.UrlSafe
      | HexadecimalLowercase -> CharSets.HexadecimalLowercase
      | HexadecimalUppercase -> CharSets.HexadecimalUppercase
      | Lowercase -> CharSets.Lowercase
      | NoLookalikes -> CharSets.NoLookalikes
      | NoLookalikesSafe -> CharSets.NoLookalikesSafe
      | Numbers -> CharSets.Numbers
      | Uppercase -> CharSets.Uppercase

    member me.WillPermit(value) =
      match value with
      | Empty -> true
      | Trimmed raw ->
        let spec =
          match me with
          | Alphanumeric -> Patterns.Alphanumeric
          | UrlSafe -> Patterns.UrlSafe
          | HexadecimalLowercase -> Patterns.HexadecimalLowercase
          | HexadecimalUppercase -> Patterns.HexadecimalUppercase
          | Lowercase -> Patterns.Lowercase
          | NoLookalikes -> Patterns.NoLookalikes
          | NoLookalikesSafe -> Patterns.NoLookalikesSafe
          | Numbers -> Patterns.Numbers
          | Uppercase -> Patterns.Uppercase
        Regex.IsMatch(raw, spec, RegexOptions.Compiled, TimeSpan.FromSeconds 1)

  static member Validate(alphabet : IAlphabet) =
    if isNull (alphabet :> obj) then
      Error(AlphabetTooSmall alphabet)
    elif alphabet.Letters.Length < 1 then
      Error(AlphabetTooSmall alphabet)
    elif 255 < alphabet.Letters.Length then
      Error(AlphabetTooLarge alphabet)
    elif not (alphabet.WillPermit alphabet.Letters) then
      Error(IncoherentAlphabet alphabet)
    else
      Ok alphabet

  [<CompiledName("ValidateOrThrow")>]
  static member ValidateOrRaise(alphabet) =
    Alphabet.Validate(alphabet)
    |> Result.defaultWith (fun e -> e.Promote())
    |> ignore
