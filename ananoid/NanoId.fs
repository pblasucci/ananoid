(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open System
open System.Runtime.CompilerServices
open System.Runtime.InteropServices


[<NoComparison>]
type NanoIdOptions = {
  Alphabet' : IAlphabet
  Size' : int
} with
  member me.Alphabet = me.Alphabet'
  member me.Size = me.Size'

  member me.Resize(size) = { me with Size' = max 0 size }

  static member Create(alphabet : IAlphabet, size) =
    alphabet
    |> Alphabet.Validate
    |> Result.map (fun letters -> { Alphabet' = letters; Size' = max 0 size })

  [<CompiledName("CreateOrThrow")>]
  static member CreateOrRaise(alphabet, size) =
    NanoIdOptions.Create(alphabet, size)
    |> Result.defaultWith (fun e -> e.Promote alphabet)

  static member UrlSafe = { Alphabet' = UrlSafe; Size' = Core.Defaults.Size }

  static member Numbers = { NanoIdOptions.UrlSafe with Alphabet' = Numbers }

  static member HexadecimalLowercase = {
    NanoIdOptions.UrlSafe with
        Alphabet' = HexadecimalLowercase
  }

  static member HexadecimalUppercase = {
    NanoIdOptions.UrlSafe with
        Alphabet' = HexadecimalUppercase
  }

  static member Lowercase = { NanoIdOptions.UrlSafe with Alphabet' = Lowercase }

  static member Uppercase = { NanoIdOptions.UrlSafe with Alphabet' = Uppercase }

  static member Alphanumeric = {
    NanoIdOptions.UrlSafe with
        Alphabet' = Alphanumeric
  }

  static member NoLookalikes = {
    NanoIdOptions.UrlSafe with
        Alphabet' = NoLookalikes
  }

  static member NoLookalikesSafe = {
    NanoIdOptions.UrlSafe with
        Alphabet' = NoLookalikesSafe
  }


[<IsReadOnly>]
[<Struct>]
type NanoId(value : string, length : uint32) =
  member _.Length = length

  override _.ToString() = let (Trimmed value') = value in value'

  static member op_Implicit(nanoId : NanoId) = string nanoId

  static member IsEmpty(nanoId : NanoId) = (nanoId.Length = 0u)

  static member Empty = NanoId()

  static member internal NewId(alphabet, size) =
    match Alphabet.Validate alphabet with
    | Ok _ when size < 1 -> NanoId.Empty
    | Ok a ->
      let nanoid & Length n = Core.nanoIdOf a.Letters size
      NanoId(nanoid, n)
    //NOTE Since this overload of NewId is never publicly exposed,
    // ... this exception is only possible to encounter via reflection!
    | Error reason -> invalidArg (nameof alphabet) (string reason)

  static member NewId(options) = NanoId.NewId(options.Alphabet', options.Size')

  static member NewId() = NanoId.NewId(NanoIdOptions.UrlSafe)


[<Sealed>]
type NanoIdParser(alphabet : IAlphabet) =
  member _.Alphabet = alphabet

  member me.Parse(value) =
    match value with
    | Empty when alphabet.WillPermit("") -> Some NanoId.Empty
    | Trimmed t & Length n when alphabet.WillPermit(t) -> Some(NanoId(t, n))
    | _ -> None

  member me.TryParse(value, [<Out>] nanoId : outref<_>) =
    let result = me.Parse(value)
    nanoId <- result |> Option.defaultValue NanoId.Empty
    Option.isSome result

  static member Alphanumeric = NanoIdParser(Alphanumeric)

  static member HexadecimalLowercase = NanoIdParser(HexadecimalLowercase)

  static member HexadecimalUppercase = NanoIdParser(HexadecimalUppercase)

  static member Lowercase = NanoIdParser(Lowercase)

  static member NoLookalikes = NanoIdParser(NoLookalikes)

  static member NoLookalikesSafe = NanoIdParser(NoLookalikesSafe)

  static member Numbers = NanoIdParser(Numbers)

  static member Uppercase = NanoIdParser(Uppercase)

  static member UrlSafe = NanoIdParser(UrlSafe)

  static member Create(alphabet) =
    alphabet |> Alphabet.Validate |> Result.map NanoIdParser

  [<CompiledName("CreateOrThrow")>]
  static member CreateOrRaise(alphabet) =
    NanoIdParser.Create(alphabet)
    |> Result.defaultWith (fun e -> e.Promote alphabet)


[<AutoOpen>]
module NanoIdOptionsPatterns =
  let inline (|SourceAlphabet|)
    (source : 'Source when 'Source : (member Alphabet : IAlphabet))
    =
    SourceAlphabet(source.Alphabet)

  let (|TargetSize|) { Size' = size } = TargetSize(size)


[<Extension>]
[<Sealed>]
type NanoIdOptionsExtensions =
  [<CompilerMessage("Not intended for use from F#", 9999, IsHidden = true)>]
  [<Extension>]
  static member Deconstruct
    (
      options : NanoIdOptions,
      alphabet : outref<IAlphabet>,
      targetSize : outref<int>
    )
    =
    alphabet <- options.Alphabet
    targetSize <- options.Size


[<Extension>]
[<Sealed>]
type IAlphabetExtensions =
  [<CompiledName("ToNanoIdFactory@FSharpFunc")>]
  [<Extension>]
  static member ToNanoIdFactory(alphabet) =
    Alphabet.Validate(alphabet)
    |> Result.map (fun a size -> NanoId.NewId(a, size))

  [<CompilerMessage("Not intended for use from F#", 9999, IsHidden = true)>]
  [<CompiledName("ToNanoIdFactory")>]
  [<Extension>]
  static member ToNanoIdFactoryDelegate(alphabet) =
    match alphabet.ToNanoIdFactory() with
    | Error e -> e.Promote(alphabet)
    | Ok func -> Func<_, _> func


[<assembly : Extension>]
do ()
