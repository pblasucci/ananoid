Public Module Basics
  Sub NanoIdsCanBeEmpty()
    ' NanoId is a struct (value type), whose default value is 'empty'.
    Dim empty0 As NanoId

    ' ⮟⮟⮟ This is the same as the previous line.
    Dim empty1 As New NanoId()

    ' ⮟⮟⮟ This is the preferred idiom.
    Dim empty2 = NanoId.Empty
    ' An instance may check the `.IsEmpty` property.
    WriteLine($"{nameof(empty0)} is empty? {empty0.Length < 1}")

    ' Empty instances are always equal.
    Dim bothAreEqual = empty1.Equals(empty2)
    WriteLine($"{nameof(empty1)} equals {nameof(empty2)}? {bothAreEqual}")
  End Sub

  Sub NonEmptyNanoIds()
    ' This generates a new instance using default options
    ' ("default" = a 21-character long string of URL-safe characters).
    Dim nanoId1 = NanoId.NewId()

    ' Coercing an instance to string reveals its value.
    WriteLine($"{nameof(nanoId1)} = {nanoId1.ToString()}")

    ' This is the same as the default behavior.
    Dim nanoId2 = KnownAlphabets.UrlSafe.MakeNanoId(size:=21)

    ' This one is much shorter.
    Dim nanoId4 = KnownAlphabets.UrlSafe.MakeNanoId(size:=5)

    ' This one is empty!
    Dim empty3 = KnownAlphabets.UrlSafe.MakeNanoId(size:=0)

    ' This one uses a different pre-defined alphabet.
    Dim nanoId5 = KnownAlphabets.HexadecimalLowercase.MakeNanoId(size:=21)

    ' This one uses a different pre-defined alphabet and is very long.
    Dim nanoId6 = KnownAlphabets.HexadecimalLowercase.MakeNanoId(size:=1024)

    WriteLine("Some different NanoIds:")
    WriteLine($"{vbTab}...Url-safe, 21: {nanoId2}")
    WriteLine($"{vbTab}...Url-safe, 5: {nanoId4}")
    WriteLine($"{vbTab}...Url-safe, 0: {empty3}")
    WriteLine($"{vbTab}...Hexadecimal, 21: {nanoId5}")
    WriteLine($"{vbTab}...Hexadecimal, 1024: {nanoId6}")
  End Sub

  Sub RehydrateExistingValues()
    ' A NanoIdParser can validate strings and transform them into NanoIds.
    Dim parsed1 As NanoId
    If KnownAlphabets.UrlSafe.TryParseNanoId("ypswLHEC", parsed1) Then
      WriteLine($"Parsed Url-safe: {parsed1}")
    End If

    ' NanoIdParser instances are pre-defined for all the well-know alphabets,
    ' because different alphabets have different validation criteria.
    Dim parsed2 As NanoId
    If Not KnownAlphabets.Numbers.TryParseNanoId("!@#$%", parsed2) Then
      WriteLine($"{nameof(parsed2)} never was parsed: '{parsed2}'")
    End If

    ' By default, an empty string can be parsed into any alphabet, but
    ' parsing can be told to explicitly fail on empty inputs instead.
    If KnownAlphabets.Uppercase.ParseNonEmptyNanoId("    ") Is Nothing Then
      WriteLine($"Could not parse an empty string")
    End If
  End Sub
End Module
