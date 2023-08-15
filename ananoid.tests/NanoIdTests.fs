(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
module pblasucci.Ananoid.Tests.NanoIdTests

open System
open System.Reflection
open FsCheck
open FsCheck.Xunit

(* ⮟ system under test ⮟ *)
open pblasucci.Ananoid
open pblasucci.Ananoid.KnownAlphabets


[<Property(MaxTest = 1)>]
let ``Input size zero produces empty NanoId`` () =
  let generated = UrlSafe |> Alphabet.makeNanoId 0
  generated |> NanoId.isEmpty |> Prop.label (string generated)

[<Property(MaxTest = 1)>]
let ``Multiple empty NanoId instances are equal`` () =
  let nils = [
    NanoId()
    NanoId.Empty
    NanoId.ofOptions UrlSafe 0
  ]
  let allEmpty = nils |> List.forall NanoId.isEmpty
  let allEqual = nils |> List.allPairs nils |> List.forall (fun (l,r) -> l = r)
  allEmpty .&. allEqual

[<Property(MaxTest = 1)>]
let ``Default NanoId is 21 UrlSafe characters`` () =
  let value = NanoId.ofDefaults ()
  let parsed = UrlSafe.ParseNanoId(string value)

  value.Length = 21 && Option.isSome parsed

[<Property>]
let ``Output size always equals input size`` (NonNegativeInt size) =
  let generated = UrlSafe.MakeNanoId(size)
  let length = int generated.Length
  length = size |> Prop.label $"expect (%i{size}) <> actual (%i{length})"

[<Property(Arbitrary = [| typeof<Generation> |])>]
let ``Multiple instances are equal when their underlying values are equal``
  (RawNanoId input)
  =
  let one = UrlSafe |> Alphabet.parseNanoId input
  let two = input |> NanoId.parseAs UrlSafe
  one = two |> Prop.label $"\nNot equal on '{input}'"

[<Property(Arbitrary = [| typeof<Generation> |])>]
let ``Multiple instances are ordered the same as their underlying values``
  (RawNanoId input1)
  (RawNanoId input2)
  (RawNanoId input3)
  =
  let fromRaw = NanoId.parseAs UrlSafe >> Option.toList

  let inputs = List.sort [ input1; input2; input3 ]
  let values =
    List.sort [
      yield! fromRaw input1
      yield! fromRaw input2
      yield! fromRaw input3
    ]

  values
  |> List.zip inputs
  |> List.forall (fun (raw, nano) -> raw = string nano)
  |> Prop.label $"\ninputs: {inputs}\nvalues: {values}"

[<Property(Arbitrary = [| typeof<Generation> |])>]
let ``Parse and ToString are invertible`` nanoId =
  match nanoId |> string |> NanoId.parseAs UrlSafe with
  | Some nanoId' -> nanoId' = nanoId
  | None -> false
  |> Prop.label $"\nNot invertible: %A{nanoId}"
