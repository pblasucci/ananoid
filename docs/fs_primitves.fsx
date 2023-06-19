(**
---
title: Primitives
category: F# Guides
categoryindex: 1
index: 3
---
*)

(*** hide ***)
#i """nuget: /home/pblasucci/Source/ananoid/ananoid/bin/Release"""
#r "nuget: pblasucci.ananoid"

(**
Low-level Functions
===

Ananoid can serve most uses cases via the `cref:T:pblasucci.Ananoid.NanoId`
type and its associates (`cref:T:pblasucci.Ananoid.NanoIdOptions`,
`cref:T:pblasucci.Ananoid.NanoIdParser`, et cetera). However, sometimes this is
not desired (or at least, not _optimal_). For times when the a struct or class
is just too much, Ananoid also provides its core functionality --
cryptographically-secure randomly-generated identifiers -- as functions which
take simple inputs and just produce strings.

> ---
> **Aside: Just the primitives, please.**
>
> There are some circumstances wherein it might be advantageous to only take
> the lowest-level parts of the library as a _source-code dependency_ (the
> so-called "vendorizing" of dependencies). In order to accomodate this, the
> library has been carefully structured so that you may simply copy the file
> [`Core.fs`][2] into your project and treat these functions as just another
> module in your library/application/et cetera (nb: this only applies for F#
> projects... sorry ¯\_(ツ)_/¯ ).
>
> ---

These primitive functions are located in the `cref:T:pblasucci.Ananoid.Core`
module, and offer two variants: one based on default values, and one which
allows customizing both alphabet and size. The following example demonstrates:
*)
open pblasucci.Ananoid.Core

let defaultId = nanoId ()
let urlSafeId = nanoIdOf Defaults.Alphabet Defaults.Size
let numericId = nanoIdOf "0123456789" 42

printfn $"Defaults.Alphabet = %s{Defaults.Alphabet}"
printfn $"Defaults.Size = %i{Defaults.Size}"
printfn "---"
printfn $"%s{nameof defaultId} = %s{defaultId}"
printfn $"%s{nameof urlSafeId} = %s{urlSafeId}"
printfn $"%s{nameof numericId} = %s{numericId}"
(*** include-output ***)

(**
### 'Tagged' Identifiers

Ananoid contains one other primitive feature: tagged identifiers. This feature
attempts to strike a balance between performance and safety. Effectively,
instead of generating strings, the functions in the
`cref:T:pblasucci.Ananoid.Core.Tagged` module (ab)use F#'s [units of measure][3]
functionality to generate string that have been "tagged" with a special meaure,
`nanoid`. This causes them to be typed-checked separately from ordinary string.
However, as they are erased at run-time, they pose none of the overhead of full
types (like `cref:T:pblasucci.Ananoid.NanoId`). Usage can be as simple as:
*)
open pblasucci.Ananoid.Core.Tagged

let taggedDefaultId = nanoId' ()
let taggedNumericId = nanoIdOf' "0123456789" 12
printfn $"%s{nameof taggedDefaultId} = %A{taggedDefaultId}"
printfn $"%s{nameof taggedNumericId} = %A{taggedNumericId}"
(*** include-fsi-merged-output ***)

(**
Ananoid further provides the `tag` function for converting simple strings into
tagged strings. For converting in the opposite direction (from a tagged string
into a simple string), simply use the built-in `string` function. The follow
example helps to demonstrate the usage of `tag` and `string`:
*)

let simpleId = nanoId ()
let taggedId = nanoid.tag simpleId

printfn $"%s{nameof simpleId} = %s{simpleId}"
printfn $"%s{nameof taggedId} = %s{string taggedId}"
(*** include-fsi-merged-output ***)

(**
### Next steps

// TODO ???

[1]: https://www.nuget.org/packages/pblasucci.ananoid
[2]: https://github.com/pblasucci/ananoid/blob/c7b6f7a5e38a38f651af267107ab18b1d00c050d/ananoid/Core.fs
[3]: https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/units-of-measure
*)
