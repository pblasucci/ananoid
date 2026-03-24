(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open System
open System.Globalization
open System.Threading.Tasks
open Avalonia
open Avalonia.Controls
open Avalonia.Input
open Avalonia.Input.Platform
open Avalonia.Media
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Hosts
open Avalonia.Threading
open FsToolkit.ErrorHandling
open IcedTasks
open Core
open Support
open KnownAlphabets

open type WindowStartupLocation
open type TextWrapping


module Main =
  open Avalonia.Input.TextInput

  [<RequireQualifiedAccess>]
  module Svg =
    let textChangeCase =
      Geometry.Parse(
        "M16.5106 3.00008C16.8242 3.00452 17.1019 3.20362 17.2068 3.49919L22.7068 18.9992C22.8453 19.3896 22.6411 19.8183 22.2508 19.9568C21.8604 20.0953 21.4317 19.8912 21.2931 19.5008L19.8735 15.5L12.7558 15.5L11.1994 19.5207C11.0499 19.907 10.6155 20.099 10.2292 19.9494C9.84293 19.7999 9.651 19.3655 9.80053 18.9793L15.8005 3.47925C15.9137 3.18678 16.197 2.99563 16.5106 3.00008ZM16.4694 5.90659L13.3364 14L19.3412 14L16.4694 5.90659Z M5.50005 10.5014L5.78725 10.5113C7.74783 10.6088 8.91466 11.7373 8.9955 13.5555L9 13.7604V19.2604C9 19.6401 8.71785 19.9539 8.35177 20.0035L8.25 20.0104C7.8703 20.0104 7.55651 19.7282 7.50685 19.3621L7.5 19.2604L7.49865 19.156C6.51108 19.7214 5.59755 20.0104 4.75 20.0104C2.91158 20.0104 1.5 18.7159 1.5 16.7604C1.5 15.036 2.68843 13.7549 4.6597 13.5158C5.592 13.4028 6.53953 13.4736 7.49853 13.7255C7.4887 12.6184 6.94255 12.0706 5.71275 12.0095C4.75051 11.9616 4.07429 12.0967 3.67825 12.3744C3.33911 12.6123 2.87139 12.5301 2.63357 12.191C2.39576 11.8518 2.47789 11.3841 2.81703 11.1463C3.47484 10.685 4.37072 10.4807 5.50005 10.5014ZM7.499 15.3241L7.19613 15.2338C6.38875 15.0117 5.60483 14.9486 4.8403 15.0413C3.60791 15.1908 3 15.8461 3 16.7967C3 17.8934 3.7124 18.5467 4.75 18.5467C5.43032 18.5467 6.26848 18.2324 7.25129 17.583L7.499 17.4141V15.3241Z"
      )

    let documentCopy =
      Geometry.Parse(
        "M11.5004316,10.3756744 L11.5,33.75 C11.5,37.4779221 14.5220779,40.5 18.25,40.5 L33.6238675,40.5005808 C32.9567459,41.9745319 31.4731284,43 29.75,43 L18.25,43 C13.1413661,43 9,38.8586339 9,33.75 L9,14.25 C9,12.5264771 10.0259377,11.0425748 11.5004316,10.3756744 Z M25.7573593,5 C26.8845294,5 27.9655335,5.44776657 28.7625631,6.24479618 L37.7552038,15.2374369 C38.5522334,16.0344665 39,17.1154706 39,18.2426407 L39,33.75 C39,36.0972102 37.0972102,38 34.75,38 L18.25,38 C15.9027898,38 14,36.0972102 14,33.75 L14,9.25 C14,6.90278981 15.9027898,5 18.25,5 L25.7573593,5 Z M25,7.5 L18.25,7.5 C17.2835017,7.5 16.5,8.28350169 16.5,9.25 L16.5,33.75 C16.5,34.7164983 17.2835017,35.5 18.25,35.5 L34.75,35.5 C35.7164983,35.5 36.5,34.7164983 36.5,33.75 L36.5,19 L29.25,19 C26.9718254,19 25.1123133,17.207493 25.0049009,14.9559163 L25,14.75 L25,7.5 Z M35.482,16.5 L27.5,8.518 L27.5,14.75 C27.5,15.6681734 28.2071103,16.4211923 29.1064728,16.4941988 L29.25,16.5 L35.482,16.5 Z"
      )

    let library =
      Geometry.Parse(
        "M3.9997,3 L5.9897,3 C7.04351818,3 7.9078157,3.81639669 7.98421089,4.85080841 L7.9897,5 L7.9897,23 C7.9897,24.0538182 7.17330331,24.9181157 6.13889159,24.9945109 L5.9897,25 L3.9997,25 C2.94588182,25 2.0815843,24.1836033 2.00518911,23.1491916 L1.9997,23 L1.9997,5 C1.9997,3.94618182 2.81609669,3.0818843 3.85050841,3.00548911 L3.9997,3 L5.9897,3 L3.9997,3 Z M10.9947,3 L12.9897,3 C14.0435182,3 14.9078157,3.81639669 14.9842109,4.85080841 L14.9897,5 L14.9897,23 C14.9897,24.0538182 14.1733033,24.9181157 13.1388916,24.9945109 L12.9897,25 L10.9947,25 C9.93992727,25 9.07649752,24.1836033 9.00018319,23.1491916 L8.9947,23 L8.9947,5 C8.9947,3.94618182 9.81018554,3.0818843 10.8453842,3.00548911 L10.9947,3 L12.9897,3 L10.9947,3 Z M20.1303,5.0264 C20.9735941,5.0264 21.7460232,5.56408858 22.0232306,6.38601897 L22.0693,6.5434 L25.9293,22.0264 C26.1851182,23.0487182 25.6026719,24.0847037 24.6168316,24.4098625 L24.4733,24.4514 L22.5103,24.9404 C22.3483,24.9804 22.1853,25.0004 22.0253,25.0004 C21.1810647,25.0004 20.4094661,24.4618256 20.1323596,23.6406247 L20.0863,23.4834 L16.2253,8.0004 C15.9704364,6.97617273 16.552926,5.94101157 17.5387684,5.61680139 L17.6823,5.5754 L19.6453,5.0864 C19.8073,5.0464 19.9703,5.0264 20.1303,5.0264 Z M5.9897,4.5 L3.9997,4.5 C3.75525556,4.5 3.55031728,4.67777778 3.50779328,4.91042524 L3.4997,5 L3.4997,23 C3.4997,23.2444444 3.67747778,23.4493827 3.91012524,23.4919067 L3.9997,23.5 L5.9897,23.5 C6.23503333,23.5 6.43928025,23.3222222 6.48163964,23.0895748 L6.4897,23 L6.4897,5 C6.4897,4.75555556 6.31271235,4.55061728 6.07953813,4.50809328 L5.9897,4.5 Z M12.9897,4.5 L10.9947,4.5 C10.7493667,4.5 10.5451198,4.67777778 10.5027604,4.91042524 L10.4947,5 L10.4947,23 C10.4947,23.2444444 10.6716877,23.4493827 10.9048619,23.4919067 L10.9947,23.5 L12.9897,23.5 C13.2350333,23.5 13.4392802,23.3222222 13.4816396,23.0895748 L13.4897,23 L13.4897,5 C13.4897,4.75555556 13.3127123,4.55061728 13.0795381,4.50809328 L12.9897,4.5 Z M20.1303,6.5264 L20.0688,6.53015 L20.0688,6.53015 L20.0073,6.5414 L18.0453,7.0304 C17.8070778,7.08995556 17.6518185,7.31148642 17.6673137,7.54750288 L17.6813,7.6364 L21.5413,23.1204 C21.6063,23.3804 21.8383,23.5004 22.0253,23.5004 L22.086675,23.496525 L22.086675,23.496525 L22.1473,23.4844 L24.1103,22.9954 C24.3485222,22.9358444 24.5037815,22.7151037 24.4882863,22.4785605 L24.4743,22.3894 L20.6133,6.9054 C20.5483,6.6444 20.3173,6.5264 20.1303,6.5264 Z"
      )

    let textChange =
      Geometry.Parse(
        "M2.75 5H21.25C21.6642 5 22 5.33579 22 5.75C22 6.1297 21.7178 6.44349 21.3518 6.49315L21.25 6.5H2.75C2.33579 6.5 2 6.16421 2 5.75C2 5.3703 2.28215 5.05651 2.64823 5.00685L2.75 5Z M21.25 9H2.75L2.64823 9.00685C2.28215 9.05651 2 9.3703 2 9.75C2 10.1642 2.33579 10.5 2.75 10.5H21.25L21.3518 10.4932C21.7178 10.4435 22 10.1297 22 9.75C22 9.33579 21.6642 9 21.25 9Z M2.75 14.5H11.7322C12.0194 13.949 12.3832 13.4443 12.8096 13H2.75L2.64823 13.0068C2.28215 13.0565 2 13.3703 2 13.75C2 14.1642 2.33579 14.5 2.75 14.5Z M2.75 17H11.0189C11.0064 17.165 11 17.3318 11 17.5C11 17.8401 11.0261 18.174 11.0764 18.5H2.75C2.33579 18.5 2 18.1642 2 17.75C2 17.3703 2.28215 17.0565 2.64823 17.0068L2.75 17Z M12 17.5C12 20.5376 14.4624 23 17.5 23C20.5376 23 23 20.5376 23 17.5C23 14.4624 20.5376 12 17.5 12C14.4624 12 12 14.4624 12 17.5ZM18.35 20.3536L18.2808 20.4114C18.0859 20.5464 17.8165 20.5271 17.6429 20.3536L17.5851 20.2843C17.4501 20.0894 17.4694 19.82 17.6429 19.6464L19.289 18L14.5 18L14.4101 17.9919C14.1769 17.9496 14 17.7455 14 17.5L14.0081 17.4101C14.0504 17.1769 14.2545 17 14.5 17L19.289 17L17.6429 15.3536L17.5851 15.2843C17.4501 15.0894 17.4694 14.82 17.6429 14.6464C17.8382 14.4512 18.1548 14.4512 18.35 14.6464L20.8657 17.1629L20.9166 17.2289L20.9505 17.2902L20.9795 17.3704L20.9945 17.4557L20.9921 17.5659L20.9732 17.6512L20.9504 17.7099L20.9063 17.7866L20.8656 17.8372L18.35 20.3536Z"
      )

  let alphabets =
    Map [
      (nameof Alphanumeric, Alphanumeric.Letters)
      (nameof HexadecimalLowercase, HexadecimalLowercase.Letters)
      (nameof HexadecimalUppercase, HexadecimalUppercase.Letters)
      (nameof Lowercase, Lowercase.Letters)
      (nameof NoLookalikes, NoLookalikes.Letters)
      (nameof NoLookalikesSafe, NoLookalikesSafe.Letters)
      (nameof Numbers, Numbers.Letters)
      (nameof Uppercase, Uppercase.Letters)
      (nameof UrlSafe, UrlSafe.Letters)
    ]

  let copyTo clipboard value =
    clipboard
    |> Option.iter (fun (board : IClipboard) ->
      Dispatcher.UIThread.InvokeAsync(fun () ->
        taskUnit {
          do! board.SetTextAsync(value)
          do! board.FlushAsync()
        }
      )
      |> ignore
    )

  let alphabetMenuItems (state : IWritable<_>) =
    MenuItem.create [
      MenuItem.header (PathIcon.create [ PathIcon.data Svg.textChangeCase ])
      MenuItem.tip "Pre-defined alphabets"
      MenuItem.viewItems [
        for KeyValue(name, letters) in alphabets do
          MenuItem.create [ MenuItem.header name; MenuItem.onClick (fun _ -> state.Set letters) ]
      ]
    ]

  let alphabetView alphabet =
    Component.create (
      nameof alphabet,
      fun context ->
        let state = context.usePassed alphabet
        let input = $"{nameof alphabet}_textbox"
        let maxLength = 255
        let computeSize (Length soFar) = $"(%i{soFar}/%i{maxLength})"

        Grid.create [
          Grid.classes [ nameof alphabet ]
          Grid.columnDefinitions "Auto, *, Auto"
          Grid.rowDefinitions "Auto, *"
          Grid.children [
            Label.create [
              Label.row 0
              Label.column 0
              Label.content "Alphabet"
              Label.tip "The set of 'letters' from which an identifier is made"
            ]
            TextBlock.create [
              TextBlock.row 0
              TextBlock.column 1
              TextBlock.text (computeSize state.Current)
            ]
            Menu.create [ Menu.row 0; Menu.column 2; Menu.viewItems [ alphabetMenuItems state ] ]
            TextBox.create [
              TextBox.name input
              TextBox.row 1
              TextBox.column 0
              TextBox.columnSpan 3
              TextBox.maxLength maxLength
              TextBox.acceptsReturn true
              TextBox.textWrapping Wrap
              TextBox.text state.Current
              TextBox.onTextChanged state.Set
            ]
          ]
        ]
    )

  let lengthView (length : IWritable<decimal>) =
    Component.create (
      nameof length,
      fun context ->
        let state = context.usePassed length

        let minLength, maxLength = (1.0M, 129.0M)

        Grid.create [
          Grid.classes [ nameof length ]
          Grid.columnDefinitions "Auto, *"
          Grid.rowDefinitions "Auto, Auto"
          Grid.children [
            Label.create [
              Label.row 0
              Label.column 0
              Label.content "Length"
              Label.tip "The length of a generated identifier, in number of characters"
            ]
            NumericUpDown.create [
              NumericUpDown.row 1
              NumericUpDown.column 0
              NumericUpDown.showButtonSpinner false
              NumericUpDown.minimum minLength
              NumericUpDown.maximum maxLength
              NumericUpDown.increment 1
              NumericUpDown.clipValueToMinMax true
              NumericUpDown.contentType TextInputContentType.Digits
              NumericUpDown.parsingNumberStyle NumberStyles.Integer
              NumericUpDown.value (truncate state.Current)
              NumericUpDown.onValueChanged (fun change ->
                if change.HasValue then
                  state.Set(truncate change.Value)
              )
            ]
            Slider.create [
              Slider.row 0
              Slider.rowSpan 2
              Slider.column 1
              Slider.tickFrequency 1.0
              Slider.smallChange 1.0
              Slider.largeChange 10.0
              Slider.isSnapToTickEnabled true
              Slider.minimum (double minLength)
              Slider.maximum (double maxLength)
              Slider.value (double state.Current)
              Slider.onValueChanged (decimal >> state.Set)
            ]
          ]
        ]
    )

  let frequencyView frequency =
    Component.create (
      nameof frequency,
      fun context ->
        let state = context.usePassed frequency

        let isPerHour =
          function
          | PerHour _ -> true
          | PerSecond _ -> false

        let updateValue (change : Nullable<decimal>) =
          if change.HasValue then
            state.Set(
              match state.Current with
              | PerHour _ -> PerHour(float change.Value)
              | PerSecond _ -> PerSecond(float change.Value)
            )

        let updateUnits units = state.Set(units state.Current.Amount)

        let group = "units"

        Grid.create [
          Grid.classes [ nameof frequency ]
          Grid.columnDefinitions "*, Auto, Auto, Auto"
          Grid.rowDefinitions "Auto, Auto"
          Grid.children [
            Label.create [
              Label.row 0
              Label.column 0
              Label.columnSpan 4
              Label.content "Frequency"
              Label.tip "The rate at which new identifiers are generated"
            ]
            NumericUpDown.create [
              NumericUpDown.row 1
              NumericUpDown.column 0
              NumericUpDown.clipValueToMinMax true
              NumericUpDown.increment 10
              NumericUpDown.maximum Int32.MaxValue
              NumericUpDown.minimum 1
              NumericUpDown.parsingNumberStyle NumberStyles.Integer
              NumericUpDown.value (decimal state.Current.Amount)
              NumericUpDown.onValueChanged updateValue
            ]
            TextBlock.create [ TextBlock.row 1; TextBlock.column 1; TextBlock.text "Nano IDs" ]
            RadioButton.create [
              RadioButton.row 1
              RadioButton.column 2
              RadioButton.groupName group
              RadioButton.content "per hour"
              RadioButton.isChecked (isPerHour state.Current)
              RadioButton.onChecked (fun _ -> updateUnits PerHour)
            ]
            RadioButton.create [
              RadioButton.row 1
              RadioButton.column 3
              RadioButton.groupName group
              RadioButton.content "per second"
              RadioButton.isChecked (not (isPerHour state.Current))
              RadioButton.onChecked (fun _ -> updateUnits PerSecond)
            ]
          ]
        ]
    )

  let generatorView ((alphabet, length) as generate) =
    Component.create (
      nameof generate,
      fun context ->
        let clipboard = (|Clipboard|_|) context

        let alphabet = context.usePassedRead alphabet
        let length = context.usePassedRead length
        let nanoId = context.useState ""

        Grid.create [
          Grid.columnDefinitions "Auto, *, Auto"
          Grid.rowDefinitions "Auto, *"
          Grid.classes [ nameof generate ]
          Grid.children [
            Label.create [
              Label.row 0
              Label.column 0
              Label.columnSpan 3
              Label.content "Nano ID"
              Label.tip "An identifier, of the selected length, generated from the given alphabet"
            ]
            Button.create [
              Grid.column 0
              Grid.row 1
              Button.tip "Generate identifier"
              Button.content (PathIcon.create [ PathIcon.data Svg.textChange ])
              Button.onClick (fun _ ->
                let value = nanoIdOf alphabet.Current (int length.Current)
                nanoId.Set(value)
              )
            ]
            TextBox.create [
              Grid.column 1
              Grid.row 1
              TextBox.isReadOnly true
              TextBox.text nanoId.Current
            ]
            Button.create [
              Grid.column 2
              Grid.row 1
              Button.tip "Copy to clipboard"
              Button.content (PathIcon.create [ PathIcon.data Svg.documentCopy ])
              Button.isEnabled (Option.isSome clipboard && 0 < nanoId.Current.Length)
              Button.onClick (fun _ -> nanoId.Current |> copyTo clipboard)
            ]
          ]
        ]
    )

  let view () =
    Component(fun context ->
      let alphabet = context.useState UrlSafe.Letters
      let length = context.useState 21.0M
      let frequency = context.useState (PerHour 1000.0)
      let results = context.useState nan

      let recompute () =
        let length' = length.Current |> truncate |> int
        results.Set(timeToCollision alphabet.Current length' frequency.Current)

      let triggers = [
        EffectTrigger.AfterChange alphabet
        EffectTrigger.AfterChange length
        EffectTrigger.AfterChange frequency
      ]

      context.useEffect (handler = recompute, triggers = triggers)

      StackPanel.create [
        StackPanel.children [
          alphabetView alphabet
          lengthView length
          frequencyView frequency

          (* results *)
          Border.create [
            Border.classes [ nameof results ]
            Border.child (TextBlock.create [ TextBlock.text (formatDuration results.Current) ])
          ]

          generatorView (alphabet, length)
        ]
      ]
    )


type MainHost() as me =
  inherit
    HostWindow(
      CanResize = false,
      Content = Main.view (),
      Height = 366.0,
      Name = nameof Main,
      Title = ":: A Nano ID Collision Calculator ::",
      Width = 480.0,
      WindowStartupLocation = CenterScreen
    )

  do me.AttachDevTools()
