(*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*)
namespace pblasucci.Ananoid

open Microsoft.FSharp.Reflection
open System
open System.Globalization
open Avalonia
open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Hosts

open Core
open Support

open type WindowStartupLocation
open type TextWrapping


module Main =
  let alphabets =
    Map [
      for info in FSharpType.GetUnionCases typeof<Alphabet> do
        let letters =
          FSharpValue.MakeUnion(info, null) |> unbox<Alphabet> |> string
        (info.Name, letters)
    ]

  let alphabetMenuItems (state : IWritable<_>) =
    MenuItem.create [
      MenuItem.header "Pre-defined..."
      MenuItem.viewItems [
        for KeyValue(name, letters) in alphabets do
          MenuItem.create [
            MenuItem.header name
            MenuItem.onClick (fun _ -> state.Set letters)
          ]
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
              Label.tip "The set of 'letters' from which an identifier is made."
            ]
            TextBlock.create [
              TextBlock.row 0
              TextBlock.column 1
              TextBlock.text (computeSize state.Current)
            ]
            Menu.create [
              Menu.row 0
              Menu.column 2
              Menu.viewItems [ alphabetMenuItems state ]
            ]
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

  let lengthView length =
    Component.create (
      nameof length,
      fun context ->
        let state = context.usePassed length

        let minLength, maxLength = (1.0, 129.0)

        Grid.create [
          Grid.classes [ nameof length ]
          Grid.columnDefinitions "Auto, *"
          Grid.rowDefinitions "Auto, Auto"
          Grid.children [
            Label.create [
              Label.row 0
              Label.column 0
              Label.content "Length"
              Label.tip
                "The length of a generated identifier, in number of characters."
            ]
            NumericUpDown.create [
              NumericUpDown.row 1
              NumericUpDown.column 0
              NumericUpDown.showButtonSpinner false
              NumericUpDown.minimum minLength
              NumericUpDown.maximum maxLength
              NumericUpDown.value state.Current
              NumericUpDown.onValueChanged state.Set
            ]
            Slider.create [
              Slider.row 0
              Slider.rowSpan 2
              Slider.column 1
              Slider.tickFrequency 1.0
              Slider.smallChange 1.0
              Slider.largeChange 10.0
              Slider.isSnapToTickEnabled true
              Slider.minimum minLength
              Slider.maximum maxLength
              Slider.value state.Current
              Slider.onValueChanged state.Set
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

        let updateValue value =
          state.Set(
            match state.Current with
            | PerHour _ -> PerHour value
            | PerSecond _ -> PerSecond value
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
              Label.tip "The rate at which new identifiers are generated."
            ]
            NumericUpDown.create [
              NumericUpDown.row 1
              NumericUpDown.column 0
              NumericUpDown.clipValueToMinMax true
              NumericUpDown.increment 10
              NumericUpDown.maximum Int32.MaxValue
              NumericUpDown.minimum 1
              NumericUpDown.parsingNumberStyle NumberStyles.Integer
              NumericUpDown.value state.Current.Amount
              NumericUpDown.onValueChanged updateValue
            ]
            TextBlock.create [
              TextBlock.row 1
              TextBlock.column 1
              TextBlock.text "Nano IDs"
            ]
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
    Component.create(nameof generate, fun context ->
      let alphabet = context.usePassedRead alphabet
      let length = context.usePassedRead length
      let nanoId = context.useState ""

      Grid.create [
        Grid.columnDefinitions "Auto, *"
        Grid.rowDefinitions "Auto"
        Grid.classes [ nameof generate ]
        Grid.children [
          Button.create [
            Grid.column 0
            Grid.row 0
            Button.content "Generate Identifier"
            Button.onClick (fun _ ->
              nanoId.Set(nanoIdOf alphabet.Current (int length.Current))
            )
          ]
          TextBox.create [
            Grid.column 1
            Grid.row 0
            TextBox.isReadOnly true
            TextBox.text nanoId.Current
          ]
        ]
      ]
    )

  let view () =
    Component(fun context ->
      let alphabet = context.useState (string UrlSafe)
      let length = context.useState 21.0
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
            Border.child (
              TextBlock.create [
                TextBlock.text (formatDuration results.Current)
              ]
            )
          ]

          generatorView (alphabet, length)
        ]
      ]
    )


type MainHost() =
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
