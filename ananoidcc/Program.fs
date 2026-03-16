(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open System
open System.Runtime.InteropServices
open System.Threading.Tasks
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Markup.Xaml
open Avalonia.Threading


type App() =
  inherit Application()

  [<Literal>]
  static let CO_E_NOTINITIALIZED = 0x800401F0

  [<Literal>]
  static let WrongLifetime = "Incorrect application lifetime detected."

  override me.Initialize() = AvaloniaXamlLoader.Load(me)

  override me.OnFrameworkInitializationCompleted() =
    match me.ApplicationLifetime with
    | :? IClassicDesktopStyleApplicationLifetime as desktop ->
      let host = MainHost()
      desktop.MainWindow <- host

      Dispatcher.UIThread.UnhandledException.Add(fun e ->
          match e.Exception with
          | :? COMException as x when x.ErrorCode = CO_E_NOTINITIALIZED ->
            Task.Run(fun () ->
              match TopLevel.GetTopLevel(desktop.MainWindow) with
              | NonNull topLevel ->
                match topLevel.Clipboard with
                | NonNull clipboard -> clipboard.FlushAsync()
                | Null -> Task.CompletedTask
              | Null -> Task.CompletedTask
            ) |> ignore
            e.Handled <- true // Only if safe to continue
          | _ -> ()
      )

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
