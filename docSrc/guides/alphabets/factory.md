---
title: By-Pass Using NanoIdOptions
category: Alphabets
categoryindex: 2
index: 3
---

How-To: By-Pass Using NanoIdOptions
===

Ananoid requires a valid `IAlphabet` in several places. Notably, it is an
input for:

+ Creating a new `cref:T:pblasucci.Ananoid.NanoIdParser` instance.
+ Creating a new `cref:T:pblasucci.Ananoid.NanoIdOptions` instance.

In other case, the given alphabet will be validate internally. That is, the
caller is _not_ responsible for invoking `Alphabet.Validate`. Regardless of the
type being instantiated, if alphabet validation fails, an `AlphabetError` will
be returned (just as we have previously seen).

### From alphabets to factories

However, this is one other place where an alphabet is useful. A caller may use
one to create a 'nano id factory'. This factory is just a function -- one which
has already validated and captured the given alphabet. Callers can then use
this factory (function) to create new `NanoId` instances of varying lengths, as
in the following example:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
match Numbers().ToNanoIdFactory() with
| Error e -> eprintfn $"Validation failed: %s{e.Message}"
| Ok factory ->
    let sixtyFourDigits = factory 64
    printfn $"%s{string sixtyFourDigits} length? %i{sixtyFourDigits.Length}"

    // NOTE the generated function returns an empty `NanoId` for any imput < 1
    let noDigits = factory -3
    printfn $"%s{nameof noDigits} is empty? %b{NanoId.IsEmpty noDigits}"
```
</details>

<details open class="lang-block">
<summary>VB</summary>

```vb
Dim factory = Call New Numbers().ToNanoIdFactory()

Dim sixtyFourDigits = factory(64)
WriteLine($"{sixtyFourDigits} length? {sixtyFourDigits.Length}")

' NOTE the generated function returns an empty `NanoId` for any imput < 1
Dim noDigits = factory(-3)
WriteLine($"{NameOf(noDigits)} is empty? {NanoId.IsEmpty(noDigits)}")
```
</details>

<details open class="lang-block">
<summary>C#</summary>

```csharp
var factory = (New Numbers()).ToNanoIdFactory();

var sixtyFourDigits = factory(64);
WriteLine($"{sixtyFourDigits} length? {sixtyFourDigits.Length}");

// NOTE the generated function returns an empty `NanoId` for any imput < 1
var noDigits = factory(-3);
WriteLine($"{NameOf(noDigits)} is empty? {NanoId.IsEmpty(noDigits)}");
```
</details>
</div>

![TODO: output of last snippet](/path/to.img)

### Next steps

// TODO ???
