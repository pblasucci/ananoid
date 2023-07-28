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


type RawNanoId = RawNanoId of string

type TaggedNanoId = TaggedNanoId of string<nanoid>


type Generation() =
  static member IAlphabet =
    let generate =
      Gen.elements (
        [
          Alphabet.Alphanumeric
          Alphabet.HexadecimalLowercase
          Alphabet.HexadecimalUppercase
          Alphabet.Lowercase
          Alphabet.NoLookalikes
          Alphabet.NoLookalikesSafe
          Alphabet.Numbers
          Alphabet.Uppercase
          Alphabet.UrlSafe
        ]
        : IAlphabet list
      )

    Arb.fromGen generate

  static member NanoIdOptions =
    let generate =
      Gen.elements [
        NanoIdOptions.Alphanumeric
        NanoIdOptions.HexadecimalLowercase
        NanoIdOptions.HexadecimalUppercase
        NanoIdOptions.Lowercase
        NanoIdOptions.NoLookalikes
        NanoIdOptions.NoLookalikesSafe
        NanoIdOptions.Numbers
        NanoIdOptions.Uppercase
        NanoIdOptions.UrlSafe
      ]

    let shrink (options : NanoIdOptions) =
      match options.Size with
      | 0 -> Seq.empty
      | n -> n |> Arb.shrinkNumber |> Seq.map options.Resize

    Arb.fromGenShrink (generate, shrink)

  static member RawNanoId =
    let generate =
      Gen.sized (fun size ->
        Gen.fresh (fun () -> RawNanoId(nanoIdOf (string UrlSafe) size))
      )

    let shrink (RawNanoId value) =
      match String.length value with
      | 0 -> Seq.empty
      | length ->
        length
        |> Arb.shrinkNumber
        |> Seq.map (fun size -> RawNanoId(nanoIdOf (string UrlSafe) size))

    Arb.fromGenShrink (generate, shrink)

  static member TaggedNanoId =
    let generate =
      Gen.sized (fun size ->
        Gen.fresh (fun () -> TaggedNanoId(nanoIdOf' (string UrlSafe) size))
      )

    let shrink (TaggedNanoId value) =
      match String.length (string value) with
      | 0 -> Seq.empty
      | length ->
        length
        |> Arb.shrinkNumber
        |> Seq.map (fun size -> TaggedNanoId(nanoIdOf' (string UrlSafe) size))

    Arb.fromGenShrink (generate, shrink)

  static member NanoId =
    let generate = Gen.fresh NanoId.NewId
    let shrink nanoId =
      match String.length (string nanoId) with
      | 0 -> Seq.empty
      | length ->
        length
        |> Arb.shrinkNumber
        |> Seq.map (fun size ->
          let options = NanoIdOptions.UrlSafe.Resize(size)
          NanoId.NewId(options)
        )
    Arb.fromGenShrink (generate, shrink)
