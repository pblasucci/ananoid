Imports pblasucci.Ananoid.Core

Public Module Primitives
  Sub BasicFunctions()
    ' The default NanoId, but as just a string.
    Dim value1 = NewNanoId()

    ' Customize the size, but still use the default alphabet.
    Dim value2 = NewNanoId(Defaults.Alphabet, size := 9)

    ' Or choose your size and use your own alphabet.
    Dim value3 = NewNanoId(alphabet := "abc123DoeRayMe", size := 99)

    WriteLine("Some primitive NanoIds:")
    WriteLine($"{vbTab}...Url-safe, 21: {value1}")
    WriteLine($"{vbTab}...Url-safe, 9: {value2}")
    WriteLine($"{vbTab}...'abc123DoeRayMe', 99: {value3}")
  End Sub

  Sub BadInputsCauseArgumentOutOfRangeExceptions()
    ' as a courtesy, sizes are truncated at 0 -- returning an empty string.
    WriteLine(NewNanoId(alphabet := "abc123DoeRayMe", size := -3))

    ' alphabet must be at least one letter
    Try
      WriteLine(NewNanoId(alphabet := "", size := 1500))
    Catch x As ArgumentOutOfRangeException
        WriteLine($"Primitive error: '{x.Message}'")
    End Try

    ' alphabet cannot exceed 255 letters
    Try
      WriteLine(NewNanoId(alphabet := new String("$"c, 386), size := 15))
    Catch x As ArgumentOutOfRangeException
        WriteLine($"Primitive error: '{x.Message}'")
    End Try
  End Sub
End Module
