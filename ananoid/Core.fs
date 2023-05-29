(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
module pblasucci.Ananoid.Core

#nowarn "9" (* unverifiable IL - see `Core.stackspan` function for details *)
#nowarn "42" (* inline IL -- see `Tagged.nanoid.tag` function for details *)

open System
open System.Security.Cryptography
open Microsoft.FSharp.NativeInterop

open type System.Numerics.BitOperations


let inline private outOfRange paramName =
  raise (ArgumentOutOfRangeException paramName)

let inline private stackspan<'T when 'T : unmanaged> size =
  Span<'T>(size |> NativePtr.stackalloc<'T> |> NativePtr.toVoidPtr, size)

let inline (|Length|) value =
  if String.IsNullOrWhiteSpace value then
    Length(0ul)
  else
    Length(value.Trim() |> String.length |> uint32)

let private generate (alphabet & Length length) size =
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


[<RequireQualifiedAccess>]
module Defaults =
  [<Literal>]
  let Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz-"

  [<Literal>]
  let Size = 21


[<CompiledName("NewNanoId")>]
let nanoIdOf (Length length as alphabet) size =
  if size < 1 then ""
  elif length < 1u || 255u < length then outOfRange (nameof alphabet)
  else generate alphabet size

[<CompiledName("NewNanoId")>]
let nanoId () = nanoIdOf Defaults.Alphabet Defaults.Size


module Tagged =
  [<CompiledName("string@measurealias")>]
  [<MeasureAnnotatedAbbreviation>]
  type string<[<Measure>] 'Tag> = string

  [<CompiledName("nanoid@measure")>]
  [<Measure>]
  type nanoid =
    static member tag value = (# "" (value : string) : string<nanoid> #)

  let nanoIdOf' alphabet size = nanoid.tag (nanoIdOf alphabet size)

  let nanoId' () = nanoIdOf' Defaults.Alphabet Defaults.Size
