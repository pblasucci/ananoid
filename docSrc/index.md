Ananoid
===

This library provides nano identifiers, an alternative to UUIDs (inspired by
[https://github.com/ai/nanoid][1]).

### Overview

A nano identifier, or nanoid, is a randomly generated opaque value, suitable
for uniquely identifying database entries, file names, et cetera. Ananoid
(pronounced: "an-an-oyd") is a library for generating such identifiers.
It uses cryptographically secure random number generation, and has no
dependencies beyond the dotnet 6 base class libraries. It has both a high-level
API, and a simpler, more memory-efficient, low-level API.

### Installation via NuGet

The latest version of this package can be installed from [NuGet][3] via the
following command:

<details open class="lang-block">
<summary>CLI</summary>
```sh
> dotnet add package pblasucci.ananoid --version 0.8.2
```
</details>

### A simple example

The most common use-case for this library is to generate an new identifier,
based on sensible defaults (21 random characters from a URL-safe alphabet
consisting of: letters, numbers, underscore, and/or hyphen). A struct
representing such an identifier can be generated as follows:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
open pblasucci.Ananoid

// NOTE type annotations only for demonstration's sake
let nanoId : NanoId = NanoId.NewId()

printfn $"nano identifier as string: %s{string nanoId}"
printfn $"nano identifier length: %i{nanoId.Length}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Imports pblasucci.Ananoid
Imports System.Console

' NOTE type annotations only for demonstration's sake
Dim nanoId As NanoId = NanoId.NewId()

WriteLine($"nano identifier as string: {nanoId}")
WriteLine($"nano identifier length: {nanoId.Length}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
using pblasucci.Ananoid;
using static System.Console;

// NOTE type annotations only for demonstration's sake
NanoId nanoId = NanoId.NewId();

WriteLine($"nano identifier as string: {nanoId}");
WriteLine($"nano identifier length: {nanoId.Length}");
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

### Further reading

???

### Copyright
The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][2] file.


[1]: https://github.com/ai/nanoid
[2]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[3]: https://www.nuget.org/packages/pblasucci.ananoid
