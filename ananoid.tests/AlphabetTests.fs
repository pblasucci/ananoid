(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
module pblasucci.Ananoid.Tests.Alphabet

open System
open FsCheck
open FsCheck.Xunit

(* ⮟ system under test ⮟ *)
open pblasucci.Ananoid
open pblasucci.Ananoid.Tests
open FsCheck.Prop


[<Property(MaxTest = 1)>]
let ``Custom alphabet fails if letter set is null`` () =
  match Alphabet.ofLetters null with
  // ⮟ pass ⮟
  | Error(AlphabetTooSmall _) -> Prop.ofTestable true
  // ⮟ fail ⮟
  | Error unexpectedly -> false |> Prop.label $"%A{unexpectedly}"
  | Ok tooShortOptions -> false |> Prop.label $"%A{tooShortOptions}"

[<Property(MaxTest = 1)>]
let ``Custom alphabet fails if too short`` () =
  match Alphabet.ofLetters "" with
  // ⮟ pass ⮟
  | Error(AlphabetTooSmall _) -> Prop.ofTestable true
  // ⮟ fail ⮟
  | Error unexpectedly -> false |> Prop.label $"%A{unexpectedly}"
  | Ok tooShortOptions -> false |> Prop.label $"%A{tooShortOptions}"

[<Property(MaxTest = 1)>]
let ``Custom alphabet fails if too large`` () =
  let tooLarge = "qwerty123" |> String.replicate 512

  match tooLarge.ToAlphabet() with
  // ⮟ pass ⮟
  | Error(AlphabetTooLarge _) -> Prop.ofTestable true
  // ⮟ fail ⮟
  | Error unexpectedly -> false |> Prop.label $"%A{unexpectedly}"
  | Ok tooShortOptions -> false |> Prop.label $"%A{tooShortOptions}"

[<Property(Arbitrary = [| typeof<Generation> |])>]
let ``All pre-defined alphabets produce comprehensible outputs`` alphabet =
  let generated = alphabet |> Alphabet.makeNanoId Core.Defaults.Size
  let parsed = alphabet.ParseNanoId(string generated)
  parsed = Some generated
  |> Prop.label $"{alphabet} failed to validate given letters. ({generated})"
