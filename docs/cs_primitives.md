---
title: Primitives
category: C# Guides
categoryindex: 2
index: 3
---

Low-level Functions
===

Ananoid can serve most uses cases via the `cref:T:pblasucci.Ananoid.NanoId`
type and its associates (`cref:T:pblasucci.Ananoid.NanoIdOptions`,
`cref:T:pblasucci.Ananoid.NanoIdParser`, et cetera). However, sometimes this is
not desired (or at least, not _optimal_). For times when the a struct or class
is just too much, Ananoid also provides its core functionality --
cryptographically-secure randomly-generated identifiers -- as functions which
take simple inputs and just produce strings. These primitive functions are
located in the `cref:T:pblasucci.Ananoid.Core` module, and offer two variants:
one based on default values, and one which allows customizing both alphabet and
size. The following example demonstrates:

```csharp
using static pblasucci.Ananoid.Core

val defaultId = NewNanoId();
val urlSafeId = NewNanoId(Defaults.Alphabet, Defaults.Size);
val numericId = NewNanoId(alphabet: "0123456789", size: 42);

WriteLine($"Defaults.Alphabet = {Defaults.Alphabet}");
WriteLine($"Defaults.Size = {Defaults.Size}");
WriteLine("---");
WriteLine($"{nameof defaultId} = {defaultId}");
WriteLine($"{nameof urlSafeId} = {urlSafeId}");
WriteLine($"{nameof numericId} = {numericId}");
```

### Next steps

// TODO ???
