(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open Microsoft.FSharp.Core
open System
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System.Text.RegularExpressions

#nowarn "9" (* unverifiable IL - see `Library.stackspan` function for details *)
#nowarn "42" (* inline IL -- see `Tagged.nanoid.tag` function for details *)


[<AutoOpen>]
module Library =
  open Microsoft.FSharp.NativeInterop

  let inline outOfRange paramName =
    raise (ArgumentOutOfRangeException paramName)

  let inline stackspan<'T when 'T : unmanaged> size =
    Span<'T>(size |> NativePtr.stackalloc<'T> |> NativePtr.toVoidPtr, size)

  let inline (|Empty|_|) value =
    if String.IsNullOrWhiteSpace value then Some() else None

  let inline (|Trimmed|) (value : string) =
    Trimmed(if String.IsNullOrWhiteSpace value then "" else value.Trim())

  let inline (|Length|) (Trimmed trimmed) = Length(uint32 trimmed.Length)


type IAlphabet =
  abstract Letters : string
  abstract WillPermit : value : string -> bool


module CharSets =
  [<Literal>]
  let UrlSafe =
    "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz-"

  [<Literal>]
  let Numbers = "0123456789"

  [<Literal>]
  let HexadecimalLowercase = "0123456789abcdef"

  [<Literal>]
  let HexadecimalUppercase = "0123456789ABCDEF"

  [<Literal>]
  let Lowercase = "abcdefghijklmnopqrstuvwxyz"

  [<Literal>]
  let Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

  [<Literal>]
  let Alphanumeric =
    "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

  [<Literal>]
  let NoLookalikes = "346789ABCDEFGHJKLMNPQRTUVWXYabcdefghijkmnpqrtwxyz"

  [<Literal>]
  let NoLookalikesSafe = "6789BCDFGHJKLMNPQRTWbcdfghjkmnpqrtwz"


module Patterns =
  [<Literal>]
  let UrlSafe = "^[a-zA-Z0-9_-]+$"

  [<Literal>]
  let Numbers = "^[0-9]+$"

  [<Literal>]
  let HexadecimalLowercase = "^[0-9abcdef]+$"

  [<Literal>]
  let HexadecimalUppercase = "^[0-9ABCDEF]+$"

  [<Literal>]
  let Lowercase = "^[a-z]+$"

  [<Literal>]
  let Uppercase = "^[A-Z]+$"

  [<Literal>]
  let Alphanumeric = "^[a-zA-Z0-9]+$"

  [<Literal>]
  let NoLookalikes = "^[346789ABCDEFGHJKLMNPQRTUVWXYabcdefghijkmnpqrtwxyz]+$"

  [<Literal>]
  let NoLookalikesSafe = "^[6789BCDFGHJKLMNPQRTWbcdfghjkmnpqrtwz]+$"


type AlphabetError =
  | AlphabetTooLarge
  | AlphabetTooSmall
  | IncoherentAlphabet
  member me.Message =
    match me with
    | AlphabetTooLarge -> "Alphabet may not contain more than 255 letters."
    | AlphabetTooSmall -> "Alphabet must contain at least one letter."
    | IncoherentAlphabet -> "Alphabet cannot validate its own letters."
  override me.ToString() =
    let case =
      match me with
      | AlphabetTooLarge -> nameof AlphabetTooLarge
      | AlphabetTooSmall -> nameof AlphabetTooSmall
      | IncoherentAlphabet -> nameof IncoherentAlphabet
    $"{nameof AlphabetError}.{case} '{me.Message}'"


type Alphabet =
  | Alphanumeric
  | HexadecimalLowercase
  | HexadecimalUppercase
  | Lowercase
  | NoLookalikes
  | NoLookalikesSafe
  | Numbers
  | Uppercase
  | UrlSafe

  override me.ToString() = (me :> IAlphabet).Letters

  interface IAlphabet with
    member me.Letters =
      match me with
      | Alphanumeric -> CharSets.Alphanumeric
      | UrlSafe -> CharSets.UrlSafe
      | HexadecimalLowercase -> CharSets.HexadecimalLowercase
      | HexadecimalUppercase -> CharSets.HexadecimalUppercase
      | Lowercase -> CharSets.Lowercase
      | NoLookalikes -> CharSets.NoLookalikes
      | NoLookalikesSafe -> CharSets.NoLookalikesSafe
      | Numbers -> CharSets.Numbers
      | Uppercase -> CharSets.Uppercase

    member me.WillPermit(value) =
      match value with
      | Empty -> true
      | Trimmed raw ->
        let spec =
          match me with
          | Alphanumeric -> Patterns.Alphanumeric
          | UrlSafe -> Patterns.UrlSafe
          | HexadecimalLowercase -> Patterns.HexadecimalLowercase
          | HexadecimalUppercase -> Patterns.HexadecimalUppercase
          | Lowercase -> Patterns.Lowercase
          | NoLookalikes -> Patterns.NoLookalikes
          | NoLookalikesSafe -> Patterns.NoLookalikesSafe
          | Numbers -> Patterns.Numbers
          | Uppercase -> Patterns.Uppercase
        Regex.IsMatch(raw, spec, RegexOptions.Compiled, TimeSpan.FromSeconds 1)

  static member Validate(alphabet : IAlphabet) =
    if isNull (alphabet :> obj) then
      Error AlphabetTooSmall
    elif alphabet.Letters.Length < 1 then
      Error AlphabetTooSmall
    elif 255 < alphabet.Letters.Length then
      Error AlphabetTooLarge
    elif not (alphabet.WillPermit alphabet.Letters) then
      Error IncoherentAlphabet
    else
      Ok alphabet


[<NoComparison>]
type NanoIdOptions =
  {
    Alphabet' : IAlphabet
    Size' : int
  }
  member me.Alphabet = me.Alphabet'
  member me.Size = me.Size'

  member me.Resize(size) = { me with Size' = max 0 size }

  static member Of(alphabet : IAlphabet, size) =
    alphabet
    |> Alphabet.Validate
    |> Result.map (fun letters -> { Alphabet' = letters; Size' = max 0 size })

  static member UrlSafe = { Alphabet' = UrlSafe; Size' = 21 }

  static member Numbers = { NanoIdOptions.UrlSafe with Alphabet' = Numbers }

  static member HexadecimalLowercase =
    { NanoIdOptions.UrlSafe with Alphabet' = HexadecimalLowercase }

  static member HexadecimalUppercase =
    { NanoIdOptions.UrlSafe with Alphabet' = HexadecimalUppercase }

  static member Lowercase = { NanoIdOptions.UrlSafe with Alphabet' = Lowercase }

  static member Uppercase = { NanoIdOptions.UrlSafe with Alphabet' = Uppercase }

  static member Alphanumeric =
    { NanoIdOptions.UrlSafe with Alphabet' = Alphanumeric }

  static member NoLookalikes =
    { NanoIdOptions.UrlSafe with Alphabet' = NoLookalikes }

  static member NoLookalikesSafe =
    { NanoIdOptions.UrlSafe with Alphabet' = NoLookalikesSafe }


module Core =
  open System.Security.Cryptography

  open type System.Numerics.BitOperations
  open type NanoIdOptions

  let generate (alphabet & Length length) size =
    let mask = (2 <<< 31 - LeadingZeroCount((length - 1u) ||| 1u)) - 1
    let step = int (ceil ((1.6 * float mask * float size) / float length))

    let nanoid = stackspan<char> size
    let mutable nanoidCount = 0

    let buffer = stackspan<byte> step
    let mutable bufferCount = 0

    while nanoidCount < size do
      RandomNumberGenerator.Fill(buffer)
      bufferCount <- 0

      while nanoidCount < size && bufferCount < step do
        let index = int buffer[bufferCount] &&& mask
        bufferCount <- bufferCount + 1

        if index < int length then
          nanoid[nanoidCount] <- alphabet[index]
          nanoidCount <- nanoidCount + 1

    nanoid.ToString()

  [<CompiledName("NewNanoId")>]
  let nanoIdOf (alphabet & Length length) size =
    if size < 1 then ""
    elif length < 1u || 255u < length then outOfRange (nameof alphabet)
    else generate alphabet size

  [<CompiledName("NewNanoId")>]
  let nanoId () = nanoIdOf UrlSafe.Alphabet.Letters UrlSafe.Size


  module Tagged =
    [<CompiledName("string@measurealias")>]
    [<MeasureAnnotatedAbbreviation>]
    type string<[<Measure>] 'Tag> = string

    [<CompiledName("nanoid@measure")>]
    [<Measure>]
    type nanoid =
      static member tag value = (# "" (value : string) : string<nanoid> #)

    let nanoIdOf' alphabet size = nanoid.tag (nanoIdOf alphabet size)

    let nanoId' () = nanoIdOf' UrlSafe.Alphabet.Letters UrlSafe.Size


[<IsReadOnly>]
[<Struct>]
type NanoId(value : string, length : uint32) =
  member _.Length = length

  override _.ToString() = let (Trimmed value') = value in value'

  static member op_Implicit(nanoId : NanoId) = string nanoId

  static member IsEmpty(nanoId : NanoId) = (nanoId.Length = 0u)

  static member Empty = NanoId()

  static member NewId(alphabet, size) =
    match Alphabet.Validate alphabet with
    | Ok _ when size < 1 -> NanoId.Empty
    | Ok a ->
      let Trimmed t & Length n = Core.generate a.Letters size
      NanoId(t, n)
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

  static member Of(alphabet) =
    alphabet |> Alphabet.Validate |> Result.map NanoIdParser


[<AutoOpen>]
[<Extension>]
module NanoIdOptions =
  let inline (|SourceAlphabet|)
    (source : 'Source when 'Source : (member Alphabet : IAlphabet))
    =
    SourceAlphabet(source.Alphabet)

  let (|TargetSize|) { Size' = size } = TargetSize(size)

  [<CompilerMessage("Not intended for use from F#", 9999, IsHidden = true)>]
  [<Extension>]
  let Deconstruct
    { Alphabet' = alphabet'; Size' = size' }
    (alphabet : outref<IAlphabet>)
    (targetSize : outref<int>)
    =
    alphabet <- alphabet'
    targetSize <- size'


[<Extension>]
[<Sealed>]
type IAlphabetExtensions =
  [<CompiledName("ToNanoIdFactory@FSharpFunc")>]
  [<Extension>]
  static member ToNanoIdFactory(alphabet) =
    alphabet
    |> Alphabet.Validate
    |> Result.map (fun a size -> NanoId.NewId(a, max 0 size))

  [<CompilerMessage("Not intended for use from F#", 9999, IsHidden = true)>]
  [<CompiledName("ToNanoIdFactory")>]
  [<Extension>]
  static member ToNanoIdFactoryDelegate(alphabet) =
    alphabet.ToNanoIdFactory() |> Result.map (fun func -> Func<_, _> func)
