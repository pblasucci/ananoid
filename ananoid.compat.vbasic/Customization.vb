Imports pblasucci.Ananoid.Alphabet

Public Module Customization
  Sub PredefinedAlphabets()
    WriteLine("Some pre-defined alphabets:")
    WriteLine($"{vbTab}{nameof(Alphanumeric)}: {Alphanumeric}")
    WriteLine($"{vbTab}{nameof(HexadecimalLowercase)}: {HexadecimalLowercase}")
    WriteLine($"{vbTab}{nameof(HexadecimalUppercase)}: {HexadecimalUppercase}")
    WriteLine($"{vbTab}{nameof(Lowercase)}: {Lowercase}")
    WriteLine($"{vbTab}{nameof(NoLookalikes)}: {NoLookalikes}")
    WriteLine($"{vbTab}{nameof(NoLookalikesSafe)}: {NoLookalikesSafe}")
    WriteLine($"{vbTab}{nameof(Numbers)}: {Numbers}")
    WriteLine($"{vbTab}{nameof(Uppercase)}: {Uppercase}")
    WriteLine($"{vbTab}{nameof(UrlSafe)}: {UrlSafe}")
  End Sub

  Sub AlphabetIsReallyIAlphabet()
    Dim alphabet1 As IAlphabet = Numbers
    ' NOTE `(IAlphabet) Alphabet.Numbers` will always be "safe".

    Dim isCoherent = alphabet1.WillPermit(alphabet1.Letters)
    WriteLine($"{nameof(Numbers)} [0-9] okay? {isCoherent}")

    If TypeOf(NoLookalikesSafe) Is IAlphabet Then
      Dim alphabet2 As IAlphabet = NoLookalikesSafe
      Dim areEqual = NoLookalikesSafe.ToString() = alphabet2.Letters
      WriteLine($"Alphabet.ToString() == IAlphabet.Letters? {areEqual}")
    End If
  End Sub

  Sub BringYourOwnAlphabet()
    Dim options = NanoIdOptions.CreateOrThrow(new CustomAlphabet1(), size := 5)
    Dim custom1 = NanoId.NewId(options)
    WriteLine($"Custom alphabet, 5: {custom1}")
  End Sub

  Sub CustomAlphabetRequirements()
    ' size can not be negative
    Try
      NanoIdOptions.CreateOrThrow(new CustomAlphabet1(), size := -3)
    Catch x As AlphabetException
      WriteLine($"Failure! reason: '{x.Message}', source: {x.Source}")
    End Try

    ' alphabet must be at least one letter
    Try
      NanoIdOptions.CreateOrThrow(new TooShortAlphabet(), size := 128)
    Catch x As AlphabetException
      WriteLine($"Failure! reason: '{x.Message}', source: {x.Source}")
    End Try

    ' alphabet cannot exceed 255 letters
    Try
      NanoIdOptions.CreateOrThrow(new TooLongAlphabet(), size := 15)
    Catch x As AlphabetException
      WriteLine($"Failure! reason: '{x.Message}', source: {x.Source}")
    End Try

    ' alphabet must be coherent (ie: permit itself)
    Try
      NanoIdOptions.CreateOrThrow(new IncoherentAlphabet(), size := 21)
    Catch x As AlphabetException
      WriteLine($"Failure! reason: '{x.Message}', source: {x.Source}")
    End Try

  End Sub
End Module
