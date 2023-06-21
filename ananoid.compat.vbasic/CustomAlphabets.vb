NotInheritable Class CustomAlphabet1
  Implements IAlphabet

  ''' <inheritdoc />
  Public Function WillPermit(value As String) As Boolean _
    Implements IAlphabet.WillPermit
    Return value.All(Function(c) Letters.Contains(c))
  End Function

  ''' <inheritdoc />
  Public ReadOnly Property Letters As String = "qwerty123" _
    Implements IAlphabet.Letters
End Class


NotInheritable Class TooShortAlphabet
  Implements IAlphabet

  ''' <inheritdoc />
  Public Function WillPermit(value As String) As Boolean _
    Implements IAlphabet.WillPermit
    Return String.IsNullOrWhiteSpace(value)
  End Function

  ''' <inheritdoc />
  Public ReadOnly Property Letters As String = "" _
    Implements IAlphabet.Letters
End Class


NotInheritable Class TooLongAlphabet
  Implements IAlphabet

  ''' <inheritdoc />
  Public Function WillPermit(value As String) As Boolean _
    Implements IAlphabet.WillPermit
    Return value IsNot Nothing AndAlso value.Length = 0
  End Function

  ''' <inheritdoc />
  Public ReadOnly Property Letters As New String("$"c, 386) _
    Implements IAlphabet.Letters
End Class


NotInheritable Class IncoherentAlphabet
  Implements IAlphabet

  ''' <inheritdoc />
  Public Function WillPermit(value As String) As Boolean _
    Implements IAlphabet.WillPermit
    Return String.IsNullOrWhiteSpace(value)
  End Function

  ''' <inheritdoc />
  Public ReadOnly Property Letters As String = "abc123DoReMe" _
    Implements IAlphabet.Letters
End Class
