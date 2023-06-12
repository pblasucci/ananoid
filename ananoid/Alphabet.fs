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


type AlphabetError =
  | AlphabetTooLarge
  | AlphabetTooSmall
  | IncoherentAlphabet
  member me.Message =
    match me with
    | AlphabetTooLarge -> "Alphabet may not contain more than 255 letters."
    | AlphabetTooSmall -> "Alphabet must contain at least one letter."
    | IncoherentAlphabet -> "Alphabet cannot validate its own letters."
  override me.ToString() =
    let case =
      match me with
      | AlphabetTooLarge -> nameof AlphabetTooLarge
      | AlphabetTooSmall -> nameof AlphabetTooSmall
      | IncoherentAlphabet -> nameof IncoherentAlphabet
    $"{nameof AlphabetError}.{case} '{me.Message}'"


[<Sealed>]
type AlphabetException(source : IAlphabet, reason : AlphabetError) =
  inherit Exception($"Invalid alphabet, reason: %s{reason.Message}")

  member me.Source = source
  member me.Reason = reason


type AlphabetError with
  member me.Promote(alphabet) = raise (AlphabetException(alphabet, me))


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
      Error AlphabetTooSmall
    elif alphabet.Letters.Length < 1 then
      Error AlphabetTooSmall
    elif 255 < alphabet.Letters.Length then
      Error AlphabetTooLarge
    elif not (alphabet.WillPermit alphabet.Letters) then
      Error IncoherentAlphabet
    else
      Ok alphabet

  [<CompiledName("ValidateOrThrow")>]
  static member ValidateOrRaise(alphabet) =
    Alphabet.Validate(alphabet)
    |> Result.defaultWith (fun e -> e.Promote alphabet)
    |> ignore
