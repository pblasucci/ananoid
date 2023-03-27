module pblasucci.Ananoid.Tests.Alphabet

open System
open FsCheck
open FsCheck.Xunit

(* ⮟ system under test ⮟ *)
open pblasucci.Ananoid


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
        member _.WillPermit(value) = String.IsNullOrWhiteSpace(value)
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
        member _.WillPermit(value) = String.IsNullOrWhiteSpace(value)
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
        member _.WillPermit(value) = String.IsNullOrWhiteSpace(value)
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
  options.Alphabet.WillPermit(string generated)
  |> Prop.label $"Alphabet failed to validate given letters. (%A{generated})"

[<Property>]
let ``NanoIdFactory produces validatable output``
  (alphabet : Alphabet)
  (NonNegativeInt size)
  =
  let factory =
    alphabet.ToNanoIdFactory()
    |> Result.defaultWith (fun x -> failwith $"{x}")

  let expect = factory size
  let didParse, actual = NanoIdParser.UrlSafe.TryParse(string expect)

  (didParse && expect = actual) |> Prop.label $"%A{expect} <> %A{actual}"
