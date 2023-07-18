---
title: Create a Default NanoId
category: Basics
categoryindex: 1
index: 1
---

How-To: Create a Default NanoId
===

### A simple example

The most common use-case for this library is to generate an new identifier,
based on sensible defaults (21 random characters from a URL-safe alphabet
consisting of: letters, numbers, underscore, or hyphen). A struct representing
such an identifier can be generated as follows:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let nanoId = NanoId.NewId()

printfn $"%s{nameof nanoId}, as string: %s{string nanoId}"
printfn $"%s{nameof nanoId}, length: %i{nanoId.Length}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim nanoId = NanoId.NewId()

WriteLine($"{nameof(nanoId)}, as string: {nanoId.ToString()})"
WriteLine($"{nameof(nanoId)}, length: {nanoId.Length})"
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var nanoId = NanoId.NewId();

WriteLine($"{nameof(nanoId)}, as string: {nanoId.ToString()})";
WriteLine($"{nameof(nanoId)}, length: {nanoId.Length})";
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

### The zeroed instance

`NanoId` is actually a [value type][1]. As such is has a "zero" representation --
the empty identifier. This is a single, special instance of a `NanoId` that has
a length of zero (0) and produces an empty string (when cast). The following
example shows different ways of creating and working with empty identifiers:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let zeroedId = NanoId()
// ⮝⮝⮝ These two values are identical. ⮟⮟⮟
let emptyId = NanoId.Empty

printfn $"%s{nameof zeroedId}, length: %i{zeroedId.Length}"
printfn $"%s{nameof emptyId}, length: %i{emptyId.Length}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim zeroedId As New NanoId()
// ⮝⮝⮝ These two values are identical. ⮟⮟⮟
Dim emptyId = NanoId.Empty

WriteLine($"{nameof(zeroedId)}, length: {zeroedId.Length}")
WriteLine($"{nameof(emptyId)}, length: {emptyId.Length}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
NanoId zeroedId = new ();
// ⮝⮝⮝ These two values are identical. ⮟⮟⮟
var emptyId = NanoId.Empty;

WriteLine($"{nameof(zeroedId)}, length: {zeroedId.Length}");
WriteLine($"{nameof(emptyId)}, length: {emptyId.Length}");
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

### Related Reading

+ API reference: `cref:T:pblasucci.Ananoid.NanoId`


[1]: https://learn.microsoft.com/en-us/dotnet/standard/base-types/common-type-system#structures
