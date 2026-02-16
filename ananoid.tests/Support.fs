(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid.Tests

open global.Xunit

open Hedgehog
open Hedgehog.FSharp
open Hedgehog.Xunit

open pblasucci.Ananoid
open pblasucci.Ananoid.Core
open pblasucci.Ananoid.Core.Tagged
open pblasucci.Ananoid.KnownAlphabets


type Generation =
  static let validAlphabet : Gen<Alphabet> =
    Gen.item [
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

  static let taggedNanoId : Gen<string<nanoid>> =
    Gen.sized (fun size -> gen { return size |> nanoIdOf' Defaults.Alphabet })

  static member Configuration =
    AutoGenConfig.defaults
    |> AutoGenConfig.addGenerator validAlphabet
    |> AutoGenConfig.addGenerator taggedNanoId


type SmallNegativeIntAttribute() =
  inherit GenAttribute<int>()

  override _.Generator = Gen.int32 (Range.constant -100 -1)


type RawNanoIdAttribute() =
  inherit GenAttribute<string>()

  override _.Generator =
    Gen.sized (fun size -> gen {
      let! alphabet = Gen.autoWith<Alphabet> Generation.Configuration
      return size |> nanoIdOf alphabet.Letters
    })


[<assembly: CaptureConsole>]
do ()
