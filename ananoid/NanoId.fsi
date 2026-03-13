(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid


open System.Diagnostics.CodeAnalysis
open System.Runtime.CompilerServices
open System.Runtime.InteropServices


/// Represents a unique textual identifier, with a known length,
/// based on a particular alphabet (i.e. a set of letters).
[<IsReadOnly>]
[<Struct>]
type NanoId =
  /// The number of characters in this instance.
  member Length : int

  /// The zero-valued instance of this type.
  static member Empty : NanoId

  /// <summary>
  /// Creates a new instance using the default alphabet and size
  /// (nb: the default alphabet is URL-friendly numbers, English letters, and
  /// symbols: <c>A-Za-z0-9_-</c> and the default size is 21).
  /// </summary>
  static member NewId : unit -> NanoId

  /// Returns true, when the given nanoId is zero-valued; otherwise, false.
  static member IsEmpty : nanoId : NanoId -> bool


/// Captures alphabet validation failure,
/// either because the alphabet had too few (&lt; 1) or too many (&gt; 255) non-whitespace characters.
[<Sealed>]
type InvalidAlphabet =
  /// The alphabet which lead to the exception.
  member Letters : string


/// Contains active patterns to simplify working with alphabets.
module AlphabetPatterns =
  /// Extracts the set of letters from an alphabet (regardless of said alphabet's validity)
  val inline (|Letters|) : hasLetters : ^T -> string when ^T : (member Letters : string)


/// <summary>
/// Represents a validated set of 'letters' from which an identifier is made
/// (for details, see <see cref="M:pblasucci.Ananoid.Alphabet.Validate" />).
/// </summary>
[<Sealed>]
type Alphabet =
  /// The validated letter set in this instance.
  member Letters : string

  /// <summary>
  /// Creates a new <see cref="T:pblasucci.Ananoid.NanoId"/> instance
  /// of the given size whose letters are taken from the current alphabet.
  /// </summary>
  /// <param name="size">
  /// The length of a generated identifier, in number of characters
  /// (note: negative values are changed to zero).
  /// </param>
  member MakeNanoId : size : int -> NanoId

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using the current alphabet to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <returns>
  /// On successful parsing, returns a <c>NanoId</c> instance;
  /// otherwise, returns <c>None</c>.
  /// </returns>
  /// <remarks>
  /// If the input <c>value</c> is empty (ie: <c>null</c>, zero-length, or
  /// consists only of whitespace), parsing is considered to succeed
  /// (ie: the method will return <c>Some NanoId.Empty</c>). This mirrors
  /// the behavior of <see cref="M:pblasucci.Ananoid.Alphabet.MakeNanoId"/>
  /// </remarks>
  member ParseNanoId : value : string -> NanoId option

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using the current alphabet to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <returns>
  /// On successful parsing, returns a <c>NanoId</c> instance;
  /// otherwise, returns <c>None</c>.
  /// </returns>
  /// <remarks>
  /// If the input <c>value</c> is empty (ie: <c>null</c>, zero-length, or
  /// consists only of whitespace), parsing is considered to fail
  /// (ie: the method will return <c>None</c>).
  /// </remarks>
  member ParseNonEmptyNanoId : value : string -> NanoId option

  /// <summary>
  /// Builds a new <see cref="T:pblasucci.Ananoid.Alphabet"/> from
  /// the given letter set after checking that it upholds certain
  /// invariants which are necessary for the algorithm to work well.
  /// </summary>
  /// <remarks>
  /// An alphabet's letters MUST uphold the following invariants:
  /// <list type="bullet">
  /// <item>Is not <c>null</c></item>
  /// <item>Contains at least one (1) non-whitespace letter</item>
  /// <item>Contains no more then 255 letters</item>
  /// </list>
  /// </remarks>
  /// <param name="letters">
  /// The letter set which will ultimately be used to generate
  /// <see cref="T:pblasucci.Ananoid.NanoId"/> instances.
  /// </param>
  /// <returns>
  /// On successful validation, returns a <see cref="T:pblasucci.Ananoid.Alphabet"/>;
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.InvalidAlphabet"/>
  /// with further details about what went wrong.
  /// </returns>
  static member Validate : letters : string -> Result<Alphabet, InvalidAlphabet>


/// Contains utilities for working with nano identifiers.
[<RequireQualifiedAccess>]
module NanoId =
  /// The number of characters in the given nanoId instance.
  val length : nanoId : NanoId -> int

  /// Returns true, when the given nanoId is zero-valued; otherwise, false.
  val isEmpty : nanoId : NanoId -> bool

  /// <summary>
  /// Creates a new instance using the default alphabet and size
  /// (nb: the default alphabet is URL-friendly numbers, English letters, and
  /// symbols: <c>A-Za-z0-9_-</c> and the default size is 21).
  /// </summary>
  val ofDefaults : unit -> NanoId

  /// <summary>
  /// Creates a new <see cref="T:pblasucci.Ananoid.NanoId"/> instance
  /// of the given size whose letters are taken from the given alphabet.
  /// </summary>
  /// <param name="alphabet">
  /// The letters from which the new <c>NanoId</c> will be generated.
  /// </param>
  /// <param name="size">
  /// The length of a generated identifier, in number of characters
  /// (note: negative values are changed to zero).
  /// </param>
  val ofOptions : alphabet : Alphabet -> size : int -> NanoId

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using the given alphabet to guide validation.
  /// </summary>
  /// <param name="alphabet">The letters to check against for validity.</param>
  /// <param name="value">The raw string to be converted.</param>
  /// <returns>
  /// On successful parsing, returns a <c>NanoId</c> instance;
  /// otherwise, returns <c>None</c>.
  /// </returns>
  /// <remarks>
  /// If the given value is empty (ie: <c>null</c>, zero-length, or consists only
  /// of whitespace), parsing will succeed (ie: return <c>Some nanoId</c>).
  /// However, the resulting <c>NanoId</c> instance will be
  /// <see cref='P:pblasucci.Ananoid.NanoId.Empty'/>.
  /// </remarks>
  val parseAs : alphabet : Alphabet -> value : string -> NanoId option

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using the given alphabet to guide validation.
  /// </summary>
  /// <param name="alphabet">The letters to check against for validity.</param>
  /// <param name="value">The raw string to be converted.</param>
  /// <returns>
  /// On successful parsing, returns a <c>NanoId</c> instance;
  /// otherwise, returns <c>None</c>.
  /// </returns>
  /// <remarks>
  /// Parsing will "fail" (ie: return <c>None</c>) if the given value is empty
  /// (ie: <c>null</c>, zero-length, or consists only of whitespace).
  /// </remarks>
  val parseNonEmptyAs : alphabet : Alphabet -> value : string -> NanoId option


/// Contains utilities for working with alphabets.
[<RequireQualifiedAccess>]
module Alphabet =
  /// <summary>
  /// Builds a new <see cref="T:pblasucci.Ananoid.Alphabet"/> from
  /// the given letter set after checking that it upholds certain
  /// invariants which are necessary for the algorithm to work well.
  /// </summary>
  /// <remarks>
  /// An alphabet's letters MUST uphold the following invariants:
  /// <list type="bullet">
  /// <item>Is not <c>null</c></item>
  /// <item>Contains at least one (1) non-whitespace letter</item>
  /// <item>Contains no more then 255 letters</item>
  /// </list>
  /// </remarks>
  /// <param name="letters">
  /// The letter set which will ultimately be used to generate
  /// <see cref="T:pblasucci.Ananoid.NanoId"/> instances.
  /// </param>
  /// <returns>
  /// On successful validation, returns a <see cref="T:pblasucci.Ananoid.Alphabet"/>;
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.InvalidAlphabet"/>
  /// with further details about what went wrong.
  /// </returns>
  val ofLetters : letters : string -> Result<Alphabet, InvalidAlphabet>

  /// <summary>
  /// Builds a new <see cref="T:pblasucci.Ananoid.Alphabet"/> from
  /// the given letter set after checking that it upholds certain
  /// invariants which are necessary for the algorithm to work well.
  /// </summary>
  /// <remarks>
  /// An alphabet's letters MUST uphold the following invariants:
  /// <list type="bullet">
  /// <item>Is not <c>null</c></item>
  /// <item>Contains at least one (1) non-whitespace letter</item>
  /// <item>Contains no more then 255 letters</item>
  /// </list>
  /// </remarks>
  /// <param name="letters">
  /// The letter set which will ultimately be used to generate
  /// <see cref="T:pblasucci.Ananoid.NanoId"/> instances.
  /// </param>
  /// <exception cref="T:pblasucci.Ananoid.InvalidAlphabetException">
  /// Raised when the given alphabet fails to uphold an invariant.
  /// </exception>
  val makeOrRaise : letters : string -> Alphabet

  /// <summary>
  /// Creates a new <see cref="T:pblasucci.Ananoid.NanoId"/> instance
  /// of the given size whose letters are taken from the given alphabet.
  /// </summary>
  /// <param name="size">
  /// The length of a generated identifier, in number of characters
  /// (note: negative values are changed to zero).
  /// </param>
  /// <param name="alphabet">
  /// The letters from which the new <c>NanoId</c> will be generated.
  /// </param>
  val makeNanoId : size : int -> alphabet : Alphabet -> NanoId

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using a valid alphabet to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <param name="alphabet">The letters to check against for validity.</param>
  /// <returns>
  /// On successful parsing, returns a <c>NanoId</c> instance;
  /// otherwise, returns <c>None</c>.
  /// </returns>
  /// <remarks>
  /// If the given value is empty (ie: <c>null</c>, zero-length, or consists
  /// only of whitespace), parsing will succeed (ie: return <c>Some nanoId</c>).
  /// However, the resulting <c>NanoId</c> instance will be
  /// <see cref='P:pblasucci.Ananoid.NanoId.Empty'/>.
  /// </remarks>
  val parseNanoId : value : string -> alphabet : Alphabet -> NanoId option

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using a valid alphabet to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <param name="alphabet">The letters to check against for validity.</param>
  /// <returns>
  /// On successful parsing, returns a <c>NanoId</c> instance;
  /// otherwise, returns <c>None</c>.
  /// </returns>
  /// <remarks>
  /// Parsing will "fail" (ie: return <c>None</c>) if the given value is empty
  /// (ie: <c>null</c>, zero-length, or consists only of whitespace).
  /// </remarks>
  val parseNonEmptyNanoId : value : string -> alphabet : Alphabet -> NanoId option


/// <summary>
/// Contains utilities intended to simplify working with
/// <see cref="T:pblasucci.Ananoid.Alphabet" /> in languages other than F#.
/// </summary>
[<Sealed>]
type AlphabetExtensions =
  /// <summary>
  /// Builds a new <see cref="T:pblasucci.Ananoid.Alphabet"/> from
  /// the given letter set after checking that it upholds certain
  /// invariants which are necessary for the algorithm to work well.
  /// </summary>
  /// <remarks>
  /// An alphabet's letters MUST uphold the following invariants:
  /// <list type="bullet">
  /// <item>Is not <c>null</c></item>
  /// <item>Contains at least one (1) non-whitespace letter</item>
  /// <item>Contains no more then 255 letters</item>
  /// </list>
  /// </remarks>
  /// <param name="letters">
  /// The letter set which will ultimately be used to generate
  /// <see cref="T:pblasucci.Ananoid.NanoId"/> instances.
  /// </param>
  /// <returns>
  /// On successful validation, returns a <see cref="T:pblasucci.Ananoid.Alphabet"/>;
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.InvalidAlphabet"/>
  /// with further details about what went wrong.
  /// </returns>
  [<Extension>]
  static member ToAlphabet : letters : string -> Result<Alphabet, InvalidAlphabet>

  /// <summary>
  /// Builds a new <see cref="T:pblasucci.Ananoid.Alphabet"/> from
  /// the given letter set after checking that it upholds certain
  /// invariants which are necessary for the algorithm to work well.
  /// </summary>
  /// <remarks>
  /// An alphabet's letters MUST uphold the following invariants:
  /// <list type="bullet">
  /// <item>Is not <c>null</c></item>
  /// <item>Contains at least one (1) non-whitespace letter</item>
  /// <item>Contains no more then 255 letters</item>
  /// </list>
  /// </remarks>
  /// <param name="letters">
  /// The letter set which will ultimately be used to generate
  /// <see cref="T:pblasucci.Ananoid.NanoId"/> instances.
  /// </param>
  /// <exception cref="T:System.ArgumentOutOfRangeException">
  /// Raised when the given letters fail to uphold an invariant.
  /// </exception>
  [<Extension>]
  static member ToAlphabetOrThrow : letters : string -> Alphabet

  /// <summary>
  /// Builds a new <see cref="T:pblasucci.Ananoid.Alphabet"/> from
  /// the given letter set after checking that it upholds certain
  /// invariants which are necessary for the algorithm to work well.
  /// </summary>
  /// <remarks>
  /// An alphabet's letters MUST uphold the following invariants:
  /// <list type="bullet">
  /// <item>Is not <c>null</c></item>
  /// <item>Contains at least one (1) non-whitespace letter</item>
  /// <item>Contains no more then 255 letters</item>
  /// </list>
  /// </remarks>
  /// <param name="letters">
  /// The letter set which will ultimately be used to generate
  /// <see cref="T:pblasucci.Ananoid.NanoId"/> instances.
  /// </param>
  /// <param name="alphabet">
  /// On successful validation, this out parameter will be a <see cref="T:pblasucci.Ananoid.Alphabet"/>;
  /// otherwise, it will be <c>null</c>.
  /// </param>
  /// <returns>
  /// <c>true</c> if validation succeeded and an <see cref="T:pblasucci.Ananoid.Alphabet"/> was created;
  /// otherwise, returns <c>false</c>.
  /// </returns>
  [<Extension>]
  static member TryMakeAlphabet :
    letters : string * [<Out; NotNullWhen(returnValue = true)>] alphabet : outref<Alphabet | null> -> bool

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using the current alphabet to guide validation.
  /// </summary>
  /// <param name="alphabet">The letters to check against for validity.</param>
  /// <param name="value">The raw string to be converted.</param>
  /// <param name="nanoId">
  /// On successful parsing, contains a <c>NanoId</c> instance.
  /// </param>
  /// <returns>
  /// <c>true</c> if parsing succeeded; <c>false</c> otherwise.
  /// </returns>
  /// <remarks>
  /// If the input <c>value</c> is empty (ie: <c>null</c>, zero-length, or
  /// consists only of whitespace), parsing is considered to succeed (ie: the
  /// method will return <c>true</c>). However, the resulting <c>NanoId</c>
  /// instance will be <see cref='P:pblasucci.Ananoid.NanoId.Empty'/>.
  /// </remarks>
  [<Extension>]
  static member TryParseNanoId :
    alphabet : Alphabet * value : string * [<Out; NotNullWhen(returnValue = true)>] nanoId : outref<NanoId> -> bool

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using the current alphabet to guide validation.
  /// </summary>
  /// <param name="alphabet">The letters to check against for validity.</param>
  /// <param name="value">The raw string to be converted.</param>
  /// <param name="nanoId">
  /// On successful parsing, contains a <c>NanoId</c> instance.
  /// </param>
  /// <returns>
  /// <c>true</c> if parsing succeeded; <c>false</c> otherwise.
  /// </returns>
  /// <remarks>
  /// If the input <c>value</c> is empty (ie: <c>null</c>, zero-length, or
  /// consists only of whitespace), parsing is considered to fail (ie: the
  /// method will return <c>false</c>).
  /// </remarks>
  [<Extension>]
  static member TryParseNonEmptyNanoId :
    alphabet : Alphabet * value : string * [<Out; NotNullWhen(returnValue = true)>] nanoId : outref<NanoId> -> bool


/// Pre-defined alphabets commonly used to generate identifiers.
module KnownAlphabets =
  /// Combination of all the lowercase, uppercase characters and numbers
  /// from 0 to 9, not including any symbols or special characters.
  val Alphanumeric : Alphabet

  /// Hexadecimal lowercase characters: 0123456789abcdef.
  val HexadecimalLowercase : Alphabet

  /// Hexadecimal uppercase characters: 0123456789ABCDEF.
  val HexadecimalUppercase : Alphabet

  /// Lowercase English letters: abcdefghijklmnopqrstuvwxyz.
  val Lowercase : Alphabet

  /// Numbers, Uppercase, and Lowercase without "lookalikes":
  /// 1, l, I, 0, O, o, u, v, 5, S, s, 2, Z.
  /// Complete set: 346789ABCDEFGHJKLMNPQRTUVWXYabcdefghijkmnpqrtwxyz.
  val NoLookalikes : Alphabet

  /// Same as Nolookalikes -- but having removed vowels and: 3, 4, x, X, V.
  /// Complete set: 6789BCDFGHJKLMNPQRTWbcdfghjkmnpqrtwz
  val NoLookalikesSafe : Alphabet

  /// Numbers from 0 to 9.
  val Numbers : Alphabet

  /// Uppercase English letters: ABCDEFGHIJKLMNOPQRSTUVWXYZ.
  val Uppercase : Alphabet

  /// <summary>
  /// URL-friendly numbers, English letters, and symbols: <c>A-Za-z0-9_-</c>.
  /// This is the default alphabet if one is not explicitly specified.
  /// </summary>
  val UrlSafe : Alphabet
