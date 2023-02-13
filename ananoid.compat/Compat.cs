using Microsoft.FSharp.Core;

namespace pblasucci.Ananoid.Compat;

/// Some helpers for working with FSharpResult`2
internal static class FSharpResultExtensions
{
  public static T? GetValueOrDefault<T, TError>
    (this FSharpResult<T, TError> result) =>
    result switch
    {
      { IsError: true } => default,
      { ResultValue: var value } => value
    };

  public static T GetValueOrThrow<T, TError>
    (this FSharpResult<T, TError> result, Func<TError, Exception> error) =>
    result switch
    {
      { IsError: true, ErrorValue: var x } => throw error(x),
      { ResultValue: var value } => value
    };

  public static void Deconstruct<T, TError>
  (
    this FSharpResult<T, TError> result,
    out bool isOk,
    out T? value,
    out TError? error
  )
  {
    isOk = result.IsOk;
    value = result.IsOk ? result.ResultValue : default;
    error = result.IsOk is false ? result.ErrorValue : default;
  }
}
