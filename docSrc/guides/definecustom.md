---
title: Define a Custom Alphabet
category: How-To Guides
categoryindex: 1
index: 4
---

How-To: Define a Custom Alphabet
===

The default settings for creating a `cref:T:pblasucci.Ananoid.NanoId`
(21 characters taken from a mix of letters, numbers, hyphen, and underscore)
reflect a reasonable balance of entropy versus performance. Further the
additional alphabets shipped with Ananoid cover a wide range of common needs.
But it is possible to go further. Consumers can define their _own_ alphabets.

### Learning about alphabets

Conceptually, an 'alphabet' is a set of 'letters' (technically, single-byte
characters) from which a `NanoId` is constituted. In practice, an
`cref:T:pblasucci.Ananoid.Alphabet` instance represents a _valildated_ set of
letters. Specifically, an `Alphabet` is safe to use for the generation and
parsing of nano identifiers because it upholds the following invariants:

+ The set of letters is NOT `null`.
+ The set of letters MUST contain at least one (1) non-whitespace letter.
+ The set of letters MAY NOT contains more than 255 letters.

These are not the most challenging invariants, and any set of letters which
conforms to them can be validated as an `Alphabet`. For example, one could
define an alphabet consisting entirely of upper case ASCII letters:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let uppercase = Alphabet.Validate("ABCDEFGHIJKLMNOPQRSTUVWXYZ")

printfn $"Is alphabet valid? %b{Result.isOk uppercase}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim uppercase = Alphabet.Validate("ABCDEFGHIJKLMNOPQRSTUVWXYZ")

WriteLine($"Is alphabet valid? {uppercase.IsOk}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var uppercase = Alphabet.Validate("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

WriteLine($"Is alphabet valid? {uppercase.IsOk}");
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/definecustom.fsx

Is alphabet valid? true
```
</details>
</div>

### Dealing with failures

Not all letter sets will be valid. When validation fails, Ananoid provides the
`cref:T:pblasucci.Ananoid.InvalidAlphabet` type, which provides details about
why, exactly, a given set of letters is not valid.

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
open AlphabetPatterns // ⮜⮜⮜ contains the `(|Letters|)` active pattern

match Alphabet.ofLetters String.Empty with
| Ok valid -> printfn $"%s{valid.Letters} are valid."

| Error(Letters invalid) when 255 < String.length invalid -> printfn "Too large: '%s{invalid}'!"
| Error(Letters invalid) when String.length invalid < 1 -> printfn "Too small: '%s{invalid}'!"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim checked = String.Empty.ToAlphabet()

If checked.IsOk Then
  Dim alphabet = checked.ResultValue
  WriteLine($"{alphabet.Letters} are valid.")
Else
  Dim [error] = checked.ErrorValue
  Select True
    Case 255 < [error].Letters.Length
      WriteLine($"Too large: '{[error].Letters}'!")

    Case [error].Letters.Length < 1
      WriteLine($"Too small: '{[error].Letters}'!")

    Case Else
      Throw New UnreachableException()
  End Select
End If
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var @checked = string.Empty.ToAlphabet();

var message = @checked switch
{
  { IsOk: true, ResultValue: var alphabet } => $"{alphabet.Letters} are valid.",

  { ErrorValue: { Letters: var letters} } when 255 < letters.Length => $"Too large: '{letters}'!",
  { ErrorValue: { Letters: var letters} } when letters.Length < 1 => $"Too small: '{letters}'!",

  _ => throw new UnreachableException()
};

WriteLine(message);
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/definecustom.fsx

Too small: ''!
```
</details>
</div>

However, sometimes, processing failures is uncessary (or, at least, unwanted).
In those cases, Ananoid has helper methods which reduce success-or-failure to a
boolean condition. Consider the following:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
match String.Empty.TryMakeAlphabet() with
| (true, alphabet) -> printfn $"%s{alphabet.Letters} are valid."
| (false, _) -> printfn "Too small: ''!
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim alphabet As Alphabet = Nothing
Dim isOkay = String.Empty.TryMakeAlphabet(alphabet)
If Not isOkay AndAlso alphabet Is Nothing Then
  WriteLine("Too small: ''!")
Else
  WriteLine($"{alphabet.Letters} are valid.")
End If
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
if ("".TryMakeAlphabet(out var alphabet))
{
  Console.WriteLine($"{alphabet.Letters} are valid.");
}
else
{
  Console.WriteLine("Too small: ''!");
}
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/definecustom.fsx

Too small: ''!
```
</details>
</div>

Further, Ananoid can escalate failures by raising a
`cref:T:System.ArgumentOutOfRangeException`, which surfaces details from an
`InvalidAlphabet` while also halting program flow and capturing a stack trace.
This is shown in the following example:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
try
  Alphabet.makeOrRaise (String.replicate 800 "$")
with
| :? ArgumentOutOfRangeException as x -> printfn $"FAIL! %s{x.Message}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Try
  Dim letters = New String("$"c, 800)
  Dim alphabet = letters.ToAlphabetOrThrow()
  WriteLine($"{alphabet.Letters} are valid.")

Catch x As ArgumentOutOfRangeException
  WriteLine($"FAIL! {x.Message}")

End Try
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
try
{
  var alphabet = new String('$', 300).ToAlphabetOrThrow();
  WriteLine($"{alphabet.Letters} are valid.");
}
catch (ArgumentOutOfRangeException x)
{
  WriteLine($"FAIL! {x.Message}");
}
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/definecustom.fsx

FAIL! must be between 1 and 255 letters (Parameter 'letters')
```
</details>
</div>

### Related Reading

+ [Utilities: Complexity Calculator][1]
+ [How-To: Parse an Existing String into a NanoId][2]

### Copyright
The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][0] file.


[0]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[1]: ../explanations/complexity.html
[2]: ../guides/nanoidparser.html
