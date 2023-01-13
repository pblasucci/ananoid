namespace MulberryLabs.Ananoid

open System
open System.Runtime.CompilerServices


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
  abstract IncludesAll : value : string -> bool


/// <summary>
/// Details the potential failures which can occur when an
/// <see cref="T:MulberryLabs.Ananoid.IAlphabet"/> is validated,
/// or when an alphabet evaluates a string for compatability.
/// </summary>
type AlphabetError =
  /// Raised when an alphabet contains more than 255 letters.
  | AlphabetTooLarge

  /// Raised when an alphabet contains no letters.
  | AlphabetTooSmall

  /// Raised when an alphabet cannot validate its own letters.
  | IncoherentAlphabet

  /// <summary>
  /// Raised when a string contains letters from outside the alphabet in
  /// question (typically, as part of parsing existing data into a
  /// <see cref="T:MulberryLabs.Ananoid.NanoId"/>).
  /// </summary>
  | IncompatibleAlphabet

  /// A human-readable description of the error, suitable for printing.
  member Message : string


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
  /// Checks that a given <see cref="T:MulberryLabs.Ananoid.IAlphabet"/>
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
  /// otherwise, returns a <see cref="T:MulberryLabs.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  static member Validate :
    alphabet : IAlphabet -> Result<IAlphabet, AlphabetError>


/// Details the values which can be changed to alter the generated identifiers.
[<NoComparison>]
[<Sealed>]
type NanoIdOptions =
  /// The alphabet from which identifiers will be generated, and against
  /// which raw strings may be checked for validity (e.g. for parsing).
  member Alphabet : IAlphabet

  /// The length of a generated identifier, in number of characters.
  member Size : int

  /// <summary>
  /// Changes the output size (i.e. the character-length of
  /// generated identifiers), while preserving the <c>Alphabet</c>.
  /// </summary>
  /// <param name="size">
  /// The new output size (note: negative values are changed to zero).
  /// </param>
  /// <returns>A new instance with the given <c>Size</c>.</returns>
  member Resize : size : int -> NanoIdOptions

  /// <summary>
  /// Creates a new instance from the given inputs, after checking the
  /// validity of <c>alphabet</c> and, if necessary, adjusting <c>size</c>.
  /// </summary>
  /// <param name="alphabet">
  /// The IAlphabet to use for generation and validation.
  /// </param>
  /// <param name="size">
  /// The length of a generated identifier, in number of characters
  /// (note: negative values are changed to zero).
  /// </param>
  /// <returns>
  /// On successful validation, returns a new <c>NanoIdOptions</c> instance;
  /// otherwise, returns a <see cref="T:MulberryLabs.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  static member Of :
    alphabet : IAlphabet * size : int -> Result<NanoIdOptions, AlphabetError>

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:MulberryLabs.Ananoid.Alphabet.Alphanumeric"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member Alphanumeric : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:MulberryLabs.Ananoid.Alphabet.HexadecimalLowercase"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member HexadecimalLowercase : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:MulberryLabs.Ananoid.Alphabet.HexadecimalUppercase"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member HexadecimalUppercase : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:MulberryLabs.Ananoid.Alphabet.Lowercase"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member Lowercase : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:MulberryLabs.Ananoid.Alphabet.Numbers"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member Numbers : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:MulberryLabs.Ananoid.Alphabet.NoLookalikes"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member NoLookalikes : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:MulberryLabs.Ananoid.Alphabet.NoLookalikesSafe"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member NoLookalikesSafe : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:MulberryLabs.Ananoid.Alphabet.Uppercase"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member Uppercase : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:MulberryLabs.Ananoid.Alphabet.UrlSafe"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  /// <remarks>These are defaults, used when no others are specified.</remarks>
  static member UrlSafe : NanoIdOptions


/// <summary>
/// Contains primitives for generating identifiers (as strings), which
/// serve as the basis for <see cref="T:MulberryLabs.Ananoid.NanoId"/>,
/// and which might be useful in some niche situations.
/// </summary>
module Core =
  /// <summary>
  /// Generates a new identifier, <c>size</c> characters in length,
  /// derived from the letters of the given alphabet
  /// (note: a size of less than zero will result in an empty string).
  /// </summary>
  /// <exception cref="T:System.ArgumentOutOfRangeException">
  /// Thrown when <c>alphabet</c> contains &lt; 1 letter, or &gt; 255 letters.
  /// </exception>
  [<CompiledName("NewNanoId")>]
  val nanoIdOf : alphabet : string -> size : int -> string

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
    val nanoIdOf' : alphabet : string -> size : int -> string<nanoid>

    /// <summary>
    /// Generates a new tagged identifier with the default alphabet and size.
    /// </summary>
    /// /// <remarks>
    /// <b>This function is not usable from languages other than F#.</b>
    /// </remarks>
    val nanoId' : unit -> string<nanoid>


/// Represents a unique textual identifier, with a known length,
/// based on a particular alphabet (i.e. a set of letters).
[<IsReadOnly>]
[<Struct>]
type NanoId =
  /// The number of characters in this instance.
  member Length : uint32

  /// The zero-valued instance of this type.
  static member Empty : NanoId

  /// Returns true, when the given nanoId is zero-valued; otherwise, false.
  static member IsEmpty : nanoId : NanoId -> bool

  /// <summary>
  /// Creates a new instance from the given
  /// <see cref="T:MulberryLabs.Ananoid.NanoIdOptions"/>.
  /// </summary>
  /// <param name="options">
  /// The options to use in generating the identifier.
  /// </param>
  static member NewId : options : NanoIdOptions -> NanoId

  /// Creates a new instance using the default options.
  static member NewId : unit -> NanoId

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:MulberryLabs.Ananoid.NanoId" />,
  /// using the given <c>alphabet</c> to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <param name="alphabet">
  /// The set of letters from which <c>value</c> must be constituted.
  /// </param>
  /// <returns>
  /// On successful parsing, returns an <c>NanoId</c> instance;
  /// otherwise, returns a <see cref="T:MulberryLabs.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  static member Parse :
    value : string * alphabet : IAlphabet -> Result<NanoId, AlphabetError>

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:MulberryLabs.Ananoid.NanoId" />,
  /// using the default alphabet to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <returns>
  /// On successful parsing, returns an <c>NanoId</c> instance;
  /// otherwise, returns a <see cref="T:MulberryLabs.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  static member Parse : value : string -> Result<NanoId, AlphabetError>

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:MulberryLabs.Ananoid.NanoId" />,
  /// using the given <c>alphabet</c> to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <param name="alphabet">
  /// The set of letters from which <c>value</c> must be constituted.
  /// </param>
  /// <param name="nanoId">
  /// On successful parsing, contains a non-empty <c>NanoId</c> instance.
  /// </param>
  /// <returns>true is parsing succeeded; false, otherwise.</returns>
  static member TryParse :
    value : string * alphabet : IAlphabet * nanoId : outref<NanoId> -> bool

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:MulberryLabs.Ananoid.NanoId" />,
  /// using the default alphabet to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <param name="nanoId">
  /// On successful parsing, contains a non-empty <c>NanoId</c> instance.
  /// </param>
  /// <returns>true is parsing succeeded; false, otherwise.</returns>
  static member TryParse : value : string * nanoId : outref<NanoId> -> bool


[<Extension>]
[<Sealed>]
type IAlphabetExtensions =
  /// <summary>
  /// Produces a function for generating NanoId instances of varying sizes
  /// (note: requires a valid <see cref="T:MulberryLabs.Ananoid.IAlphabet"/>).
  /// </summary>
  /// <param name="alphabet">An IAlphabet from which to generate NanoIds.</param>
  /// <returns>
  /// On successful validation, returns a "factory function" which will produce
  /// a <see cref="T:MulberryLabs.Ananoid.NanoId"/> of the given size,
  /// constituted from the input given alphabet;
  /// otherwise, returns a <see cref="T:MulberryLabs.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  [<CompiledName("ToNanoIdFactory@FSharpFunc")>]
  [<Extension>]
  static member ToNanoIdFactory :
    alphabet : IAlphabet -> Result<int -> NanoId,AlphabetError>

  /// <summary>
  /// Produces a function for generating NanoId instances of varying sizes
  /// (note: requires a valid <see cref="T:MulberryLabs.Ananoid.IAlphabet"/>).
  /// </summary>
  /// <param name="alphabet">An IAlphabet from which to generate NanoIds.</param>
  /// <returns>
  /// On successful validation, returns a "factory function" which will produce
  /// a <see cref="T:MulberryLabs.Ananoid.NanoId"/> of the given size,
  /// constituted from the input given alphabet;
  /// otherwise, returns a <see cref="T:MulberryLabs.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  [<CompilerMessage("Not intended for use from F#", 9999, IsHidden = true)>]
  [<CompiledName("ToNanoIdFactory")>]
  [<Extension>]
  static member ToNanoIdFactoryDelegate :
    alphabet : IAlphabet -> Result<Func<int, NanoId>,AlphabetError>
