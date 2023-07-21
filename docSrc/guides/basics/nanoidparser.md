---
title: Parse an Existing String into a NanoId
category: Basics
categoryindex: 1
index: 3
---

How-To: Parse an Existing String into a NanoId
===

### Converting existing values

While _generating_ identifiers is the primary purpose for Ananoid, it is also
sometimes useful to _parse_ raw strings into `NanoId` instances (eg: when
rehydrating entities from a database). To help facilitate this conversion,
Ananoid provides a `NanoIdParser` type. A parser instance tries to convert
strings into nano identifiers. However, in doing so it validates that the
string in question _could_ have been created from a specific
`cref:T:pblasucci.Ananoid.IAlphabet` (nb: the length of the raw string is _not_
validated, other then to check if the associated alphabet supports, or
prohibits, zero-length string). Further, much like for options, Ananoid provides
`NanoIdParser` [instances for several well-known alphabets][1]. Here, we parse
a URL-safe identifier:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let rawId = NanoId.NewId().ToString()

match NanoIdParser.UrlSafe.Parse(rawId) with
| Some parsedId ->
    printfn $"raw: %s{rawId}, parsed: %A{parsedId}"

| None ->
    eprintfn $"Failed to parse %s{rawId}! Found non-UrlSafe characters."
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim rawId = NanoId.NewId().ToString()

Dim parsedId As NanoId
If NanoIdParser.UrlSafe.TryParse(rawId, parsedId) Then
    WriteLine($"raw: {rawId}, parsed: {parsedId}")
Else
    WriteLine($"Failed to parse {rawId}! Found non-UrlSafe characters.")
End If
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var rawId = NanoId.NewId().ToString();

var msg = NanoIdParser.UrlSafe.TryParse(rawId, out var parsedId) switch
{
    true => $"raw: {rawId}, parsed: {parsedId}"),
    false => $"Failed to parse {rawId}! Found non-UrlSafe characters."
};
WriteLine(msg);
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoidparser.fsx

raw: ZoCRoGew6hVWYIIimu-p4, parsed: ZoCRoGew6hVWYIIimu-p4
```
</details>
</div>

In the same way that we can build a `NanoIdOptions` instance from any alphabet,
we can also use an arbitrary alphabet to create a new `NanoIdParser` instance.

> ---
> _How's that work?_
>
> `NanoIdParser` uses a simple algorithm, which ultimately delegates to
> `cref:M:pblasucci.Ananoid.IAlphabet.WillPermit`, which is required of all
> `IAlphabet` instances.
>
> ---

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
match NanoIdParser.Create(Alphabet.NoLookalikes) with
| Ok nanoIdParser ->
    let originalId = string (NanoId.NewId NanoIdOptions.NoLookalikes)

    match nanoIdParser.Parse originalId with
    | Some noLookalikeId ->
        printfn $"original: %s{originalId}, parsed: %A{noLookalikeId}"

    | None ->
        eprintfn $"Failed to parse %s{originalId}! Found 'lookalikes'."

| Error failure ->
    eprintfn $"Failed to coreate NanoIdOptions: %A{failure}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Imports NanoIdOptions

Dim parser = NanoIdParser.CreateOrThrow(Alphabet.NoLookalikes)
Dim originalId = NanoId.NewId(NoLookalikes).ToString()

Dim noLookalikeId As NanoId
If parser.TryParse(originalId, noLookalikeId) Then
    WriteLine($"original: {originalId}, parsed: {noLookalikeId}")
Else
    WriteLine($"Failed to parse {originalId}! Found 'lookalikes'.")
End If
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
using static NanoIdOptions;

var parser = NanoIdParser.CreateOrThrow(Alphabet.NoLookalikes);
var originalId = NanoId.NewId(NoLookalikes).ToString();

var msg = parser.TryParse(originalId, out var noLookalikeId) switch
{
    true => $"original: {originalId}, parsed: {noLookalikeId}"),
    false => $"Failed to parse {originalId}! Found 'lookalikes'."
};
WriteLine(msg);
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoidparser.fsx

raw: BEC7biEHCUDwRihgFCExa, parsed: BEC7biEHCUDwRihgFCExa
```
</details>
</div>

### Related Reading

+ [How-To: Work with NanoId strings][2]
+ API Reference: `cref:T:pblasucci.Ananoid.NanoId`

### Copyright
The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][0] file.


[0]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[1]: /reference/pblasucci-ananoid-nanoidparser.html
[2]: /guides/primitives/nanoidstring.html
