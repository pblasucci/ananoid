namespace pblasucci.Ananoid

open System
open System.Runtime.CompilerServices


[<AutoOpen>]
module private StringPatterns =
  let inline (|Empty|_|) value =
    if String.IsNullOrWhiteSpace value then Some() else None

  let inline (|Trimmed|) (value : string) =
    Trimmed(if String.IsNullOrWhiteSpace value then "" else value.Trim())

  let inline (|Length|) (Trimmed trimmed) = Length(uint32 trimmed.Length)


/// <summary>
/// Defines helpers to simplify using
/// <see cref='T:Microsoft.FSharp.Core.FSharpResult`2'/> from C#.
/// </summary>
[<Extension>]
type ResultExtensions =
  /// <summary>
  /// Invokes one of the given callbacks, based on the state of the given result.
  /// </summary>
  /// <param name="result">Determines which callback will be invoked.</param>
  /// <param name="ok">
  /// Invoked when the state of the given result is <c>Ok</c>, any
  /// underlying data in the result will be passed into the callback.
  /// </param>
  /// <param name="error">
  /// Invoked when the state of the given result is <c>Error</c>, any
  /// underlying data in the result will be passed into the callback.
  /// </param>
  /// <returns>The result of invoking the appropriate callback.</returns>
  /// <exception cref="T:System.ArgumentNullException">
  /// Raised if either of the given callbacks are <c>null</c>.
  /// </exception>
  [<Extension>]
  static member Match
    (
      result : Result<'T, 'TError>,
      ok : Func<'T, 'TReturn>,
      error : Func<'TError, 'TReturn>
    )
    : 'TReturn
    =
    if isNull ok then
      nullArg (nameof ok)

    if isNull error then
      nullArg (nameof error)

    match result with
    | Error ex -> error.Invoke(ex)
    | Ok value -> ok.Invoke(value)

  /// <summary>
  /// Extracts data from a given result when said result is <c>Ok</c>, or
  /// invokes the given factory callback when said result is <c>Error</c>.
  /// </summary>
  /// <param name="result">
  /// Determines, by its state if the given factory callback is invoked.
  /// </param>
  /// <param name="factory">
  /// A delegate which, when given an <c>Error</c> value,
  /// produces a new instance of the desired return type.
  /// </param>
  /// <returns>
  /// The underlying <c>Ok</c> value of the given result, or
  /// value returned from invoking the given factory callback.
  /// </returns>
  /// <exception cref="T:System.ArgumentNullException">
  /// Raised if the given factory callback is <c>null</c>.
  /// </exception>
  [<Extension>]
  static member GetValueOrDefault
    (
      result : Result<'T, 'TError>,
      factory : Func<'TError, 'T>
    )
    : 'T
    =
    if isNull factory then
      nullArg (nameof factory)

    match result with
    | Ok value -> value
    | Error error -> factory.Invoke(error)

  /// <summary>
  /// Extracts data from a given result when said result is <c>Error</c>,
  /// or invokes the given factory callback when said result is <c>Ok</c>.
  /// </summary>
  /// <param name="result">
  /// Determines, by its state if the given factory callback is invoked.
  /// </param>
  /// <param name="factory">
  /// A delegate which, when given an <c>Ok</c> value,
  /// produces a new instance of the desired return type.
  /// </param>
  /// <returns>
  /// The underlying <c>Error</c> value of the given result, or
  /// value returned from invoking the given factory callback.
  /// </returns>
  /// <exception cref="T:System.ArgumentNullException">
  /// Raised if the given factory callback is <c>null</c>.
  /// </exception>
  [<Extension>]
  static member GetErrorOrDefault
    (
      result : Result<'T, 'TError>,
      factory : Func<'T, 'TError>
    )
    : 'TError
    =
    if isNull factory then
      nullArg (nameof factory)

    match result with
    | Error error -> error
    | Ok value -> factory.Invoke(value)
