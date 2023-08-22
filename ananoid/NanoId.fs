(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open System
open System.Runtime.CompilerServices
open Microsoft.FSharp.Core


[<AutoOpen>]
module StringPatterns =
  let inline (|Empty|_|) value =
    if String.IsNullOrWhiteSpace value then Some() else None

  let inline (|Trimmed|) (value : string) =
    Trimmed(if String.IsNullOrWhiteSpace value then "" else value.Trim())

  let inline (|Length|) (Trimmed trimmed) = Length(uint32 trimmed.Length)


[<IsReadOnly>]
[<Struct>]
type NanoId(nanoId' : string) =
  member _.Length = String.length nanoId'

  override _.ToString() = let (Trimmed value) = nanoId' in value

  static member Empty = NanoId()

  static member NewId() = NanoId(Core.nanoId ())


type AlphabetError =
  | AlphabetTooLarge of Source : string
  | AlphabetTooSmall of Source : string

  member me.Letters =
    match me with
    | AlphabetTooLarge letters
    | AlphabetTooSmall letters -> letters

  member me.Message =
    match me with
    | AlphabetTooLarge _ -> "Alphabet may not contain more than 255 letters."
    | AlphabetTooSmall _ -> "Alphabet must contain at least one letter."

  override me.ToString() =
    let case =
      match me with
      | AlphabetTooLarge _ -> nameof AlphabetTooLarge
      | AlphabetTooSmall _ -> nameof AlphabetTooSmall
    $"{nameof AlphabetError}.{case} '{me.Message}'"


[<Sealed>]
type AlphabetException(reason : AlphabetError) =
  inherit Exception($"Invalid alphabet, reason: %s{reason.Message}")

  member _.Alphabet = reason.Letters
  member _.Reason = reason


type Alphabet = {
  alphabet' : string
} with
  member me.Letters = let (Trimmed letters) = me.alphabet' in letters

  override me.ToString() = me.Letters

  static member Validate(Trimmed letters) =
    if String.length letters < 1 then Error(AlphabetTooSmall letters)
    elif 255 < String.length letters then Error(AlphabetTooLarge letters)
    else Ok({ alphabet' = letters })


type AlphabetError with
  member me.Promote() = raise (AlphabetException me)


[<RequireQualifiedAccess>]
module Alphabet =
  let ofLetters letters = Alphabet.Validate letters

  let inline (|Letters|) (alphabet : Alphabet) = Letters(string alphabet)

  let makeOrRaise letters =
    match ofLetters letters with
    | Ok alphabet -> alphabet
    | Error error -> error.Promote()

  let makeNanoId size (Letters alphabet) =
    if size < 1 then NanoId.Empty else NanoId(Core.nanoIdOf alphabet size)

  let inline (|Allowed|_|) (Letters alphabet) (Trimmed value) =
    if value |> String.forall alphabet.Contains then Some value else None

  let parseNanoId value alphabet =
    match value with
    | Empty -> Some NanoId.Empty
    | Allowed alphabet value -> Some(NanoId value)
    | _ -> None


type Alphabet with
  member me.MakeNanoId(size) = me |> Alphabet.makeNanoId size

  member me.ParseNanoId(value) = me |> Alphabet.parseNanoId value


[<Extension>]
[<Sealed>]
type AlphabetExtensions =
  [<Extension>]
  static member ToAlphabet(letters) = Alphabet.ofLetters letters

  [<Extension>]
  static member ToAlphabetOrThrow(letters) = Alphabet.makeOrRaise letters

  [<Extension>]
  static member TryParseNanoId
    (
      alphabet,
      value,
      nanoId : outref<NanoId>
    )
    =
    let parsed = alphabet |> Alphabet.parseNanoId value
    nanoId <- parsed |> Option.defaultValue NanoId.Empty
    Option.isSome parsed


[<RequireQualifiedAccess>]
module NanoId =
  let length (nanoId : NanoId) = nanoId.Length

  let isEmpty nanoId = length nanoId < 1

  let ofDefaults () = NanoId.NewId()

  let ofOptions alphabet size = alphabet |> Alphabet.makeNanoId size

  let parseAs alphabet value = alphabet |> Alphabet.parseNanoId value


module KnownAlphabets =
  open Core.Alphabets

  let Alphanumeric = { alphabet' = Alphanumeric }

  let HexadecimalLowercase = { alphabet' = HexadecimalLowercase }

  let HexadecimalUppercase = { alphabet' = HexadecimalUppercase }

  let Lowercase = { alphabet' = Lowercase }

  let NoLookalikes = { alphabet' = NoLookalikes }

  let NoLookalikesSafe = { alphabet' = NoLookalikesSafe }

  let Numbers = { alphabet' = Numbers }

  let Uppercase = { alphabet' = Uppercase }

  let UrlSafe = { alphabet' = UrlSafe }


[<assembly : Extension>]
do ()
