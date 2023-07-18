---
title: Work with NanoId strings
category: Primitives
categoryindex: 3
index: 1
---

How-To: Work with NanoId strings
===

Ananoid can serve most uses cases via the `cref:T:pblasucci.Ananoid.NanoId`
type and its associates (`cref:T:pblasucci.Ananoid.NanoIdOptions`,
`cref:T:pblasucci.Ananoid.NanoIdParser`, et cetera). However, sometimes this is
not desired (or at least, not _optimal_). For times when the a struct or class
is just too much, Ananoid also provides its core functionality --
cryptographically-secure randomly-generated identifiers -- as functions which
take simple inputs and just produce strings.

These primitive functions are located in the `cref:T:pblasucci.Ananoid.Core`
module, and offer two variants: one based on default values, and one which
allows customizing both alphabet and size. The following example demonstrates:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
open pblasucci.Ananoid.Core

let defaultId = nanoId ()
// ⮝⮝⮝ These two call do the same thing. ⮟⮟⮟
let urlSafeId = nanoIdOf Defaults.Alphabet Defaults.Size

let alphabet, size = ("0123456789", 42)
let numericId = nanoIdOf alphabet size
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Imports static pblasucci.Ananoid.Core

Dim defaultId = NewNanoId()
' ⮝⮝⮝ These two call do the same thing. ⮟⮟⮟
Dim urlSafeId = NewNanoId(Defaults.Alphabet, Defaults.Size)

Dim numericId = NewNanoId(alphabet:="0123456789", size:=42)
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
using static pblasucci.Ananoid.Core

val defaultId = NewNanoId();
// ⮝⮝⮝ These two call do the same thing. ⮟⮟⮟
val urlSafeId = NewNanoId(Defaults.Alphabet, Defaults.Size);

val numericId = NewNanoId(alphabet: "0123456789", size: 42);
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

### Next steps

// TODO ???
