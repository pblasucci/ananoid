---
title: Work with Tagged NanoId strings
category: Primitives
categoryindex: 3
index: 1
---

How-To: Work with Tagged NanoId strings
===

Ananoid can serve most uses cases via the `cref:T:pblasucci.Ananoid.NanoId`
type and its associates (`cref:T:pblasucci.Ananoid.NanoIdOptions`,
`cref:T:pblasucci.Ananoid.NanoIdParser`, et cetera). However, sometimes this is
not desired (or at least, not _optimal_). For times when the a struct or class
is just too much, Ananoid also provides its core functionality --
cryptographically-secure randomly-generated identifiers -- as functions which
take simple inputs and just produce strings ([as seen here][1]). And, for F#
consumers, there is one more option: tagged identifiers. This feature attempts
to strike a balance between the performance of raw strings and safety of more
robust types.

> ---
> ##Attention!!!
>
> __This feature is only relevant to F# consumers... sorry ¯\_(ツ)_/¯ .__
>
> ---

Effectively, instead of generating strings, the functions in the
`cref:T:pblasucci.Ananoid.Core.Tagged` module (ab)use F#'s [units of measure][2]
functionality to generate strings that have been "tagged" with a special
measure, `cref:T:pblasucci.Ananoid.Core.Tagged.nanoid@measure`. This causes
them to be typed-checked separately from ordinary string. However, as they are
erased at run-time, they pose none of the overhead of full types (like
`cref:T:pblasucci.Ananoid.NanoId`). Usage can be as simple as:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
open pblasucci.Ananoid.Core.Tagged

let taggedDefaultId = nanoId' ()
let taggedNumericId = nanoIdOf' "0123456789" 12
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi
> open pblasucci.Ananoid.Core.Taggd;;

> let taggeedDefaultId = nanoId' ();;
    taggedDefaultId: string<nanoid> = "SG0vBABewIRQxruwugpTD"

> let taggedNumericId = nanoIdOf' "0123456789" 12;;
    taggedNumericIdL string<nanoid> = "387443539896"
```
</div>

Ananoid further provides the `tag` function for converting simple strings into
tagged strings. For converting in the opposite direction (from a tagged string
into a simple string), simply use the built-in `string` function. The follow
example helps to demonstrate the usage of `tag` and `string`:

<div class="lang-bar">
<details open class="lang-block">
<summary>F#</summary>

```fsharp
let simpleId = nanoId ()
let taggedId = nanoid.tag simpleId

let simpleId' = string taggedId
printfn $"%s{nameof simpleId} = %s{nameof simpleId'}? %b{simpleId = simpleId'}"
```
</details>

<details open class="lang-block console">
<summary>OUT</summary>

```sh
> dotnet fsi ~/scratches/taggednanoid.fsx

simpleId = simpleId'? true
```
</details>
</div>

### Related Reading

+ [Performance: Select Highlights][3]
+ [How-To: Install the Core Module from Source][4]

### Copyright
The library is available under the Mozilla Public License, Version 2.0.
For more information see the project's [License][0] file.


[0]: https://github.com/pblasucci/ananoid/blob/main/LICENSE.txt
[1]: /guides/primitives/nanoidstring.html
[2]: https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/units-of-measure
[3]: /explanations/performance/highlights.html
[4]: /guides/primitives/installcore.html
