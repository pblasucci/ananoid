Ananoid
===

Ananoid provides nano identifiers, an alternative to UUIDs. This project was
inspired by [https://github.com/ai/nanoid][1].

### Overview

A nano identifier, or nanoid, is a randomly generated opaque value, suitable
for uniquely identifying database entries, file names, et cetera.

Ananoid (pronounced: "an-an-oyd") is a library for generating such identifiers.
It uses cryptographically secure random number generation, and has no
dependencies beyond the dotnet 6 base class libraries. It has both a high-level
API, and a simpler, more memory-efficient, low-level API.

### Recommended Reading

- [Getting Started][4]
- [API Documentation][5]
- [CHANGELOG][6]

### Installation via NuGet

The latest version of this package can be installed from [NuGet][2] via the
following command:

```sh
> dotnet add package pblasucci.ananoid --version 1.1.0
```

### Building from source

_Prerequisites:_

[.NET SDK version 7][3], or higher (note, .NET 7 is required to _build_ all
the projects in this repository. However, _consuming_ the `pblasucci.ananoid.dll`
file only requires .NET 6).

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
> dotnet fsdocs watch --port 2023 --input ./docSrc/ \
  --properties Configuration=Release \
  --sourcerepo https://github.com/pblasucci/ananoid
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
[3]: https://dotnet.microsoft.com/en-us/download/dotnet/7.0
[4]: https://pblasucci.github.io/ananoid/guides/nanoiddefault.html
[5]: https://pblasucci.github.io/ananoid/reference/pblasucci-ananoid.html
[6]: https://github.com/pblasucci/ananoid/blob/main/CHANGELOG.md
[7]: https://github.com/TheAngryByrd
