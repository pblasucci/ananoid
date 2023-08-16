---
title: Customize NanoId Creation
category: How-To Guides
categoryindex: 1
index: 2
---

How-To: Customize NanoId Creation
===

### Configuring generated values

While the default settings for a `NanoId` (21 random characters from a URL-safe
alphabet consisting of: letters, numbers, underscore, or hyphen) are excellent
for most use cases, it remains possible to generate instances of varying sizes,
or even generate an instance from an entirely different alphabet. Further, the
Ananoid library ships with several [common alphabets already defined][1].
For example, the default behavior can be mimicked as follows:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let sameAsDefault = NanoId.ofOptions KnownAlphabets.UrlSafe 21

printfn $"%s{nameof sameAsDefault}: %A{sameAsDefault}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim sameAsDefault = KnownAlphabets.UrlSafe.MakeNanoId(size:=21)

WriteLine($"{NameOf(sameAsDefault)}: {sameAsDefault}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var sameAsDefault = KnownAlphabets.UrlSafe.MakeNanoId(size: 21);

WriteLine($"{nameof(sameAsDefault)}: {sameAsDefault}");
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoidoptions.fsx

sameAsDefault: Yq1CtDALLQCgP-XIBlzE6
```
</details>
</div>

But maybe we want a 64-character URL-safe identifier instead? Easy:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
open KnownAlphabets

let longerId = UrlSafe |> Alphabet.makeNanoId 64

printfn $"%s{nameof longerId}: %A{longerId}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Imports KnownAlphabets

Dim longerId = UrlSafe.MakeNanoId(size:=64)

WriteLine($"{NameOf(longerId)}: {longerId}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
using static KnownAlphabets;

var longerId = UrlSafe.MakeNanoId(size: 64);

WriteLine($"{nameof(longerId)}: {longerId}");
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoiddefault.fsx

longerId: xXFsRF7OGQK9dXpYZp6i88wlTU6YaVGdPWAHJTyo6SjHy-whflF7Lom0oVJerVoM
```
</details>
</div>

Or if we wanted a 12-character value composed entirely of numbers:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let numericId = Numbers.MakeNanoId(size = 128)

printfn $"%s{nameof numericId}: %A{numericId}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim numericId = Numbers.MakeNanoId(size:=128)

WriteLine($"{NameOf(numericId)}: {numericId}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var numericId = Numbers.MakeNanoId(size: 128);

WriteLine($"{nameof(numericId)}: {numericId}");
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoiddefault.fsx

numericId: 63605584488709912960741866160961621054311208530158529005938917360552694066372962792631604006204502313290707959512413672018143848
```
</details>
</div>

### Further Reading

+ [Utilities: Complexity Calculator][1]
+ [How-To: Define a Custom Alphabet][2]

### Copyright
The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][0] file.


[0]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[1]: /explanations/complexity.html
[2]: /guides/definecustom.html
