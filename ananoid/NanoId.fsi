(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open System
open System.Runtime.CompilerServices


/// <summary>
/// Details the values which can be changed to alter the generation of
/// <see cref="T:pblasucci.Ananoid.NanoId" /> identifiers.
/// </summary>
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
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  static member Create :
    alphabet : IAlphabet * size : int -> Result<NanoIdOptions, AlphabetError>

  /// <summary>
  /// Creates a new instance from the given inputs, after checking the
  /// validity of <c>alphabet</c> and, if necessary, adjusting <c>size</c>.
  /// </summary>
  /// <remarks>
  /// F# callers should use the name <c>CreateOrRaise</c>; however, callers
  /// in other languages (eg: C#, Visual Basic) should use the name
  /// <c>CreateOrThrow</c> to refer to this method.
  /// </remarks>
  /// <param name="alphabet">
  /// The IAlphabet to use for generation and validation.
  /// </param>
  /// <param name="size">
  /// The length of a generated identifier, in number of characters
  /// (note: negative values are changed to zero).
  /// </param>
  /// <exception cref="T:pblasucci.Ananoid.AlphabetException">
  /// Raised if the given alphabet cannot be validated.
  /// </exception>
  [<CompiledName("CreateOrThrow")>]
  static member CreateOrRaise :
    alphabet : IAlphabet * size : int -> NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:pblasucci.Ananoid.Alphabet.Alphanumeric"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member Alphanumeric : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:pblasucci.Ananoid.Alphabet.HexadecimalLowercase"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member HexadecimalLowercase : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:pblasucci.Ananoid.Alphabet.HexadecimalUppercase"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member HexadecimalUppercase : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:pblasucci.Ananoid.Alphabet.Lowercase"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member Lowercase : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:pblasucci.Ananoid.Alphabet.NoLookalikes"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member NoLookalikes : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:pblasucci.Ananoid.Alphabet.NoLookalikesSafe"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member NoLookalikesSafe : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:pblasucci.Ananoid.Alphabet.Numbers"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member Numbers : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:pblasucci.Ananoid.Alphabet.Uppercase"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  static member Uppercase : NanoIdOptions

  /// <summary>
  /// A <c>NanoIdOptions</c> instance with a
  /// <see cref="M:pblasucci.Ananoid.Alphabet.UrlSafe"/>
  /// alphabet and a default output size (21 characters).
  /// </summary>
  /// <remarks>These are defaults, used when no others are specified.</remarks>
  static member UrlSafe : NanoIdOptions


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

  /// <exclude />
  static member op_Implicit : nanoId : NanoId -> string

  /// <summary>
  /// Creates a new instance from the given
  /// <see cref="T:pblasucci.Ananoid.NanoIdOptions"/>.
  /// </summary>
  /// <param name="options">
  /// The options to use in generating the identifier.
  /// </param>
  static member NewId : options : NanoIdOptions -> NanoId

  /// Creates a new instance using the default options.
  static member NewId : unit -> NanoId


/// <summary>
/// Provides methods for validating and parsing strings into NanoId instances of
/// <see cref="T:pblasucci.Ananoid.NanoId" />, while using a specific
/// <see cref="T:pblasucci.Ananoid.IAlphabet" /> to validate the inputs.
/// </summary>
[<Sealed>]
type NanoIdParser =
  /// The set of letters against which raw strings will be checked for validity.
  member Alphabet : IAlphabet

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using a known alphabet to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <returns>
  /// On successful parsing, returns a <c>NanoId</c> instance;
  /// otherwise, returns <c>None</c>.
  /// </returns>
  member Parse : value : string -> Option<NanoId>

  /// <summary>
  /// Attempts to convert the given <c>value</c> into a
  /// <see cref="T:pblasucci.Ananoid.NanoId" />,
  /// using a known alphabet to guide validation.
  /// </summary>
  /// <param name="value">The raw string to be converted.</param>
  /// <param name="nanoId">
  /// On successful parsing, contains a non-empty <c>NanoId</c> instance.
  /// </param>
  /// <returns>true is parsing succeeded; false, otherwise.</returns>
  member TryParse : value : string * nanoId : outref<NanoId> -> bool

  /// <summary>
  /// A validating parser based on the
  /// <see cref="M:pblasucci.Ananoid.Alphabet.Alphanumeric" /> alphabet.
  /// </summary>
  static member Alphanumeric : NanoIdParser

  /// <summary>
  /// A validating parser based on the
  /// <see cref="M:pblasucci.Ananoid.Alphabet.HexadecimalLowercase" />
  /// alphabet.
  /// </summary>
  static member HexadecimalLowercase : NanoIdParser

  /// <summary>
  /// A validating parser based on the
  /// <see cref="M:pblasucci.Ananoid.Alphabet.HexadecimalUppercase" />
  /// alphabet.
  /// </summary>
  static member HexadecimalUppercase : NanoIdParser

  /// <summary>
  /// A validating parser based on the
  /// <see cref="M:pblasucci.Ananoid.Alphabet.Lowercase" /> alphabet.
  /// </summary>
  static member Lowercase : NanoIdParser

  /// <summary>
  /// A validating parser based on the
  /// <see cref="M:pblasucci.Ananoid.Alphabet.NoLookalikes" /> alphabet.
  /// </summary>
  static member NoLookalikes : NanoIdParser

  /// <summary>
  /// A validating parser based on the
  /// <see cref="M:pblasucci.Ananoid.Alphabet.NoLookalikesSafe" /> alphabet.
  /// </summary>
  static member NoLookalikesSafe : NanoIdParser

  /// <summary>
  /// A validating parser based on the
  /// <see cref="M:pblasucci.Ananoid.Alphabet.Numbers" /> alphabet.
  /// </summary>
  static member Numbers : NanoIdParser

  /// <summary>
  /// A validating parser based on the
  /// <see cref="M:pblasucci.Ananoid.Alphabet.Uppercase" /> alphabet.
  /// </summary>
  static member Uppercase : NanoIdParser

  /// <summary>
  /// A validating parser based on the
  /// <see cref="M:pblasucci.Ananoid.Alphabet.UrlSafe" /> alphabet.
  /// </summary>
  static member UrlSafe : NanoIdParser

  /// <summary>
  /// Tries to create a new instance.
  /// </summary>
  /// <param name="alphabet">
  /// The set of letters against which raw strings will be checked for validity.
  /// </param>
  /// <returns>
  /// On successful creation, returns an <c>NanoIdParser</c> instance;
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  static member Create :
    alphabet : IAlphabet -> Result<NanoIdParser, AlphabetError>

  /// <summary>
  /// Tries to create a new instance.
  /// </summary>
  /// <remarks>
  /// F# callers should use the name <c>CreateOrRaise</c>; however, callers
  /// in other languages (eg: C#, Visual Basic) should use the name
  /// <c>CreateOrThrow</c> to refer to this method.
  /// </remarks>
  /// <param name="alphabet">
  /// The set of letters against which raw strings will be checked for validity.
  /// </param>
  /// <returns>
  /// On successful creation, returns an <c>NanoIdParser</c> instance.
  /// </returns>
  /// <exception cref="T:pblasucci.Ananoid.AlphabetException">
  /// Raised if the given alphabet cannot be validated.
  /// </exception>
  [<CompiledName("CreateOrThrow")>]
  static member CreateOrRaise : alphabet : IAlphabet -> NanoIdParser


/// Provided utilities for working with
/// <see cref="T:pblasucci.Ananoid.NanoIdOption"/> instances.
[<AutoOpen>]
module NanoIdOptionsPatterns =
  /// <summary>
  /// Extracts the <see cref="T:pblasucci.Ananoid.IAlphabet"/> instance
  /// from a <see cref="T:pblasucci.Ananoid.NanoIdOptions"/> instance,
  /// or from a <see cref="T:pblasucci.Ananoid.NanoIdParser"/> instance.
  /// </summary>
  /// <remarks>
  /// <b>This active pattern is not intended for languages other than F#.</b>
  /// </remarks>
  val inline (|SourceAlphabet|) :
    source : 'Source -> IAlphabet when 'Source : (member Alphabet : IAlphabet)

  /// <summary>
  /// Extracts the <c>TargetSize</c> value from a
  /// <see cref="T:pblasucci.Ananoid.NanoIdOptions"/> instance.
  /// </summary>
  /// <remarks>
  /// <b>This active pattern is not intended for languages other than F#.</b>
  /// </remarks>
  val (|TargetSize|) : source : NanoIdOptions -> int


/// Provided utilities for working with
/// <see cref="T:pblasucci.Ananoid.NanoIdOption"/> instances.
[<Extension>]
[<Sealed>]
type NanoIdOptionsExtensions =
  /// <summary>
  /// Extracts the <see cref="T:pblasucci.Ananoid.IAlphabet"/> instance
  /// and the <c>TargetSize</c> value from a
  /// <see cref="T:pblasucci.Ananoid.NanoIdOptions"/> instance.
  /// </summary>
  /// <remarks>
  /// This method is primarily intended for interoperability with C#.
  /// </remarks>
  [<Extension>]
  static member Deconstruct :
    options : NanoIdOptions *
    alphabet : outref<IAlphabet> *
    targetSize : outref<int> ->
      unit


/// <summary>
/// Provides tools for generating <see cref="T:pblasucci.Ananoid.NanoId"/>
/// instances directly from <see cref="T:pblasucci.Ananoid.IAlphabet"/>
/// instances (i.e. by-passing the explicit use of
/// <see cref="T:pblasucci.Ananoid.NanoIdOptions "/>).
/// </summary>
[<Extension>]
[<Sealed>]
type IAlphabetExtensions =
  /// <summary>
  /// Produces a function for generating NanoId instances of varying sizes
  /// (note: requires a valid <see cref="T:pblasucci.Ananoid.IAlphabet"/>).
  /// </summary>
  /// <remarks>
  /// <b>This method is not intended for languages other than F#.</b> Further,
  /// it should be referenced via its compiled name,
  /// <c>ToNanoIdFactory@FSharpFunc</c> (eg: during reflection).
  /// </remarks>
  /// <param name="alphabet">An IAlphabet from which to generate NanoIds.</param>
  /// <returns>
  /// On successful validation, returns a "factory function" which will produce
  /// a <see cref="T:pblasucci.Ananoid.NanoId"/> of the given size,
  /// constituted from the input given alphabet;
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  [<CompiledName("ToNanoIdFactory@FSharpFunc")>]
  [<Extension>]
  static member ToNanoIdFactory :
    alphabet : IAlphabet -> Result<int -> NanoId, AlphabetError>

  /// <summary>
  /// Produces a function for generating NanoId instances of varying sizes
  /// (note: requires a valid <see cref="T:pblasucci.Ananoid.IAlphabet"/>).
  /// </summary>
  /// <remarks>
  /// <b>This method is NOT intended for use from F#.</b> Further, it should
  /// be referenced via its compiled name, <c>ToNanoIdFactory</c> (in C# or
  /// Visual Basic, during reflection, et cetera).
  /// </remarks>
  /// <param name="alphabet">An IAlphabet from which to generate NanoIds.</param>
  /// <returns>
  /// On successful validation, returns a "factory function" which will produce
  /// a <see cref="T:pblasucci.Ananoid.NanoId"/> of the given size,
  /// constituted from the input given alphabet;
  /// otherwise, returns a <see cref="T:pblasucci.Ananoid.AlphabetError"/>
  /// with further details about what went wrong.
  /// </returns>
  /// <exception cref="T:pblasucci.Ananoid.AlphabetException">
  /// Raised when given <c>alphabet</c> fails validation.
  /// </exception>
  [<CompilerMessage("Not intended for use from F#", 9999, IsHidden = true)>]
  [<CompiledName("ToNanoIdFactory")>]
  [<Extension>]
  static member ToNanoIdFactoryDelegate :
    alphabet : IAlphabet -> Func<int, NanoId>
