(**
---
title: Customization
category: F# Guides
categoryindex: 1
index: 2
---
*)

(*** hide ***)
#i """nuget: /home/pblasucci/Source/ananoid/ananoid/bin/Release"""

#r "nuget: pblasucci.ananoid"
open pblasucci.Ananoid

[<RequireQualifiedAccess>]
module Result =
  let iterError action result =
    match result with
    | Error e -> action e
    | Ok _ -> ((* noop *))

(**
Customization
===

The default settings for creating a `NanoId` (21 character taken from a mix of
letters, numbers, hyphen, and underscore) reflect a reasonable balance of
entropy and performance. Further the additional alphabets shipped with Ananoid
cover a wide range of common wants and needs. However, it is possible to go
further. Consumers can provide their _own_ alphabets.

### Learning about alphabets

The only requirement for any alphabet is that it implement the `IAlphabet`
interface. This simple contract provides everything needed to generate new
 `NanoId` instances. Its contract specifies two members:

+ `Letters : string with get`
+ `WillPermit : string -> bool`

The `Letters` property defines the universe of characters, which will be
pulled from at random, as part of creating a `NanoId`. The `WillPermit` method,
meanwhile, tests if a given value could be constituted from the alphabet. For
example, an alphabet consisting only of numbers (0 through 9 inclusive) might
be implemented like so:
*)

type Numbers() =
  interface IAlphabet with
    member _.Letters = "0123456789"
    member _.WillPermit(value) =
      value |> String.forall (fun c -> uint (c - '0') <= uint ('9' - '0'))

(**
However, an alphabet is more than just a string. There are certain _invariants_
which any alphabet must uphold. They are:

+ The set of letter MUST contain at least one (1) letter.
+ The set of letter MAY NOT contains more than 255 letters.
+ The alphabet MUST be 'coherent' (it can validate its own letter set).

These are not the most challenging invariants. Nevertheless, the Ananoid
library checks them in certain key places. If an alphabet fails to pass
validation, an `cref:T:pblasucci.Ananoid.AlphabetError`, containing exact
details, will be produced. The validation logic is centralized and you can call
it yourself, by using the `Validate` function on the
`cref:T:pblasucci.Ananoid.Alphabet` type (this, by the way, is the type which
defines all the pre-defined alphabets which ship with Ananoid), as shown below:
*)

Numbers()
|> Alphabet.Validate
|> Result.iterError (fun e -> eprintfn $"Validation failed: %s{e.Message}")

(**
### From alphabets to factories

Ananoid requires a valid `IAlphabet` in several places. Notably, it is an
input for:

+ Creating a new `cref:T:pblasucci.Ananoid.NanoIdParser` instance.
+ Creating a new `cref:T:pblasucci.Ananoid.NanoIdOptions` instance.

In other case, the given alphabet will be validate internally. That is, the
caller is _not_ responsible for invoking `Alphabet.Validate`. Regardless of the
type being instantiated, if alphabet validation fails, an `AlphabetError` will
be returned (just as we have previously seen).

However, this is one other place where an alphabet is useful. A caller may use
one to create a 'nano id factory'. This factory is just a function -- one which
has already validated and captured the given alphabet. Callers can then use
this factory (function) to create new `NanoId` instances of varying lengths, as
in the following example:
*)

let factory =
  Numbers().ToNanoIdFactory()
  |> Result.defaultWith (fun e -> failwith $"Validation failed: %s{e.Message}")

let sixtyFourDigits = factory 64
printfn $"%s{nameof sixtyFourDigits} = %s{string sixtyFourDigits}"

// NOTE the generated factory function returns an empty `NanoId`
// ...  for any size input value less than 1
let noDigits = factory -3
printfn $"%s{nameof noDigits} = NanoId.Empty? %b{NanoId.IsEmpty noDigits}"
(*** include-output ***)

(**
### Next steps

// TODO ???
*)
