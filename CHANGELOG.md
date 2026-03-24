# Ananoid Changelog

Ananoid provides nano identifiers, an alternative to UUIDs (inspired by [Inspiration]).

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog][Changelog],
and this project adheres to [Semantic Versioning][SemVer].

## [2.0.0] - 2026-03-24

### Changed

- Updated SDK to .NET 10
- Updated RTM to .NET 8
- Upgraded dependecies and tools
- More performance optimizations in `Core.fs`
- More benchmarks covering more scenarios
- BREAK! Replaced `AlphabetError` with `InvalidAlphabet`
- BREAK! Code which previously raised `TryMakeAlphabet` now raises `ArgumentOutOfRangeException`
- Updated documentation (new styles, better search, up-to-date content)

### Added

- `NanoId.IsEmpty` static method (analog to `NanoId.isEmpty` module function)
- `TryMakeAlphabet` extension method (on `String`), simplifies creation of custom alphabets
- `|Letters|` active pattern for extracting letters from `Alphabet`, `InvalidAlphabet`

## [1.1.0] - 2023-09-02

### Added

- Functions and methods which treat parsing "empty" inputs as a failure


## [1.0.1] - 2023-08-21

### Changed

- Lower `FSharp.Core` dependency version to `6.0.0`


## [1.0.0] - 2023-08-21

### Added

- Functions for generating nano identifiers as strings
- Functions for generating nano identifiers as "tagged strings" (F# only)
- Types and functions for working with nano identifers as opaque types
- Narrative and API documentation covering types, values, and behaviors
- A (desktop) utility for determining complexity of generated nano identifiers


[Inspiration]: https://github.com/ai/nanoid
[Changelog]: https://keepachangelog.com/en/1.0.0/
[SemVer]: https://semver.org/spec/v2.0.0.html
[Unreleased]: https://github.com/pblasucci/ananoid/compare/v1.1.0...HEAD
[1.0.0]: https://github.com/pblasucci/ananoid/releases/tag/v1.0.0
[1.0.1]: https://github.com/pblasucci/ananoid/releases/tag/v1.0.1
[1.1.0]: https://github.com/pblasucci/ananoid/releases/tag/v1.1.0
[2.0.0]: https://github.com/pblasucci/ananoid/releases/tag/v2.0.0
