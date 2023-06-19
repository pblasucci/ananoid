---
title: Basics
category: C# Guides
categoryindex: 2
index: 1
---

Getting Started
===

### A simple example

The most common use-case for this library is to generate an new identifier,
based on sensible defaults (21 random characters from a URL-safe alphabet
consisting of: letters, numbers, underscore, or hyphen). A struct representing
such an identifier can be generated as follows:

```csharp
var nanoId = NanoId.NewId();

WriteLine($"{nameof(nanoId)}, as string: {nanoId.ToString()})";
WriteLine($"{nameof(nanoId)}, length: {nanoId.Length})";
```

### The zeroed instance

`NanoId` is actually a value type, and as such as a default representation --
the empty identifier. This is a single, special instance of a `NanoId` that has
a length of zero (0) and produces an empty string (when cast). The following
example shows different ways of creating and working with empty identifiers:

```csharp
NanoId zeroedId = new ();
// ⮝⮝⮝ These two values are identical. ⮟⮟⮟
var emptyId = NanoId.Empty;

WriteLine($"{nameof(zeroedId)} = {nameof(emptyId)}? {(zeroedId = emptyId)});"
WriteLine($"{nameof(zeroedId)}, length: {zeroedId.Length});"
WriteLine($"{nameof(emptyId)}, length: {emptyId.Length});"
```

### Configuring generated values

Ananoid can be customized by changing either the "alphabet" from which the value
is generated, or by adjusting the character length of the generated instance.
In order to change either of these settings, `.NewId()` can receive an instance
of the `NanoIdOptions` type. And Ananoid ships with several [pre-defined instances][3]
(which track 1:1 the [pre-defined alphabets][2] also included in the library).
So, we could mimic the default behavior by calling the following:

```csharp
var sameAsDefault = NanoId.NewId(NanoIdOptions.UrlSafe);

WriteLine($"{nameof(sameAsDefault)} -> {sameAsDefault}");
```

But maybe we want the 64-character URL-safe identifier? Easy:

```csharp
var urlSafe64 = NanoIdOptions.UrlSafe.Resize(64);
var longerId = NanoId.NewId(urlSafe64);

WriteLine($"{nameof(longerId)} => {longerId}");
```

Or if we wanted a 12-character value composed entirely of numbers:

```csharp
var twelveNumbers = NanoIdOptions.Numbers.Resize(12);
var numericId = NanoId.NewId(twelveNumbers);

WriteLine($"{nameof(numericId)} ->{numericId}");
```

We can even define out own alphabets ([read about that here][1])! But for now,
let's use one of the [pre-defined alphabets][2] that ship with Ananoid.
Plugging an alphabet into a `NanoIdOptions` instance is as follows:

```csharp
using static Alphabet;

var hexUp16Options = NanoIdOptions.CreateOrThrow(HexadecimalUppercase, size: 16);
var hexUp16Id = NanoId.NewId(hexUp16Options);

WriteLine($"{nameof(hexUp16Id)} -> {hexUp16Id}");
```

### Converting existing values

While _generating_ identifiers is the primary purpose for Ananoid, it is also
sometimes useful to _parse_ raw strings into `NanoId` instances (eg: when
rehydrating entities from a database). To help facilitate this conversion,
Ananoid provides a `NanoIdParser` type. A parser instance tries to convert
strings into nano identifiers. However, in doing so it validates that the
string in question _could_ have been created from a specific
`cref:T:pblasucci.Ananoid.IAlphabet` (nb: the length of the raw string is _not_
validated). Further, much like for options, Ananoid provides a `NanoIdParser`
instance for several well-known alphabets. Here, we parse a URL-safe identifier:

```csharp
var rawId = NanoId.NewId().ToString();

WriteLine(
    NanoIdParser.UrlSafe.TryParse(rawId, out var parsedId)
    ? $"raw: {rawId}, parsed: {parsedId}")
    : $"Failed to parse {rawId}! Found non-UrlSafe characters."
);
```

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

```csharp
using static NanoIdOptions;

var parser = NanoIdParser.CreateOrThrow(Alphabet.NoLookalikes);
var originalId = NanoId.NewId(NoLookalikes).ToString();

WriteLine(
    parser.TryParse(originalId, out var noLookalikeId)
    ? $"original: {originalId}, parsed: {noLookalikeId}")
    : $"Failed to parse {originalId}! Found 'lookalikes'."
);
```


### Next steps

// TODO ???

[1]: fs_customize.html
[2]: /reference/pblasucci-ananoid-alphabet.html
[3]: /reference/pblasucci-ananoid-nanoidoptions.html
