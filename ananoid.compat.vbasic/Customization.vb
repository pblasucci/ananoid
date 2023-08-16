Imports Microsoft.FSharp.Core
Imports pblasucci.Ananoid.KnownAlphabets

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

  Sub BringYourOwnAlphabet()
    Dim result = Alphabet.Validate("qwerty123")
    If result.IsOk then
      Dim alphabet = result.ResultValue
      Dim custom1 = alphabet.MakeNanoId(size := 5)
      WriteLine($"Custom alphabet, 5: {custom1}")
    Else
      Dim reason = result.ErrorValue
      WriteLine($"Invalid alphabet: {reason}")
    End If
  End Sub

  Sub CustomAlphabetRequirements()
    ' alphabet must be at least one letter
    Try
      Dim alphabet = "".ToAlphabetOrThrow()
    Catch x As AlphabetException
      WriteLine($"Failure! reason: '{x.Message}', source: {x.Source}")
    End Try

    ' alphabet cannot exceed 255 letters
    Try
      Dim alphabet = new String("$"c, 386).ToAlphabetOrThrow()
    Catch x As AlphabetException
      WriteLine($"Failure! reason: '{x.Message}', source: {x.Source}")
    End Try
  End Sub
End Module
