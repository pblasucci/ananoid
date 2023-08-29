(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.

  This Source Code Form maybe be re-used independent of other parts of
  the Ananoid project (https://github.com/pblasucci/ananoid), provided this
  disclaimer is maintained and the terms of the above license are respected.
*)

/// <summary>
/// Contains simple functions for generating 'nano identifier' strings
/// (a simple alternative to things like 'Universal Unique Identifiers').
/// </summary>
///
/// <namespacedoc>
///   <summary>
///   This library provides nano identifiers, an alternative to UUIDs (inspired
///   by <a href="https://github.com/ai/nanoid">github.com/ai/nanoid</a>).
///   </summary>
/// </namespacedoc>
///
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

let inline private (|Length|) value =
  if String.IsNullOrWhiteSpace value then
    Length(0ul)
  else
    Length(value.Trim() |> String.length |> uint32)

let private generate (Length length as alphabet) size =
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


/// Defines the recommended set of characters and output length
/// for optimally generating nano identifier strings.
[<RequireQualifiedAccess>]
module Defaults =
  /// <summary>
  /// An alphabet consisting of: URL-friendly numbers, English letters, and
  /// symbols (ie: <c>A-Za-z0-9_-</c>).
  /// This is the default alphabet if one is not explicitly specified.
  /// </summary>
  [<Literal>]
  let Alphabet =
    "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz-"

  /// Twenty-one (21) single-byte characters.
  /// This is the default output length if one is not explicitly specified.
  [<Literal>]
  let Size = 21


/// <summary>
/// Generates a new identifier, <c>size</c> characters in length,
/// derived from the letters of the given alphabet
/// (note: a size of less than one will result in an empty string).
/// </summary>
/// <remarks>
/// When viewed from other languages, this function is named <c>NewNanoId</c>.
/// </remarks>
/// <exception cref="T:System.ArgumentOutOfRangeException">
/// Thrown when <c>alphabet</c> contains &lt; 1 letter, or &gt; 255 letters.
/// </exception>
[<CompiledName("NewNanoId")>]
let nanoIdOf (Length length as alphabet) size =
  if size < 1 then ""
  elif length < 1u || 255u < length then outOfRange (nameof alphabet)
  else generate alphabet size

/// <summary>
/// Generates a new identifier with the default alphabet and size.
/// </summary>
/// <remarks>
/// When viewed from other languages, this function is named <c>NewNanoId</c>.
/// </remarks>
[<CompiledName("NewNanoId")>]
let nanoId () : string = nanoIdOf Defaults.Alphabet Defaults.Size


/// <summary>
/// Contains primitives for generating identifiers which are "tagged"
/// with a discriminator (useful for managing lots of string which have
/// different purposes, but where using a full CLR type is undesirable).
/// </summary>
/// <remarks>
/// <b>This module is not intended for languages other than F#.</b>
/// </remarks>
module Tagged =
  /// <summary>
  /// An abbreviation for the CLI type System.String.
  /// </summary>
  /// <remarks>
  /// <b>This alias is not intended for languages other than F#.</b>
  /// </remarks>
  [<CompiledName("string@measurealias'")>]
  [<MeasureAnnotatedAbbreviation>]
  type string<[<Measure>] 'Tag> = string

  /// <summary>
  /// A "tag", which can be used as a discriminator.
  /// </summary>
  /// <remarks>
  /// <b>This tag is not intended for languages other than F#.</b>
  /// </remarks>
  [<CompiledName("nanoid@measure'")>]
  [<Measure>]
  type nanoid =
    /// <summary>
    /// Applies the <c>nanoid</c> "tag" to a string.
    /// </summary>
    /// <remarks>
    /// <b>This function is not intended for languages other than F#.</b>
    /// </remarks>
    static member tag value = (# "" (value : string) : string<nanoid> #)

  /// <summary>
  /// Generates a new tagged identifier, <c>size</c> characters in length,
  /// derived from the letters of the given alphabet
  /// (note: a size of less than one will result in an empty string).
  /// </summary>
  /// <remarks>
  /// <b>This function is not intended for languages other than F#.</b>
  /// </remarks>
  /// <exception cref="T:System.ArgumentOutOfRangeException">
  /// Thrown when <c>alphabet</c> contains &lt; 1 letter, or &gt; 255 letters.
  /// </exception>
  let nanoIdOf' alphabet size = nanoid.tag (nanoIdOf alphabet size)

  /// <summary>
  /// Generates a new tagged identifier with the default alphabet and size.
  /// </summary>
  /// <remarks>
  /// <b>This function is not intended for languages other than F#.</b>
  /// </remarks>
  let nanoId' () = nanoIdOf' Defaults.Alphabet Defaults.Size


/// Pre-defined alphabets commonly used to generate identifiers.
module Alphabets =
  // Combination of all the lowercase, uppercase characters and numbers
  /// from 0 to 9, not including any symbols or special characters.
  [<Literal>]
  let Alphanumeric =
    "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

  /// Hexadecimal lowercase characters: 0123456789abcdef.
  [<Literal>]
  let HexadecimalLowercase = "0123456789abcdef"

  /// Hexadecimal uppercase characters: 0123456789ABCDEF.
  [<Literal>]
  let HexadecimalUppercase = "0123456789ABCDEF"

  /// Lowercase English letters: abcdefghijklmnopqrstuvwxyz.
  [<Literal>]
  let Lowercase = "abcdefghijklmnopqrstuvwxyz"

  /// Numbers, Uppercase, and Lowercase without "lookalikes":
  /// 1, l, I, 0, O, o, u, v, 5, S, s, 2, Z.
  /// Complete set: 346789ABCDEFGHJKLMNPQRTUVWXYabcdefghijkmnpqrtwxyz.
  [<Literal>]
  let NoLookalikes = "346789ABCDEFGHJKLMNPQRTUVWXYabcdefghijkmnpqrtwxyz"

  /// Same as Nolookalikes -- but having removed vowels and: 3, 4, x, X, V.
  /// Complete set: 6789BCDFGHJKLMNPQRTWbcdfghjkmnpqrtwz
  [<Literal>]
  let NoLookalikesSafe = "6789BCDFGHJKLMNPQRTWbcdfghjkmnpqrtwz"

  /// Numbers from 0 to 9.
  [<Literal>]
  let Numbers = "0123456789"

  /// Uppercase English letters: ABCDEFGHIJKLMNOPQRSTUVWXYZ.
  [<Literal>]
  let Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

  /// <summary>
  /// URL-friendly numbers, English letters, and symbols: <c>A-Za-z0-9_-</c>.
  /// This is the default alphabet if one is not explicitly specified.
  /// </summary>
  [<Literal>]
  let UrlSafe = Defaults.Alphabet
