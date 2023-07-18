---
title: Define a Custom Alphabet
category: Alphabets
categoryindex: 2
index: 1
---

How-To: Define a Custom Alphabet
===

The default settings for creating a `NanoId` (21 character taken from a mix of
letters, numbers, hyphen, and underscore) reflect a reasonable balance of
entropy and performance. Further the additional alphabets shipped with Ananoid
cover a wide range of common wants and needs. However, it is possible to go
further. Consumers can provide their _own_ alphabets.

### Learning about alphabets

The only requirement for any alphabet is that it implement the `IAlphabet`
interface. This simple contract provides everything needed to generate new
 `NanoId` instances. Its contract specifies two members:

<div class="lang-strip">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
type IAlphabet =
  abstract Letters : string with get

  abstract WillPermit : string -> bool
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Interface IAlphabet
  ReadOnly Property Letters As String

  Function WillPermit(String) As Boolean
End Interface
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
interface IAlphabet
{
  string Letters { get; }

  bool WillPermit(string);
}
```
</details>
</div>

The `Letters` property defines the universe of characters, which will be
pulled from at random, as part of creating a `NanoId`. The `WillPermit` method,
meanwhile, tests if a given value could be constituted from the alphabet. For
example, an alphabet consisting only of numbers (0 through 9 inclusive) might
be (näively) implemented like so:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
[<Struct>]
type Numbers() =
  interface IAlphabet with
    member _.Letters = "0123456789"

    member _.WillPermit(value) = value |> String.forall Char.IsAsciiDigit
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
NotInheritable Class Numbers : Implements IAlphabet
  Public Function WillPermit(value As String) As Boolean Implements IAlphabet.WillPermit
    Return value.All(Char.IsAsciiDigit)
  End Function

  ReadOnly Property Letters As String = "0123456789" Implements IAlphabet.Letters
End Class
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
struct Numbers : IAlphabet
{
  bool IAlphabet.WillPermit(string value)
      => value.All(Char.IsAsciiDigit)

  string IAlphabet.Letters => "0123456789";
}
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

However, an alphabet is more than just a string. There are certain _invariants_
which any alphabet must uphold. They are:

+ The set of letters MUST contain at least one (1) letter.
+ The set of letters MAY NOT contains more than 255 letters.
+ The alphabet MUST be 'coherent' (it can validate its own letter set).

These are not the most challenging invariants. Nevertheless, the Ananoid
library checks them in certain key places. If an alphabet fails to pass
validation, an `cref:T:pblasucci.Ananoid.AlphabetError`, containing exact
details, will be produced. The validation logic is centralized and you can call
it yourself, by using the `Validate` function on the
`cref:T:pblasucci.Ananoid.Alphabet` type (this, by the way, is the type which
defines all the pre-defined alphabets which ship with Ananoid), as shown below:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
match Alphabet.Validate(Numbers()) with
| Error e -> eprintfn $"Validation failed: %s{e.Message}"
| Ok valid -> printfn $"Alphabet validated: %s{valid.Letters}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim valid = Alphabet.ValidateOrThrow(New Numbers())

WriteLine($"Alphabet validated: {valid.Letters}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var valid = Alphabet.ValidateOrThrow(new Numbers());

WriteLine($"Alphabet validated: {valid.Letters}");
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

### Next steps

// TODO ???
