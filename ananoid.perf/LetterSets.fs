(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
module pblasucci.Ananoid.Perf.LetterSets

[<Literal>]
let Alphanumeric =
  "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

[<Literal>]
let HexadecimalLowercase = "0123456789abcdef"

[<Literal>]
let HexadecimalUppercase = "0123456789ABCDEF"

[<Literal>]
let Lowercase = "abcdefghijklmnopqrstuvwxyz"

[<Literal>]
let NoLookalikes = "346789ABCDEFGHJKLMNPQRTUVWXYabcdefghijkmnpqrtwxyz"

[<Literal>]
let NoLookalikesSafe = "6789BCDFGHJKLMNPQRTWbcdfghjkmnpqrtwz"

[<Literal>]
let Numbers = "0123456789"

[<Literal>]
let Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

[<Literal>]
let UrlSafe = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz-"
