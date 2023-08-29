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
`cref:T:pblasucci.Ananoid.AlphabetError` type, which provides details about
why, exactly, a given set of letters is not valid.

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
match Alphabet.ofLetters String.Empty with
| Ok valid -> printfn $"%s{valid.Letters} are valid."
| Error(AlphabetTooLarge letters) -> printfn "Too large: '%s{letters}'!"
| Error(AlphabetTooSmall letters) -> printfn "Too small: '%s{letters}'!"
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
  Dim error = checked.ErrorValue
  Select True
    Case error.IsAlphabetTooLarge
      WriteLine($"Too large: '{error.Letters}'!")

    Case error.IsAlphabetTooSmall
      WriteLine($"Too small: '{error.Letters}'!")

    Case Else
      Throw New UnreachableException()
  End Select
End If
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var @checked = String.Empty.ToAlphabet();

var message = @checked switch
{
  { IsOk: true, ResultValue: var alphabet } => $"{alphabet.Letters} are valid.",

  { ErrorValue: var error } => error switch
  {
    { IsAlphabetTooLarge: true } => $"Too large: '{error.Letters}'!",
    { IsAlphabetTooSmall: true } => $"Too small: '{error.Letters}'!",

    _ => throw new UnreachableException()
  },

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

However, sometimes, processing complex failures is uncessary (or, at least,
undesirable). In those cases, Ananoid can raise an
`cref:T:pblasucci.Ananoid.AlphabetException`, which not only surfaces an
`AlphabetError` but also halts program flow and captures a stack trace. This
is shown in the following example:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
try
  Alphabet.makeOrRaise (String.replicate 800 "$")
with
| :? AlphabetException as x -> printfn $"FAIL! %A{x.Reason}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Try
  Dim letters = New String("$"c, 800)
  Dim alphabet = letters.ToAlphabetOrThrow()
  WriteLine($"{alphabet.Letters} are valid.")

Catch x As AlphabetException
  WriteLine($"FAIL! {x.Reason}")

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
catch (AlphabetException x)
{
  WriteLine($"FAIL! {x.Reason}");
}
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/definecustom.fsx

FAIl! AlphabetTooLarge
  "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$"
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
