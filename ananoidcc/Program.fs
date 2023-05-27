(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Markup.Xaml


type App() =
  inherit Application()

  [<Literal>]
  static let WrongLifetime = "Incorrect application lifetime detected."

  override me.Initialize() = AvaloniaXamlLoader.Load(me)

  override me.OnFrameworkInitializationCompleted() =
    match me.ApplicationLifetime with
    | :? IClassicDesktopStyleApplicationLifetime as desktop ->
      desktop.MainWindow <- MainHost()

    | _ -> raise (InvalidProgramException WrongLifetime)

    base.OnFrameworkInitializationCompleted()


module Program =
  [<EntryPoint>]
  let main args =
    try
      AppBuilder
        .Configure<App>()
        .UsePlatformDetect()
        .UseSkia()
        .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose)
    with x ->
      eprintfn $"CRITICAL ERROR: %s{x.Message}"
      1 // ⮜⮜⮜ non-success exit code
