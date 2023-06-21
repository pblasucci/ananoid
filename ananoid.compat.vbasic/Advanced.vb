Public Module Advanced
  Sub BypassingNanoIdOptions()
    ' We can provide the alphabet now, and the size later.
    Dim factory = Alphabet.Numbers.ToNanoIdFactory()

    Dim delayed1 = factory(9)
    Dim delayed2 = factory(99)

    WriteLine("Some delayed NanoIds:")
    WriteLine($"{vbTab}...Numeric, 9: {delayed1}")
    WriteLine($"{vbTab}...Numeric, 99: {delayed2}")

    ' Alphabet validity still applies
    Try
      With new TooLongAlphabet()
        .ToNanoIdFactory()
      End With
    Catch x As AlphabetException When x.Reason.IsAlphabetTooLarge
      WriteLine($"Failure! {x.Message}")
    End Try
  End Sub
End Module
