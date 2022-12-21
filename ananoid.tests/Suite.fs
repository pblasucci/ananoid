namespace MulberryLabs.Ananoid.Tests

open System
open FsCheck
open FsCheck.Xunit

(* ⮟ system under test ⮟ *)
open MulberryLabs.Ananoid
open MulberryLabs.Ananoid.Core
open MulberryLabs.Ananoid.Core.Tagged


module rec Functions =
  [<Property>]
  let ``Returns empty on negative size`` (NegativeInt size) =
    let value = nanoIdOf (string Numbers) size
    value |> String.IsNullOrWhiteSpace |> Prop.label $"generated '%s{value}'"

  [<Property>]
  let ``Raises exception on zero-length alphabet`` (NonNegativeInt count) =
    lazy (nanoIdOf (String.replicate count " ") 21)
    |> Prop.throws<ArgumentOutOfRangeException, _>

  [<Property(MaxTest = 1)>]
  let ``Raises exception on over-large alphabet`` () =
    lazy (nanoIdOf (String.replicate 1024 "-") 21)
    |> Prop.throws<ArgumentOutOfRangeException, _>

  [<Property(Arbitrary = [| typeof<Generation> |])>]
  let ``Tagged output equals untagged output`` (TaggedNanoId tagged) =
    let untagged = string tagged
    tagged = nanoid.tag untagged


module rec NanoId =
  [<Property(MaxTest = 1)>]
  let ``Input size zero produces empty NanoId`` () =
    let generated = NanoId.NewId(NanoIdOptions.UrlSafe.Resize(size = 0))
    generated |> NanoId.IsEmpty |> Prop.label (string generated)

  [<Property(MaxTest = 1)>]
  let ``Multiple empty NanoId instances are equal`` () =
    let resized = NanoId.NewId(NanoIdOptions.UrlSafe.Resize(size = 0))
    let one = NanoId() = NanoId.Empty |> Prop.label "\n.ctor is not `Empty`"
    let two = NanoId.Empty = resized |> Prop.label "\nzeroed is not `Empty`"
    one .&. two

  [<Property>]
  let ``Output size always equals input size`` (NonNegativeInt size) =
    let generated = NanoId.NewId(NanoIdOptions.UrlSafe.Resize size)
    let length = int generated.Length
    length = size |> Prop.label $"expect (%i{size}) <> actual (%i{length})"

  [<Property(Arbitrary = [| typeof<Generation> |])>]
  let ``Multiple instances are equal when their underlying values are equal``
    (RawNanoId input)
    =
    let one = NanoId.Parse(input)
    let two = NanoId.Parse(input)
    one = two |> Prop.label $"\nNot equal on '{input}'"

  [<Property(Arbitrary = [| typeof<Generation> |])>]
  let ``Multiple instances are ordered the same as their underlying values``
    (RawNanoId input1)
    (RawNanoId input2)
    (RawNanoId input3)
    =
    let value1 = NanoId.Parse(input1)
    let value2 = NanoId.Parse(input2)
    let value3 = NanoId.Parse(input3)

    let inputs = List.sort [ input1; input2; input3 ]
    let values =
      List.sort
        [
          yield! Result.toList value1
          yield! Result.toList value2
          yield! Result.toList value3
        ]

    values
    |> List.zip inputs
    |> List.forall (fun (raw, nano) -> raw = string nano)
    |> Prop.label $"\ninputs: {inputs}\nvalues: {values}"

  [<Property(Arbitrary = [| typeof<Generation> |])>]
  let ``Parse and ToString are invertible`` nanoId =
    match NanoId.Parse(string nanoId) with
    | Ok nanoId' -> nanoId' = nanoId
    | Error _err -> false
    |> Prop.label $"\nNot invertible: %A{nanoId}"

  [<Property>]
  let ``Parse returns error on invalid letters`` (PositiveInt count) =
    let input = String.replicate count "*"
    match NanoId.Parse(input, Alphanumeric) with
    | Error IncompatibleAlphabet -> Prop.ofTestable true
    | Error failure -> false |> Prop.label $"\nGot Error: '{failure.Message}'"
    | Ok nanoId -> false |> Prop.label $"\nParse Ok: '{nanoId}'"


module rec Alphabet =

  open FsCheck
  open MulberryLabs.Ananoid

  [<Property(MaxTest = 1)>]
  let ``Custom alphabet fails if alphabet is null`` () =
    match Alphabet.Validate(Unchecked.defaultof<_>) with
    // ⮟ pass ⮟
    | Error AlphabetTooSmall -> Prop.ofTestable true
    // ⮟ fail ⮟
    | Error unexpectedly -> false |> Prop.label $"%A{unexpectedly}"
    | Ok tooShortOptions -> false |> Prop.label $"%A{tooShortOptions}"

  [<Property(MaxTest = 1)>]
  let ``Custom alphabet fails if incoherent`` () =
    let incoherent =
      { new IAlphabet with
          member _.Letters = "abc123DoReMe"
          member _.IncludesAll(value) = String.IsNullOrWhiteSpace(value)
      }

    match Alphabet.Validate(incoherent) with
    // ⮟ pass ⮟
    | Error IncoherentAlphabet -> Prop.ofTestable true
    // ⮟ fail ⮟
    | Error unexpectedly -> false |> Prop.label $"%A{unexpectedly}"
    | Ok tooShortOptions -> false |> Prop.label $"%A{tooShortOptions}"

  [<Property(MaxTest = 1)>]
  let ``Custom alphabet fails if too short`` () =
    let tooShort =
      { new IAlphabet with
          member _.Letters = ""
          member _.IncludesAll(value) = String.IsNullOrWhiteSpace(value)
      }

    match Alphabet.Validate(tooShort) with
    // ⮟ pass ⮟
    | Error AlphabetTooSmall -> Prop.ofTestable true
    // ⮟ fail ⮟
    | Error unexpectedly -> false |> Prop.label $"%A{unexpectedly}"
    | Ok tooShortOptions -> false |> Prop.label $"%A{tooShortOptions}"

  [<Property(MaxTest = 1)>]
  let ``Custom alphabet fails if too large`` () =
    let tooLarge =
      { new IAlphabet with
          member _.Letters = "qwerty123" |> String.replicate 512
          member _.IncludesAll(value) = String.IsNullOrWhiteSpace(value)
      }

    match Alphabet.Validate(tooLarge) with
    // ⮟ pass ⮟
    | Error AlphabetTooLarge -> Prop.ofTestable true
    // ⮟ fail ⮟
    | Error unexpectedly -> false |> Prop.label $"%A{unexpectedly}"
    | Ok tooShortOptions -> false |> Prop.label $"%A{tooShortOptions}"

  [<Property(Arbitrary = [| typeof<Generation> |])>]
  let ``All pre-defined alphabets produce comprehensible outputs`` options =
    let generated = NanoId.NewId options
    options.Alphabet.IncludesAll(string generated)
    |> Prop.label $"%s{IncompatibleAlphabet.Message} (%A{generated})"

  [<Property>]
  let ``NanoIdFactory produces validatable output``
    (alphabet : Alphabet)
    (NonNegativeInt size)
    =
    let expected =
      alphabet
      |> Alphabet.ToNanoIdFactory
      |> Result.map (fun makeNanoId -> makeNanoId size)
    let raw = expected |> Result.map string |> Result.defaultValue ""
    let actual = NanoId.Parse(raw, alphabet)
    actual = expected |> Prop.label $"%A{expected} <> %A{actual}"
