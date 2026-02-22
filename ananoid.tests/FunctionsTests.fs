(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
module pblasucci.Ananoid.Tests.CoreTests

open global.Xunit

open System

open Hedgehog.FSharp
open Hedgehog.Xunit

(* ⮟ system under test ⮟ *)
open pblasucci.Ananoid
open pblasucci.Ananoid.KnownAlphabets
open pblasucci.Ananoid.Core
open pblasucci.Ananoid.Core.Tagged


[<Property(typeof<Generation>)>]
let ``Tagged output equals untagged output`` (tagged : string<nanoid>) =
  let untagged = string tagged
  tagged = nanoid.tag untagged

[<Property>]
let ``Returns empty on negative size`` ([<SmallNegativeInt>] size) =
  match nanoIdOf Alphabets.Numbers size with
  | "" -> Property.success ()
  | value -> Property.counterexample (fun () -> $"generated '%s{value}'")

[<Fact>]
let ``Raises exception on zero-length alphabet`` () =
  Assert.Throws<ArgumentOutOfRangeException>(fun () ->
    let nanoId = nanoIdOf (String.replicate 52 " ") Defaults.Size
    printfn $"Expected exn, but got: %A{nanoId}"
  )

[<Fact>]
let ``Raises exception on over-large alphabet`` () =
  Assert.Throws<ArgumentOutOfRangeException>(
    "alphabet",
    (fun () -> nanoIdOf (String.replicate 1024 "-") 21 |> printfn "%s")
  )

[<Fact>]
let ``Default is UrlSafe alphabet of size 21`` () =
  let value = nanoId ()
  Assert.Multiple(
    (fun () -> Assert.Equal(21, value.Length)),
    (fun () ->
      // NOTE "can be parsed" is a good-enough proxy for "is of alphabet"
      Assert.True(UrlSafe |> Alphabet.parseNanoId value |> Option.isSome)
    )
  )

[<Fact>]
let ``Tagged default is UrlSafe alphabet of size 21`` () =
  let value = string (nanoId' ())
  Assert.Multiple(
    (fun () -> Assert.Equal(21, value.Length)),
    (fun () ->
      // NOTE "can be parsed" is a good-enough proxy for "is of alphabet"
      Assert.True(value |> NanoId.parseAs UrlSafe |> Option.isSome)
    )
  )
