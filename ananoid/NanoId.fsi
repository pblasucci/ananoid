(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open System.Runtime.CompilerServices


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


/// <summary>
/// Details the potential failures which can occur when an a letter set is
/// validated during <see cref="T:pblasucci.Ananoid.Alphabet"/> creation.
/// </summary>
type AlphabetError =
  /// Raised when an alphabet contains more than 255 letters.
  | AlphabetTooLarge of Source : string

  /// Raised when an alphabet contains no letters.
  | AlphabetTooSmall of Source : string

  /// The letter set which generated the current error.
  member Letters : string

  /// A human-readable description of the error, suitable for printing.
  member Message : string

  /// <summary>
  /// Creates an <see cref="T:pblasucci.Ananoid.AlphabetException"/>
  /// from the current <c>AlphabetError</c> instance.
  /// The newly created exception is then raised.
  /// </summary>
  /// <exception cref="T:pblasucci.Ananoid.AlphabetException">
  /// Raised as the intended consequence of invoking this method.
  /// </exception>
  member Promote : unit -> 'T


/// Encapsulates data for the point-in-time failure of
/// an operation involving alphabet validation.
[<Sealed; Class>]
type AlphabetException =
  inherit System.Exception

  /// The alphabet which lead to the exception.
  member Alphabet : string

  /// Further details about the actual failure.
  member Reason : AlphabetError


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
  member ParseNanoId : value : string -> NanoId option

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
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  static member Validate : letters : string -> Result<Alphabet, AlphabetError>


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
  val parseAs : alphabet : Alphabet -> value : string -> NanoId option


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
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  val ofLetters : letters : string -> Result<Alphabet, AlphabetError>

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
  /// <exception cref="T:pblasucci.Ananoid.AlphabetException">
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
  val parseNanoId : value : string -> alphabet : Alphabet -> NanoId option


/// <summary>
/// Contains utilities intended to simplify working with
/// <see cref="T:pblasucci.Ananoid.Alphabet" /> in languages other than F#.
/// </summary>
[<Extension>]
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
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  [<Extension>]
  static member ToAlphabet : letters : string -> Result<Alphabet, AlphabetError>

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
  /// <exception cref="T:pblasucci.Ananoid.AlphabetException">
  /// Raised when the given alphabet fails to uphold an invariant.
  /// </exception>
  [<Extension>]
  static member ToAlphabetOrThrow : letters : string -> Alphabet

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
  /// <returns>true is parsing succeeded; false, otherwise.</returns>
  static member TryParseNanoId :
    alphabet : Alphabet * value : string * nanoId : outref<NanoId> -> bool


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
