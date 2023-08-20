---
title: Create a Default NanoId
category: How-To Guides
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
let nanoId = NanoId.ofDefaults ()

printfn $"%s{nameof nanoId}, as string: %s{string nanoId}"
printfn $"%s{nameof nanoId}, length: %i{nanoId.Length}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim nanoId = NanoId.NewId()

WriteLine($"{NameOf(nanoId)}, as string: {nanoId.ToString()})"
WriteLine($"{NameOf(nanoId)}, length: {nanoId.Length})"
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

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoiddefault.fsx

nano identifier as string: 6aWPM2MNoB_xnAt9ZCyL0
nano identifier length: 21
```
</details>
</div>

### The zeroed instance

`NanoId` is actually a [value type][1]. As such is has a "zero" representation
-- the empty identifier. This is a single, special instance of a `NanoId` that
has a length of zero (0) and produces an empty string (when cast). The following
example shows different ways of creating and working with empty identifiers:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let zeroedId = NanoId()
// ⮝⮝⮝ Identical! ⮟⮟⮟
let emptyId = NanoId.Empty

printfn $"%s{nameof zeroedId}, length: %i{NanoId.length zeroedId}"
printfn $"%s{nameof emptyId}, length: %i{NanoId.length emptyId}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim zeroedId As New NanoId()
' ⮝⮝⮝ Identical! ⮟⮟⮟
Dim emptyId = NanoId.Empty

WriteLine($"{NameOf(zeroedId)}, length: {zeroedId.Length}")
WriteLine($"{NameOf(emptyId)}, length: {emptyId.Length}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
NanoId zeroedId = new ();
// ⮝⮝⮝ Identical! ⮟⮟⮟
var emptyId = NanoId.Empty;

WriteLine($"{nameof(zeroedId)}, length: {zeroedId.Length}");
WriteLine($"{nameof(emptyId)}, length: {emptyId.Length}");
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoiddefault.fsx

zeroedId, length: 0
emptyId, length: 0
```
</details>
</div>

### Further Reading

+ [How-To: Customize NanoId Creation][2]
+ [How-To: Work with NanoId strings][3]

### Copyright
The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][0] file.


[0]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[1]: https://learn.microsoft.com/en-us/dotnet/standard/base-types/common-type-system#structures
[2]: ../guides/nanoidoptions.html
[3]: ../guides/nanoidstring.html
