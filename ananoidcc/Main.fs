namespace MulberryLabs.Ananoid

open Microsoft.FSharp.Reflection
open System.Globalization
open Avalonia
open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Hosts

open Utilities

open type WindowStartupLocation
open type TextWrapping


module Main =
  //TODO tooltips?

  let alphabetMenuItems (state : IWritable<_>) =
    MenuItem.create
      [
        MenuItem.header "Pre-defined..."
        MenuItem.viewItems
          [
            for info in FSharpType.GetUnionCases typeof<Alphabet> do
              MenuItem.create
                [
                  MenuItem.header info.Name
                  MenuItem.onClick (fun _ ->
                    FSharpValue.MakeUnion(info, null)
                    |> unbox<Alphabet>
                    |> string
                    |> state.Set
                  )
                ]
          ]
      ]

  let alphabetView alphabet =
    Component.create (
      nameof alphabet,
      fun context ->
        let state = context.usePassed alphabet
        let input = $"{nameof alphabet}_textbox"
        Grid.create
          [
            Grid.classes [ nameof alphabet ]
            Grid.columnDefinitions "*, Auto"
            Grid.rowDefinitions "Auto, *"
            Grid.children
              [
                Label.create
                  [
                    Label.row 0
                    Label.column 0
                    Label.content "Alphabet:"
                  ]
                Menu.create
                  [
                    Menu.row 0
                    Menu.column 1
                    Menu.viewItems [ alphabetMenuItems state ]
                  ]
                TextBox.create
                  [
                    TextBox.name input
                    TextBox.row 1
                    TextBox.column 0
                    TextBox.columnSpan 2
                    TextBox.acceptsReturn true
                    TextBox.textWrapping Wrap
                    TextBox.text state.Current
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

        Grid.create
          [
            Grid.classes [ nameof length ]
            Grid.columnDefinitions "Auto, *"
            Grid.rowDefinitions "Auto, Auto"
            Grid.children
              [
                Label.create
                  [
                    Label.row 0
                    Label.column 0
                    Label.content "Length:"
                  ]
                NumericUpDown.create
                  [
                    NumericUpDown.row 1
                    NumericUpDown.column 0
                    NumericUpDown.showButtonSpinner false
                    NumericUpDown.minimum minLength
                    NumericUpDown.maximum maxLength
                    NumericUpDown.value state.Current
                    NumericUpDown.onValueChanged state.Set
                  ]
                Slider.create
                  [
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

        Grid.create
          [
            Grid.classes [ nameof frequency ]
            Grid.columnDefinitions "*, Auto, Auto, Auto"
            Grid.rowDefinitions "Auto, Auto"
            Grid.children
              [
                Label.create
                  [
                    Label.row 0
                    Label.column 0
                    Label.columnSpan 4
                    Label.content "Frequency:"
                  ]
                NumericUpDown.create
                  [
                    NumericUpDown.row 1
                    NumericUpDown.column 0
                    NumericUpDown.parsingNumberStyle NumberStyles.Integer
                    NumericUpDown.value state.Current.Amount
                    NumericUpDown.onValueChanged updateValue
                  ]
                TextBlock.create
                  [
                    TextBlock.row 1
                    TextBlock.column 1
                    TextBlock.text "Nano IDs"
                  ]
                RadioButton.create
                  [
                    RadioButton.row 1
                    RadioButton.column 2
                    RadioButton.groupName group
                    RadioButton.content "per hour"
                    RadioButton.isChecked (isPerHour state.Current)
                    RadioButton.onChecked (fun _ -> updateUnits PerHour)
                  ]
                RadioButton.create
                  [
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

  let view () =
    Component(fun context ->
      let alphabet = context.useState (string UrlSafe)
      let length = context.useState 21.0
      let frequency = context.useState (PerHour 1000.0)
      let results = context.useState nan

      let recompute () =
        let length' = length.Current |> truncate |> int
        results.Set(timeToCollision alphabet.Current length' frequency.Current)

      let triggers =
        [
          EffectTrigger.AfterChange alphabet
          EffectTrigger.AfterChange length
          EffectTrigger.AfterChange frequency
        ]

      context.useEffect(handler = recompute, triggers = triggers)

      StackPanel.create
        [
          StackPanel.children
            [
              alphabetView alphabet
              lengthView length
              frequencyView frequency

              (* results *)
              Border.create [
                Border.classes [ nameof results ]
                Border.child (
                  TextBlock.create
                    [
                      TextBlock.text (formatDuration results.Current)
                    ]
                )
              ]
            ]
        ]
    )


type MainHost() as me =
  inherit HostWindow
    (

      CanResize = false,
      Content = Main.view (),
      Height = 320.0,
      Name = nameof Main,
      Title = ":: A Nano ID Collision Calculator ::",
      Width = 480.0,
      WindowStartupLocation = CenterScreen
    )
#if DEBUG
  do me.AttachDevTools()
#endif
