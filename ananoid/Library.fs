namespace pblasucci.Ananoid

open System

[<AutoOpen>]
module private StringPatterns =
  let inline (|Empty|_|) value =
    if String.IsNullOrWhiteSpace value then Some() else None

  let inline (|Trimmed|) (value : string) =
    Trimmed(if String.IsNullOrWhiteSpace value then "" else value.Trim())

  let inline (|Length|) (Trimmed trimmed) = Length(uint32 trimmed.Length)


[<RequireQualifiedAccess>]
module private Result =
  let inline either ok error result =
    match result with
    | Error ex -> error ex
    | Ok value -> ok value
