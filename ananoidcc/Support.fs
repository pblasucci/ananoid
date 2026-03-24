(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open FsToolkit.ErrorHandling

module Label =
  let create (attrs : IAttr<_> list) = ViewBuilder.Create<Label>(attrs)

[<AutoOpen>]
module TopLevel =
    let inline (|Clipboard|_|) (context : IComponentContext) =
      option {
        let! visual = TopLevel.GetTopLevel(context.control)
        return! Option.ofObj visual.Clipboard
      }

(* _____________ ⮝⮝⮝ GUI DSL / Calculations ⮟⮟⮟ _____________ *)

namespace pblasucci.Ananoid

open System

module Support =
  type Frequency =
    | PerHour of amount : float
    | PerSecond of amount : float

    static let secondsPerHour = 3600.0

    member me.Amount =
      match me with
      | PerHour amount
      | PerSecond amount -> amount

    member me.TotalSeconds =
      match me with
      | PerSecond amount -> amount
      | PerHour amount -> amount / secondsPerHour

  let inline (|Length|) value =
    if String.IsNullOrWhiteSpace(value) then 0 else value.Trim().Length

  let timeToCollision (Length alphabetLength) (nanoIdLength : int) (frequency : Frequency) =
    if 0 < alphabetLength && alphabetLength < 256 && 0 < nanoIdLength then
      // Target probability
      let P = 0.01

      // Number of bits is a single Nano ID, based on:
      // 1. the number of letters in the alphabet
      // 2. the length of the generated Nano ID
      let entropy = float nanoIdLength * (log (float alphabetLength) / log 2.0)

      // Estimated number of generated identifiers until collision at target probability
      let numberOfIds = sqrt (2.0 * Math.Pow(2.0, entropy) * (log (1.0 / (1.0 - P))))

      // Theoretical time needed to generate until collision
      floor (numberOfIds / frequency.TotalSeconds)
    else
      // cannot compute time-to-collision because inputs are invalid
      nan

  type DurationFormat = {
    Factor : float
    Label : string
    Pluralize : bool
  } with
    member me.Render(value) =
      let suffix = if me.Pluralize && 1 < value then $"%s{me.Label}s" else me.Label
      if value < 1 then $"Less than 1 %s{suffix}" else $"~{value} %s{suffix}"

    static member Of(factor, label, ?pluralize) = {
      Factor = factor
      Label = label
      Pluralize = defaultArg pluralize true
    }

  let formatDuration seconds =
    let infinity = "\u221E"

    let timeNames = [
      DurationFormat.Of(60.0, "second")
      DurationFormat.Of(60.0, "minute")
      DurationFormat.Of(24.0, "hour")
      DurationFormat.Of(365.26, "day")
      DurationFormat.Of(1000, "year")
      DurationFormat.Of(1000, "thousand years", pluralize = false)
      DurationFormat.Of(1000, "million years", pluralize = false)
      DurationFormat.Of(1000, "billion years", pluralize = false)
      DurationFormat.Of(1000, "trillion years", pluralize = false)
      DurationFormat.Of(1000, "Over 1 quadrillion years", pluralize = false)
    ]

    let rec loop current items =
      match items with
      | [] -> infinity
      | h :: t ->
        let next = current / h.Factor
        if List.isEmpty t then h.Label
        elif next < 1.0 then h.Render(int (round current))
        else loop next t

    if Double.IsNaN seconds then infinity else loop seconds timeNames
