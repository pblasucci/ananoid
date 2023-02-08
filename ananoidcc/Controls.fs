namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Label =
  open Avalonia.Controls
  open Avalonia.FuncUI.Builder
  open Avalonia.FuncUI.Types

  let create (attrs : IAttr<_> list) = ViewBuilder.Create<Label>(attrs)
