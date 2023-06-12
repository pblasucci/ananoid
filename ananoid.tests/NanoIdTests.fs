(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
module pblasucci.Ananoid.Tests.NanoIdTests

open System
open System.Reflection
open Xunit
open FsCheck
open FsCheck.Xunit

(* ⮟ system under test ⮟ *)
open pblasucci.Ananoid


[<Property(MaxTest = 1)>]
let ``Input size zero produces empty NanoId`` () =
  let generated = NanoId.NewId(NanoIdOptions.UrlSafe.Resize(size = 0))
  generated |> NanoId.IsEmpty |> Prop.label (string generated)

[<Property(MaxTest = 1)>]
let ``Multiple empty NanoId instances are equal`` () =
  let resized = NanoId.NewId(NanoIdOptions.UrlSafe.Resize(size = 0))
  let one = NanoId() = NanoId.Empty |> Prop.label "\n.ctor is not `Empty`"
  let two = NanoId.Empty = resized |> Prop.label "\nzeroed is not `Empty`"
  one .&. two

[<Property(MaxTest = 1)>]
let ``By-passing safety checks caused run-time failure`` () =
  let bypassWithBadInput () =
    let badAlphabet =
      { new IAlphabet with
         member _.Letters = "123"
         member me.WillPermit(value) = (value <> me.Letters)
      }

    try
      typeof<NanoId>
        .GetMethod("NewId", BindingFlags.Static ||| BindingFlags.NonPublic)
        .Invoke(null, [| badAlphabet; 21 |])
    with :? TargetInvocationException as x ->
      raise x.InnerException

  Prop.throws<ArgumentException, _> (lazy bypassWithBadInput ())

[<Property(MaxTest = 1)>]
let ``Default NanoId is 21 UrlSafe characters`` () =
  let SourceAlphabet alphabet & TargetSize size = NanoIdOptions.UrlSafe
  let value = NanoId.NewId().ToString()
  value.Length = size && alphabet.WillPermit(value)

[<Property>]
let ``Output size always equals input size`` (NonNegativeInt size) =
  let generated = NanoId.NewId(NanoIdOptions.UrlSafe.Resize size)
  let length = int generated.Length
  length = size |> Prop.label $"expect (%i{size}) <> actual (%i{length})"

[<Property(Arbitrary = [| typeof<Generation> |])>]
let ``Multiple instances are equal when their underlying values are equal``
  (RawNanoId input)
  =
  let one = NanoIdParser.UrlSafe.Parse(input)
  let two = NanoIdParser.UrlSafe.Parse(input)
  one = two |> Prop.label $"\nNot equal on '{input}'"

[<Property(Arbitrary = [| typeof<Generation> |])>]
let ``Multiple instances are ordered the same as their underlying values``
  (RawNanoId input1)
  (RawNanoId input2)
  (RawNanoId input3)
  =
  let value1 = NanoIdParser.UrlSafe.Parse(input1)
  let value2 = NanoIdParser.UrlSafe.Parse(input2)
  let value3 = NanoIdParser.UrlSafe.Parse(input3)

  let inputs = List.sort [ input1; input2; input3 ]
  let values =
    List.sort [
      yield! Option.toList value1
      yield! Option.toList value2
      yield! Option.toList value3
    ]

  values
  |> List.zip inputs
  |> List.forall (fun (raw, nano) -> raw = string nano)
  |> Prop.label $"\ninputs: {inputs}\nvalues: {values}"

[<Property(Arbitrary = [| typeof<Generation> |])>]
let ``Parse and ToString are invertible`` nanoId =
  match NanoIdParser.UrlSafe.Parse(string nanoId) with
  | Some nanoId' -> nanoId' = nanoId
  | None -> false
  |> Prop.label $"\nNot invertible: %A{nanoId}"

[<Property>]
let ``Parse returns error on invalid letters`` (PositiveInt count) =
  let input = String.replicate count "*"
  let parser =
    NanoIdParser.Create(Numbers)
    |> Result.defaultWith (fun x -> failwith $"{x}")

  match parser.Parse input with
  | None -> Prop.ofTestable true
  | Some nanoId -> false |> Prop.label $"\nParse Ok: '{nanoId}'"
