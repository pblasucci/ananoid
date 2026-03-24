(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
module pblasucci.Ananoid.Tests.NanoIdTests

open Hedgehog.FSharp
open Hedgehog.Xunit
open global.Xunit

(* ⮟ system under test ⮟ *)
open pblasucci.Ananoid
open pblasucci.Ananoid.KnownAlphabets


[<Fact>]
let ``Input size zero produces empty NanoId`` () =
  let generated = UrlSafe |> Alphabet.makeNanoId 0
  Assert.True(NanoId.isEmpty generated)

[<Fact>]
let ``Multiple empty NanoId instances are equal`` () =
  let nils = [ NanoId(); NanoId.Empty; NanoId.ofOptions UrlSafe 0 ]
  let allEmpty = nils |> List.forall NanoId.isEmpty
  let allEqual = nils |> List.allPairs nils |> List.forall (fun (l, r) -> l = r)
  Assert.True(allEmpty && allEqual)

[<Fact>]
let ``Default NanoId is 21 UrlSafe characters`` () =
  let value = NanoId.ofDefaults ()
  Assert.Multiple(
    (fun () -> Assert.Equal(21, value.Length)),
    (fun () ->
      // NOTE "can be parsed" is a good-enough proxy for "is of alphabet"
      Assert.True(value |> string |> UrlSafe.ParseNanoId |> Option.isSome)
    )
  )

[<Property>]
let ``Output size always equals input size`` ([<NonNegativeInt(1000000)>] size) =
  property {
    let _generated = UrlSafe.MakeNanoId(size)
    let _length = _generated.Length
    // HACK:
    // `length` is actually used, but needs a "no warning leading underscore"
    // because of a bug in the interaction between tooling and computation expressions.
    counterexample $"expect (%i{size}) <> actual (%i{_length})"
    _length = size
  }

[<Property(typeof<Generation>)>]
let ``Multiple instances are equal when their underlying values are equal`` ([<RawNanoId>] input) =
  property {
    let _one = UrlSafe |> Alphabet.parseNanoId input
    let _two = input |> NanoId.parseAs UrlSafe
    counterexample $"\nNot equal on '{input}'"
    _one = _two
  }

[<Property(typeof<Generation>)>]
let ``Multiple instances are ordered the same as their underlying values``
  ([<RawNanoId>] input1)
  ([<RawNanoId>] input2)
  ([<RawNanoId>] input3)
  =
  let fromRaw = NanoId.parseAs UrlSafe >> Option.toList

  property {
    let _inputs = List.sort [ input1; input2; input3 ]
    let _values =
      List.sort [ yield! fromRaw input1; yield! fromRaw input2; yield! fromRaw input3 ]

    let _correctlySorted =
      _values
      |> List.zip _inputs
      |> List.forall (fun (raw, nano) -> raw = string nano)

    counterexample $"\ninputs: {_inputs}\nvalues: {_values}"
    return _correctlySorted
  }

[<Property(typeof<Generation>)>]
let ``Parse and ToString are invertible`` (alphabet : Alphabet) ([<PositiveInt(1000000)>] nanoIdLength) =
  property {
    let nanoId = alphabet.MakeNanoId nanoIdLength
    let _parsed = nanoId |> string |> NanoId.parseAs alphabet
    // HACK:
    // `parsed` is actually used, but needs a "no warning leading underscore"
    // because of a bug in the interaction between tooling and computation expressions.
    counterexample $"%A{_parsed} <> %A{nanoId}"
    _parsed = Some nanoId
  }

[<Property(typeof<Generation>)>]
let ``Parse can explicitly reject empty input`` (alphabet : Alphabet) =
  "" |> alphabet.ParseNonEmptyNanoId |> Option.isNone
