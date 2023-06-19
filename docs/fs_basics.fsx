(**
---
title: Basics
category: F# Guides
categoryindex: 1
index: 1
---
*)

(*** hide ***)
#i """nuget: /home/pblasucci/Source/ananoid/ananoid/bin/Release"""
#r "nuget: pblasucci.ananoid"
open pblasucci.Ananoid

(**
Getting Started
===

### A simple example

The most common use-case for this library is to generate an new identifier,
based on sensible defaults (21 random characters from a URL-safe alphabet
consisting of: letters, numbers, underscore, or hyphen). A struct representing
such an identifier can be generated as follows:
*)
let nanoId = NanoId.NewId()

printfn $"%s{nameof nanoId}, as string: %s{string nanoId}"
printfn $"%s{nameof nanoId}, length: %i{nanoId.Length}"
(*** include-output ***)

(**
### The zeroed instance

`NanoId` is actually a value type, and as such as a default representation --
the empty identifier. This is a single, special instance of a `NanoId` that has
a length of zero (0) and produces an empty string (when cast). The following
example shows different ways of creating and working with empty identifiers:
*)

let zeroedId = NanoId()
// ⮝⮝⮝ These two values are identical. ⮟⮟⮟
let emptyId = NanoId.Empty

printfn $"%s{nameof zeroedId} = %s{nameof emptyId}? %b{zeroedId = emptyId}"
printfn $"%s{nameof zeroedId}, length: %i{zeroedId.Length}"
printfn $"%s{nameof emptyId}, length: %i{emptyId.Length}"
(*** include-output ***)

(**
### Configuring generated values

Ananoid can be customized by changing either the "alphabet" from which the value
is generated, or by adjusting the character length of the generated instance.
In order to change either of these settings, `.NewId()` can receive an instance
of the `NanoIdOptions` type. And Ananoid ships with several [pre-defined instances][3]
(which track 1:1 the [pre-defined alphabets][2] also included in the library).
So, we could mimic the default behavior by calling the following:
*)
let sameAsDefault = NanoId.NewId(NanoIdOptions.UrlSafe)
printfn $"%s{nameof sameAsDefault} => %A{sameAsDefault}"
(*** include-output ***)

(**
But maybe we want the 64-character URL-safe identifier? Easy:
*)
let urlSafe64 = NanoIdOptions.UrlSafe.Resize(64)
let longerId = NanoId.NewId(urlSafe64)
printfn $"%s{nameof longerId} => %A{longerId}"
(*** include-output ***)

(**
Or if we wanted a 12-character value composed entirely of numbers:
*)
let twelveNumbers = NanoIdOptions.Numbers.Resize(12)
let numericId = NanoId.NewId(twelveNumbers)
printfn $"%s{nameof numericId} => %A{numericId}"
(*** include-output ***)

(**
We can even define out own alphabets ([read about that here][1])! But for now,
let's use one of the [pre-defined alphabets][2] that ship with Ananoid.
Plugging an alphabet into a `NanoIdOptions` instance is as follows:
*)
match NanoIdOptions.Create(Alphabet.HexadecimalUppercase, 16) with
| Ok nanoIdOptions ->
    let hexUp16Id = NanoId.NewId(nanoIdOptions)
    printfn $"%s{nameof hexUp16Id} => %A{hexUp16Id}"

| Error failure ->
    eprintfn $"Failed to create NanoIdOptions: %A{failure}"
(*** include-output ***)

(**
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
*)

let rawId = NanoId.NewId().ToString()

match NanoIdParser.UrlSafe.Parse(rawId) with
| Some parsedId ->
    printfn $"raw: %s{rawId}, parsed: %A{parsedId}"

| None ->
    eprintfn $"Failed to parse %s{rawId}! Found non-UrlSafe characters."
(*** include-output ***)

(**
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

*)

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
(*** include-output ***)

(**
### Next steps

// TODO ???

[1]: fs_customize.html
[2]: /reference/pblasucci-ananoid-alphabet.html
[3]: /reference/pblasucci-ananoid-nanoidoptions.html
*)
