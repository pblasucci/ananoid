Ananoid
===

Ananoid provides nano identifiers, an alternative to UUIDs. This project was
inspired by [https://github.com/ai/nanoid][1].

### Overview

A nano identifier, or nanoid, is a randomly generated opaque value, suitable
for uniquely identifying database entries, file names, et cetera.

Ananoid (pronounced: "an-an-oyd") is a library for generating such identifiers.
It uses cryptographically secure random number generation, and has no
dependencies beyond the .NET 8 base class libraries. It has both a high-level
API, and a simpler, more memory-efficient, low-level API.

### Recommended Reading

- [Getting Started][4]
- [API Documentation][5]
- [CHANGELOG][6]

### Installation via NuGet

The latest version of this package can be installed from [NuGet][2] via the
following command:

```sh
> dotnet add package pblasucci.ananoid --version 1.2.0
```

### Building from source

_Prerequisites:_

[.NET SDK version 10][3], or higher (note, .NET 10 is required to _build_ all
the projects in this repository. However, _consuming_ the
`pblasucci.ananoid.dll` assembly only requires .NET 8, or higher).

_Build steps:_

1. Clone this repo.
2. From a command prompt, move into the root of th cloned repo.
3. Restore the necessary tools (eg: `> dotnet tool restore`).
4. Compile all project in the solution (eg: `> dotnet build`).
5. Run the test suite (eg: `> dotnet test`).

_Preview the docs locally, with "hot reload":_

From a command prompt (assuming a basic build and test has worked), run the
following:

```sh
> dotnet fsdocs watch --input ./docSrc/ --properties Configuration=Release
```

_Product artifacts for relase:_

Run the following commands. Then upload .nupkg (under `./artifacts/`) to the desired locations
(NuGet.org, GitHub, et cetera).

#### NuGet package
```sh
> dotnet pack ./ananoid/ananoid.fsproj -c Release -o ./artifacts/
```

#### Complexity calculator

Run the following commands, replacing `{version}` with the version number of the release
(eg: `1.2.0`). Then compress the contents of each folder under `./artifacts/` into a zip file.
Finally, upload to the release page on GitHub.

```sh
> dotnet publish -c Release -r win-x64 --sc -o ./artifacts/ananoidcc-{version}-win-x64/ ./ananoidcc/
> dotnet publish -c Release -r linux-x64 --sc -o ./artifacts/ananoidcc-{version}-linux-x64/ ./ananoidcc/
```

---
> ### Shout Outs!
>
> Special thanks goes out to [TheAngryByrd][7] for doing all the dirty work
> around the mechanics of HTML layout / theming, which he then -- graciously --
> made available under MIT License.
>
> This repository's documentation would look much uglier without his efforts.
---

### Copyright

The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][0] file.

[0]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[1]: https://github.com/ai/nanoid
[2]: https://www.nuget.org/packages/pblasucci.ananoid
[3]: https://dotnet.microsoft.com/en-us/download/dotnet/10.0
[4]: https://pblasucci.github.io/ananoid/guides/nanoiddefault.html
[5]: https://pblasucci.github.io/ananoid/reference/pblasucci-ananoid.html
[6]: https://github.com/pblasucci/ananoid/blob/main/CHANGELOG.md
[7]: https://github.com/TheAngryByrd
