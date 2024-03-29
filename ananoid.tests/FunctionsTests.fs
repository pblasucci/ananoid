(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
module pblasucci.Ananoid.Tests.CoreTests

open System
open FsCheck
open FsCheck.Xunit

(* ⮟ system under test ⮟ *)
open pblasucci.Ananoid
open pblasucci.Ananoid.KnownAlphabets
open pblasucci.Ananoid.Core
open pblasucci.Ananoid.Core.Tagged


[<Property>]
let ``Returns empty on negative size`` (NegativeInt size) =
  let value = nanoIdOf Alphabets.Numbers size
  value |> String.IsNullOrWhiteSpace |> Prop.label $"generated '%s{value}'"

[<Property>]
let ``Raises exception on zero-length alphabet`` (NonNegativeInt count) =
  lazy (nanoIdOf (String.replicate count " ") 21)
  |> Prop.throws<ArgumentOutOfRangeException, _>

[<Property(MaxTest = 1)>]
let ``Raises exception on over-large alphabet`` () =
  lazy (nanoIdOf (String.replicate 1024 "-") 21)
  |> Prop.throws<ArgumentOutOfRangeException, _>

[<Property(MaxTest = 1)>]
let ``Default is UrlSafe alphabet of size 21`` () =
  let value = nanoId ()
  let parsed = UrlSafe |> Alphabet.parseNanoId value
  value.Length = 21 && Option.isSome parsed

[<Property(Arbitrary = [| typeof<Generation> |])>]
let ``Tagged output equals untagged output`` (TaggedNanoId tagged) =
  let untagged = string tagged
  tagged = nanoid.tag untagged

[<Property(MaxTest = 1)>]
let ``Tagged default is UrlSafe alphabet of size 21`` () =
  let value = string (nanoId' ())
  let parsed = value |> NanoId.parseAs UrlSafe
  value.Length = 21 && Option.isSome parsed
