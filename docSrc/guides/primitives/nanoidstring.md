---
title: Work with NanoId strings
category: Primitives
categoryindex: 3
index: 2
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

printfn $"%s{nameof defaultId}: %s{defaultId}"
printfn $"%s{nameof urlSafeId}: %s{urlSafeId}"

let alphabet, size = ("0123456789", 42)
let numericId = nanoIdOf alphabet size

printfn $"%s{nameof numericId}: %s{numericId}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Imports static pblasucci.Ananoid.Core

Dim defaultId = NewNanoId()
' ⮝⮝⮝ These two call do the same thing. ⮟⮟⮟
Dim urlSafeId = NewNanoId(Defaults.Alphabet, Defaults.Size)

WriteLine($"{NameOf(defaultId)}: {defaultId}")
WriteLine($"{NameOf(urlSafeId)}: {urlSafeId}")

Dim numericId = NewNanoId(alphabet:="0123456789", size:=42)

WriteLine($"{NameOf(numericId)}: {numericId}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
using static pblasucci.Ananoid.Core

val defaultId = NewNanoId();
// ⮝⮝⮝ These two call do the same thing. ⮟⮟⮟
val urlSafeId = NewNanoId(Defaults.Alphabet, Defaults.Size);

WriteLine($"{nameof(defaultId)}: {defaultId}");
WriteLine($"{nameof(urlSafeId)}: {urlSafeId}");

val numericId = NewNanoId(alphabet: "0123456789", size: 42);

WriteLine($"{nameof(numericId)}: {numericId}");
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoidstring.fsx

defaultId: StsrpEEfFWnoSSUqB0IyM
urlSafeId: Yg6PLr0_l2P6IsWgsMh3w
numericId: 176645656821584823660920061658558763998443
```
</details>
</div>

### Related Reading

+ [Performance: Select Highlights][2]
+ [Utilities: Complexity Calculator][3]

### Copyright
The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][0] file.


[0]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[2]: /explanations/performance/highlights.html
[3]: /explanations/utilities/complexity.html
