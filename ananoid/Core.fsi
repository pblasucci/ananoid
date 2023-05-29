(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
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
  val Alphabet : string = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz-"

  /// Twenty-one (21) single-byte characters.
  /// This is the default output lenght if one is not explicitly specified.
  [<Literal>]
  val Size : int = 21


/// <summary>
/// Generates a new identifier, <c>size</c> characters in length,
/// derived from the letters of the given alphabet
/// (note: a size of less than zero will result in an empty string).
/// </summary>
/// <exception cref="T:System.ArgumentOutOfRangeException">
/// Thrown when <c>alphabet</c> contains &lt; 1 letter, or &gt; 255 letters.
/// </exception>
[<CompiledName("NewNanoId")>]
val nanoIdOf : alphabet : string -> size  : int -> string

/// Generates a new identifier with the default alphabet and size.
[<CompiledName("NewNanoId")>]
val nanoId : unit -> string

/// Contains primitives for generating identifiers which are "tagged"
/// with a discriminator (useful for managing lots of string which have
/// different purposes, but where using a full CLR type is undesirable).
module Tagged =
  /// <summary>
  /// An abbreviation for the CLI type System.String.
  /// </summary>
  /// <remarks>
  /// <b>This alias is not usable from languages other than F#.</b>
  /// </remarks>
  [<CompiledName("string@measurealias")>]
  [<MeasureAnnotatedAbbreviation>]
  type string<[<Measure>] 'Tag> = string

  /// <summary>
  /// A "tag", which can be used as a discriminator.
  /// </summary>
  /// <remarks>
  /// <b>This tag is not usable from languages other than F#.</b>
  /// </remarks>
  [<CompiledName("nanoid@measure")>]
  [<Measure>]
  type nanoid =
    /// <summary>
    /// Applies the <c>nanoid</c> "tag" to a string.
    /// </summary>
    /// <remarks>
    /// <b>This function is not usable from languages other than F#.</b>
    /// </remarks>
    static member tag : value : string -> string<nanoid>

  /// <summary>
  /// Generates a new tagged identifier, <c>size</c> characters in length,
  /// derived from the letters of the given alphabet
  /// (note: a size of less than zero will result in an empty string).
  /// </summary>
  /// <remarks>
  /// <b>This function is not usable from languages other than F#.</b>
  /// </remarks>
  val nanoIdOf' : alphabet : string -> size  : int -> string<nanoid>

  /// <summary>
  /// Generates a new tagged identifier with the default alphabet and size.
  /// </summary>
  /// <remarks>
  /// <b>This function is not usable from languages other than F#.</b>
  /// </remarks>
  val nanoId' : unit -> string<nanoid>
