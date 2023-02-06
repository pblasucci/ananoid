module rec MulberryLabs.Ananoid.Tests.Scratch

open System
open FsCheck
open FsCheck.Xunit

(* system under test *)
open MulberryLabs.Ananoid

[<Property(MaxTest = 1, Skip = "For messing around only!")>]
let ``Do It!`` () = false |> Prop.label $"\n{nameof ``Do It!``}"
