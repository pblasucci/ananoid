(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open System
open System.Diagnostics.CodeAnalysis
open System.Runtime.CompilerServices
open System.Runtime.InteropServices


[<AutoOpen>]
module String =
  let inline trimmed value =
    match value with
    | Null -> ""
    | NonNull(value : string) -> value.Trim()

  let inline (|Trimmed|) value = trimmed value

  let inline (|Length|) value =
    match value with
    | Null -> 0ul
    | NonNull(Trimmed value) -> uint32 value.Length


[<IsReadOnly>]
[<Struct>]
type NanoId(nanoId' : string | null) =
  member _.Length = nanoId' |> trimmed |> String.length

  override _.ToString() = trimmed nanoId'

  static member Empty = NanoId()

  static member NewId() = NanoId(Core.nanoId ())

  static member IsEmpty(nanoId : NanoId) = nanoId.Length < 1


type InvalidAlphabet = {
  invalidAlphabet' : string | null
} with
  member me.Letters = trimmed me.invalidAlphabet'
  member internal _.Message = "must be between 1 and 255 letters"
  override me.ToString() = $"Invalid Alphabet: '%s{me.Letters}' %s{me.Message}"


type Alphabet = {
  alphabet' : string
} with
  member me.Letters = let (Trimmed letters) = me.alphabet' in letters

  override me.ToString() = me.Letters

  static member Validate(Trimmed letters & Length length) =
    if length < 1u || 255u < length then
      Error { invalidAlphabet' = letters }
    else
      Ok { alphabet' = letters }


module AlphabetPatterns =
  let inline (|Letters|) (hasLetters : 'T) = (^T : (member Letters : string) hasLetters)


[<RequireQualifiedAccess>]
module Alphabet =
  open AlphabetPatterns

  let ofLetters letters = Alphabet.Validate letters

  let makeOrRaise letters =
    match ofLetters letters with
    | Ok alphabet -> alphabet
    | Error e -> raise <| ArgumentOutOfRangeException(nameof letters, letters, e.Message)

  let makeNanoId size (Letters alphabet) =
    if size < 1 then NanoId.Empty else NanoId(Core.nanoIdOf alphabet size)

  let inline (|Allowed|_|) (Letters alphabet) (Trimmed value) =
    if value |> String.forall alphabet.Contains then Some value else None

  let parseNanoId value alphabet =
    match value with
    | Length 0ul -> Some NanoId.Empty
    | Allowed alphabet value -> Some(NanoId value)
    | _ -> None

  let parseNonEmptyNanoId value alphabet =
    match value with
    | Length 0ul -> None
    | Allowed alphabet value -> Some(NanoId value)
    | _ -> None


type Alphabet with
  member me.MakeNanoId(size) = me |> Alphabet.makeNanoId size

  member me.ParseNanoId(value) = me |> Alphabet.parseNanoId value

  member me.ParseNonEmptyNanoId(value) = me |> Alphabet.parseNonEmptyNanoId value


[<Sealed>]
type AlphabetExtensions =
  [<Extension>]
  static member ToAlphabet(letters) = Alphabet.ofLetters letters

  [<Extension>]
  static member ToAlphabetOrThrow(letters) = Alphabet.makeOrRaise letters

  [<Extension>]
  static member TryMakeAlphabet(letters, [<Out; NotNullWhen(returnValue = true)>] alphabet : outref<Alphabet | null>) =
    let validated = Alphabet.ofLetters letters
    alphabet <- validated |> Result.map withNull |> Result.defaultValue null
    Result.isOk validated

  [<Extension>]
  static member TryParseNanoId(alphabet, value, [<Out; NotNullWhen(returnValue = true)>] nanoId : outref<NanoId>) =
    let parsed = alphabet |> Alphabet.parseNanoId value
    nanoId <- parsed |> Option.defaultValue NanoId.Empty
    Option.isSome parsed

  [<Extension>]
  static member TryParseNonEmptyNanoId
    (alphabet, value, [<Out; NotNullWhen(returnValue = true)>] nanoId : outref<NanoId>)
    =
    let parsed = alphabet |> Alphabet.parseNonEmptyNanoId value
    nanoId <- parsed |> Option.defaultValue NanoId.Empty
    Option.isSome parsed


[<RequireQualifiedAccess>]
module NanoId =
  let length (nanoId : NanoId) = nanoId.Length

  let isEmpty nanoId = length nanoId < 1

  let ofDefaults () = NanoId.NewId()

  let ofOptions alphabet size = alphabet |> Alphabet.makeNanoId size

  let parseAs alphabet value = alphabet |> Alphabet.parseNanoId value

  let parseNonEmptyAs alphabet value = alphabet |> Alphabet.parseNonEmptyNanoId value


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


// NOTE ⮟⮟⮟ needed for VB.NET support
[<assembly : Extension>]
do ()
