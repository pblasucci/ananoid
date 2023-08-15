(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid.Tests

open FsCheck
open pblasucci.Ananoid
open pblasucci.Ananoid.Core
open pblasucci.Ananoid.Core.Tagged
open pblasucci.Ananoid.KnownAlphabets


type RawNanoId = RawNanoId of string

type TaggedNanoId = TaggedNanoId of string<nanoid>


type Generation() =
  static member ValidAlphabet =
    let generate =
      Gen.elements [
        Alphanumeric
        HexadecimalLowercase
        HexadecimalUppercase
        Lowercase
        NoLookalikes
        NoLookalikesSafe
        Numbers
        Uppercase
        UrlSafe
      ]

    Arb.fromGen generate

  static member RawNanoId =
    let generate =
      Gen.sized (fun size ->
        Gen.fresh (fun () -> RawNanoId(nanoIdOf Defaults.Alphabet size))
      )
      |> Gen.scaleSize (max 1)

    let shrink (RawNanoId value) =
      match String.length value with
      | length when length > 1 ->
        length
        |> Arb.shrinkNumber
        |> Seq.map (fun size -> RawNanoId(nanoIdOf Defaults.Alphabet size))
      | _ -> Seq.empty

    Arb.fromGenShrink (generate, shrink)

  static member TaggedNanoId =
    let generate =
      Gen.sized (fun size ->
        Gen.fresh (fun () -> TaggedNanoId(nanoIdOf' Defaults.Alphabet size))
      )

    let shrink (TaggedNanoId value) =
      match String.length (string value) with
      | 0 -> Seq.empty
      | length ->
        length
        |> Arb.shrinkNumber
        |> Seq.map (fun size -> TaggedNanoId(nanoIdOf' Defaults.Alphabet size))

    Arb.fromGenShrink (generate, shrink)

  static member NanoId =
    let generate = Gen.fresh NanoId.NewId
    let shrink nanoId =
      match String.length (string nanoId) with
      | 0 -> Seq.empty
      | length ->
        length
        |> Arb.shrinkNumber
        |> Seq.map (NanoId.ofOptions UrlSafe)

    Arb.fromGenShrink (generate, shrink)
