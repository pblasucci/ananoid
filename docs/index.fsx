(*** hide ***)
#i """nuget: /home/pblasucci/Source/ananoid/ananoid/bin/Release"""
#r "nuget: pblasucci.ananoid"

(**
ananoid
===

This library provides nano identifiers, an alternative to UUIDs (inspired by
[https://github.com/ai/nanoid][1]).

### Overview

A nano identifier, or nanoid, is a randomly generated opaque value, suitable
for uniquely identifying database entries, file names, et cetera. Ananoid
(pronounced: "an-an-oyd") is a library for generating such identifiers.
It uses cryptographically secure random number generation, and has no
dependencies beyond the dotnet 7 base class libraries. It has both a high-level
API, and a simpler, more memory-efficient, low-level API.
*)

(*** hide, define-output: version ***)
let assemblyName = typeof<pblasucci.Ananoid.NanoId>.Assembly.GetName()
let nm, vsn = assemblyName.Name, assemblyName.Version
printfn $"> dotnet add package {nm} --version {vsn.Major}.{vsn.Minor}.{vsn.Build}"

(**
### Installation via NuGet

The latest version of this package can be installed from [NuGet][3] via the
following command:
*)
(*** include-output: version ***)

(**
> ---
> _Special circumstances_
>
> While it is strongly recommend to reference the [Ananoid NuGet package][3]
> (and take full advantage of the range of features offered by the library),
> there are some circumstances wherein it might be advantages to only take the
> lowest-level parts of the library as a _source-code dependency_ (the
> so-called "vendorizing" of dependencies). In order to accomodate this, the
> library has been carefully structured so that you may simply copy the file
> [`Core.fs`][4] into your project and party on! Please note, the low-level
> types, values, and functions are in the module `pblasucci.Ananoid.Core`.
>
> ---
*)

(**
### A simple example

The most common use-case for this library is to generate an new identifier,
based on sensible defaults (21 random characters from a URL-safe alphabet
consisting of: letters, numbers, underscore, or hyphen). A struct representing
such an identifier can be generated as follows (in F#):
*)
open pblasucci.Ananoid

let nanoId = NanoId.NewId()

printfn $"nano identifier as string: %s{string nanoId}"
printfn $"nano identifier length: %i{nanoId.Length}"
(*** include-output ***)

(**
Alternatively, a C# client might be coded as follows:

```csharp
using pblasucci.Ananoid;

using static System.Console;

var nanoId = NanoId.NewId();

WriteLine($"{nameof(nanoId)}, as string: {nanoId.ToString()}";
WriteLine($"{nameof(nanoId)}, length: {nanoId.Length}";
```

### Further reading

???

### Copyright
The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][2] file.


[1]: https://github.com/ai/nanoid
[2]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[3]: https://www.nuget.org/packages/pblasucci.ananoid
[4]: https://github.com/pblasucci/ananoid/blob/c7b6f7a5e38a38f651af267107ab18b1d00c050d/ananoid/Core.fs
*)
