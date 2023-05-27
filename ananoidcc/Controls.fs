(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Label =
  open Avalonia.Controls
  open Avalonia.FuncUI.Builder
  open Avalonia.FuncUI.Types

  let create (attrs : IAttr<_> list) = ViewBuilder.Create<Label>(attrs)
