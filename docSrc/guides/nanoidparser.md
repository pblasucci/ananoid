---
title: Parse an Existing String into a NanoId
category: How-To Guides
categoryindex: 1
index: 3
---

How-To: Parse an Existing String into a NanoId
===

### Converting existing values

While _generating_ identifiers is the primary purpose for Ananoid, it is also
sometimes useful to _parse_ raw strings into `NanoId` instances (eg: when
rehydrating entities from a database). To help facilitate this, Ananoid
provides a fwe utilites, each of which validates that the string in question
_could_ have been created from a given `cref:T:pblasucci.Ananoid.Alphabet`
instance. Here, we parse a URL-safe identifier:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
open KnownAlphabets

let nanoId = NanoId.NewId()

nanoId
|> string
|> NanoId.parseAs UrlSafe
|> Option.iter (fun parsed -> printfn $"nanoId: %A{nanoId}, parsed: %A{parsed}")
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Imports KnownAlphabets

Dim nanoId = NanoId.NewId()

Dim parsed As NanoId
If UrlSafe.TryParse(nanoId.ToString(), Out parsed) Then
  WriteLine($"nanoId: {nanoId}, parsed: {parsed}")
End If
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
open static KnownAlphabets

var nanoId = NanoId.NewId();

if (UrlSafe.TryParse(nanoId.ToString(), out var parsed))
{
  WriteLine($"nanoId: {nanoId}, parsed: {parsed}");
}
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoidparser.fsx

nanoId: ZoCRoGew6hVWYIIimu-p4, parsed: ZoCRoGew6hVWYIIimu-p4
```
</details>
</div>


Next, we see what happens if a raw string _cannot_ be constituted from a given
alphabet (eg, using a purely numeric alphaber to parse a hexidecimal string):

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let nanoId = HexidecimalUppercase.MakeNanoId(size = 32)

match Numbers.ParseNanoId(string nanoId) with
| Some parsed ->
    printfn $"nanoId: %A{nanoId}, parsed: %A{parsed}"

| None ->
    printfn $"Failed to parse '%A{nanoId}' as a numeric nano identifer!"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim nanoId = HexidecimalUppercase.MakeNanoId(size:=32)

Dim parsed As NanoId
If UrlSafe.TryParse(nanoId.ToString(), Out parsed) Then
  WriteLine($"nanoId: {nanoId}, parsed: {parsed}")
Else
  WriteLine($"Failed to parse '{nanoId}' as a numeric nano identifer!")
End If
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var nanoId = HexidecimalUppercase.MakeNanoId(size: 32);

var message =
  UrlSafe.TryParse(nanoId.ToString(), out var parsed)
  ? $"nanoId: {nanoId}, parsed: {parsed}"
  : $"Failed to parse '{nanoId}' as a numeric nano identifer!";
WriteLine(message);
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoidparser.fsx

Failed to parse 'B978025EB0903089A89F40E43632587D' as a numeric nano identifer!
```
</details>
</div>

Finally, it is worth noting: the length of the string being parsed is _not_
validated. However, strings which are: `null`, zero-lengh, or consist only of
whitespace will result in a parsed value of `cref:M:pblasucci.Ananoid.NanoId.Empty`.

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
NoLookalikes
|> Alphabet.parseNanoId String.Empty
|> Option.iter (fun parsed ->
  printfn $"Is empty? %b{NanoId.isEmpty parsed}"
)
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim parsed As NanoId
If NoLookalikes.TryParse(nanoId.ToString(), parsed) Then
  WriteLine($"Is empty? {NanoId.Empty.Equals(parsed)}")
End If
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
if (NoLookalikes.TryParse(nanoId.ToString(), out var parsed))
{
  WriteLine($"Is empty? {NanoId.Empty.Equals(parsed)}");
}
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/nanoidparser.fsx

Is empty? true
```
</details>
</div>


### Related Reading

+ [How-To: Work with NanoId strings][1]
+ API Reference: `cref:T:pblasucci.Ananoid.KnownAlphabets`

### Copyright
The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][0] file.


[0]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[1]: /guides/nanoidstring.html
