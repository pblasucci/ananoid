---
title: Customize NanoId Creation
category: Basics
categoryindex: 1
index: 2
---

How-To: Customize NanoId Creation
===

### Configuring generated values

Ananoid can be customized by changing either the "alphabet" from which the value
is generated, or by adjusting the character length of the generated instance.
In order to change either of these settings, `.NewId()` can receive an instance
of the `NanoIdOptions` type. And Ananoid ships with several
[pre-defined instances][1]. So, we could mimic the default behavior by calling
the following:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let sameAsDefault = NanoId.NewId(NanoIdOptions.UrlSafe)

printfn $"%s{nameof sameAsDefault} => %A{sameAsDefault}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim sameAsDefault = NanoId.NewId(NanoIdOptions.UrlSafe)

WriteLine($"{nameof(sameAsDefault)} -> {sameAsDefault}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var sameAsDefault = NanoId.NewId(NanoIdOptions.UrlSafe);

WriteLine($"{nameof(sameAsDefault)} -> {sameAsDefault}");
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

But maybe we want the 64-character URL-safe identifier? Easy:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let urlSafe64 = NanoIdOptions.UrlSafe.Resize(64)
let longerId = NanoId.NewId(urlSafe64)

printfn $"%s{nameof longerId} => %A{longerId}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim urlSafe64 = NanoIdOptions.UrlSafe.Resize(64)
Dim longerId = NanoId.NewId(urlSafe64)

WriteLine($"{nameof(longerId)} => {longerId}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var urlSafe64 = NanoIdOptions.UrlSafe.Resize(64);
var longerId = NanoId.NewId(urlSafe64);

WriteLine($"{nameof(longerId)} => {longerId}");
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

Or if we wanted a 12-character value composed entirely of numbers:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let twelveNumbers = NanoIdOptions.Numbers.Resize(12)
let numericId = NanoId.NewId(twelveNumbers)

printfn $"%s{nameof numericId} => %A{numericId}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim twelveNumbers = NanoIdOptions.Numbers.Resize(12)
Dim numericId = NanoId.NewId(twelveNumbers)

WriteLine($"{nameof(numericId)} ->{numericId}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var twelveNumbers = NanoIdOptions.Numbers.Resize(12);
var numericId = NanoId.NewId(twelveNumbers);

WriteLine($"{nameof(numericId)} ->{numericId}");
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

We can even define out own alphabets ([read about that here][2])! But for now,
let's use one of the [pre-defined alphabets][3] that ship with Ananoid.
Plugging an alphabet into a `NanoIdOptions` instance is as follows:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
match NanoIdOptions.Create(Alphabet.HexadecimalUppercase, 16) with
| Ok nanoIdOptions ->
    let hexUp16Id = NanoId.NewId(nanoIdOptions)
    printfn $"%s{nameof hexUp16Id} => %A{hexUp16Id}"

| Error failure ->
    eprintfn $"Failed to create NanoIdOptions: %A{failure}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Imports Alphabet

Dim hexUp16Options =
  NanoIdOptions.CreateOrThrow(HexadecimalUppercase, size:=16)
Dim hexUp16Id = NanoId.NewId(hexUp16Options)

WriteLine($"{nameof(hexUp16Id)} -> {hexUp16Id}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
using static Alphabet;

var hexUp16Options =
  NanoIdOptions.CreateOrThrow(HexadecimalUppercase, size: 16);
var hexUp16Id = NanoId.NewId(hexUp16Options);

WriteLine($"{nameof(hexUp16Id)} -> {hexUp16Id}");
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

### Next steps

// TODO ???


[1]: /reference/pblasucci-ananoid-nanoidoptions.html
[2]: /guides/alphabets/definecustom.html
[3]: /reference/pblasucci-ananoid-alphabet.html
