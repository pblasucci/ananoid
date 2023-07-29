(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid


/// Represents a set of 'letters' from which an identifier is made.
type IAlphabet =
  /// The set of 'letters' constituting this alphabet.
  abstract Letters : string

  /// <summary>
  /// Tests if the given value could be constituted from this alphabet.
  /// </summary>
  /// <remarks>
  /// All predefined alphabets are considered to include inputs which are
  /// <c>null</c> or consist entirely of whitespace characters. It is
  /// recommended, but not required, that any custom alphabets do the same.
  /// </remarks>
  /// <param name="value">a string of letters to be evaluated.</param>
  /// <returns>
  /// true when the input is composed solely of this alphabet; false otherwise.
  /// </returns>
  abstract WillPermit : value : string -> bool


/// <summary>
/// Details the potential failures which can occur when an
/// <see cref="T:pblasucci.Ananoid.IAlphabet"/> is validated,
/// or when an alphabet evaluates a string for compatability.
/// </summary>
[<NoComparison>]
type AlphabetError =
  /// Raised when an alphabet contains more than 255 letters.
  | AlphabetTooLarge of Source : IAlphabet

  /// Raised when an alphabet contains no letters.
  | AlphabetTooSmall of Source : IAlphabet

  /// Raised when an alphabet cannot validate its own letters.
  | IncoherentAlphabet of Source : IAlphabet

  /// <summary>
  /// The <see cref="T:pblasucci.Ananoid.IAlphabet" />
  /// which generated the current error.
  /// </summary>
  member Alphabet : IAlphabet

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


/// Encapsulates data for the point-in-time failure of an operation involving
/// an <see cref="pblasucci.Ananoid.IAlphabet"/> instance
[<Sealed; Class>]
type AlphabetException =
  inherit System.Exception

  /// The alphabet which lead to the exception.
  member Alphabet : IAlphabet

  /// Further details about the actual failure.
  member Reason : AlphabetError


/// Pre-defined alphabets commonly used to generation identities.
type Alphabet =
  /// Combination of all the lowercase, uppercase characters and numbers
  /// from 0 to 9, not including any symbols or special characters.
  | Alphanumeric

  /// English hexadecimal lowercase characters: 0123456789abcdef.
  | HexadecimalLowercase

  /// English hexadecimal uppercase characters: 0123456789ABCDEF
  | HexadecimalUppercase

  /// Lowercase English letters: abcdefghijklmnopqrstuvwxyz.
  | Lowercase

  /// Numbers, Uppercase, and Lowercase without "lookalikes":
  /// 1, l, I, 0, O, o, u, v, 5, S, s, 2, Z.
  /// Complete set: 346789ABCDEFGHJKLMNPQRTUVWXYabcdefghijkmnpqrtwxyz.
  | NoLookalikes

  /// Same as Nolookalikes -- but having removed vowels and: 3, 4, x, X, V.
  /// Complete set: 6789BCDFGHJKLMNPQRTWbcdfghjkmnpqrtwz
  | NoLookalikesSafe

  /// Numbers from 0 to 9.
  | Numbers

  /// Uppercase English letters: ABCDEFGHIJKLMNOPQRSTUVWXYZ.
  | Uppercase

  /// <summary>
  /// URL-friendly numbers, English letters, and symbols: <c>A-Za-z0-9_-</c>.
  /// This is the default alphabet if one is not explicitly specified.
  /// </summary>
  | UrlSafe

  interface IAlphabet

  /// <summary>
  /// Checks that a given <see cref="T:pblasucci.Ananoid.IAlphabet"/>
  /// upholds certain invariants necessary for the algorithm to work well.
  /// </summary>
  /// <remarks>
  /// An IAlphabet instance MUST uphold the following invariants:
  /// <list type="bullet">
  /// <item>Is not <c>null</c></item>
  /// <item>Contains at least one (1) letter</item>
  /// <item>Contains no more then 255 letters.</item>
  /// <item>Is able to successfully validate its own set of letters.</item>
  /// </list>
  /// </remarks>
  /// <param name="alphabet">An IAlphabet instance to be validated.</param>
  /// <returns>
  /// On successful validation, returns the given input (unmodified);
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  static member Validate :
    alphabet : IAlphabet -> Result<IAlphabet, AlphabetError>

  /// <summary>
  /// Checks that a given <see cref="T:pblasucci.Ananoid.IAlphabet"/>
  /// upholds certain invariants necessary for the algorithm to work well.
  /// </summary>
  /// <remarks>
  /// An IAlphabet instance MUST uphold the following invariants:
  /// <list type="bullet">
  /// <item>Is not <c>null</c></item>
  /// <item>Contains at least one (1) letter</item>
  /// <item>Contains no more then 255 letters.</item>
  /// <item>Is able to successfully validate its own set of letters.</item>
  /// </list>
  /// F# callers should use the name <c>ValidateOrRaise</c>; however, callers
  /// in other languages (eg: C#, Visual Basic) should use the name
  /// <c>ValidateOrThrow</c> to refer to this method.
  /// </remarks>
  /// <param name="alphabet">An IAlphabet instance to be validated.</param>
  /// <exception cref="T:pblasucci.Ananoid.AlphabetException">
  /// Raised when the given alphabet fails to uphold an invariant.
  /// </exception>
  [<CompiledName("ValidateOrThrow")>]
  static member ValidateOrRaise : alphabet : IAlphabet -> unit
