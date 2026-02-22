(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
module pblasucci.Ananoid.Tests.Alphabet

open Hedgehog.FSharp
open Hedgehog.Xunit
open global.Xunit
open pblasucci.Ananoid.Tests

(* ⮟ system under test ⮟ *)
open pblasucci.Ananoid


[<Fact>]
let ``Custom alphabet fails if letter-set is null`` () =
  match Alphabet.ofLetters (null |> box |> unbox) with
  // ⮟ pass ⮟
  | Error(AlphabetTooSmall _) -> ()
  // ⮟ fail ⮟
  | Error unexpected -> Assert.Fail($"Failed, unexpectedly: %s{unexpected.Message}")
  | Ok shouldNotHave -> Assert.Fail($"Did not fail; generated: %s{shouldNotHave.Letters}")

[<Fact>]
let ``Custom alphabet fails if too short`` () =
  match Alphabet.ofLetters "" with
  // ⮟ pass ⮟
  | Error(AlphabetTooSmall _) -> ()
  // ⮟ fail ⮟
  | Error unexpected -> Assert.Fail($"Failed, unexpectedly: %s{unexpected.Message}")
  | Ok shouldNotHave -> Assert.Fail($"Did not fail; generated: %s{shouldNotHave.Letters}")

[<Fact>]
let ``Custom alphabet fails if too large`` () =
  let tooLarge = "qwerty123" |> String.replicate 512

  match tooLarge.ToAlphabet() with
  // ⮟ pass ⮟
  | Error(AlphabetTooLarge _) -> ()
  // ⮟ fail ⮟
  | Error unexpected -> Assert.Fail($"Failed, unexpectedly: %s{unexpected.Message}")
  | Ok shouldNotHave -> Assert.Fail($"Did not fail; generated: %s{shouldNotHave.Letters}")

[<Property(typeof<Generation>)>]
let ``All pre-defined alphabets produce comprehensible outputs`` alphabet =
  property {
    let generated = alphabet |> Alphabet.makeNanoId Core.Defaults.Size
    let _parsed = alphabet.ParseNanoId(string generated)
    counterexample $"{alphabet} failed to validate given letters. ({generated})"
    _parsed = Some generated
  }
