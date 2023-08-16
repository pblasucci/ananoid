Module Program
  Sub Main()
    WriteLine("ananoid.compat")

    ':: Basics ::
    WriteLine($"{NewLine}::{NameOf(Basics)}::")
    NanoIdsCanBeEmpty()
    NonEmptyNanoIds()
    RehydrateExistingValues()

    ':: Customization ::
    WriteLine($"{NewLine}::{NameOf(Customization)}::")
    PredefinedAlphabets()
    BringYourOwnAlphabet()
    CustomAlphabetRequirements()

    ':: Primitives ::
    WriteLine($"{NewLine}::{NameOf(Primitives)}::")
    BasicFunctions()
    BadInputsCauseArgumentOutOfRangeExceptions()
  End Sub
End Module
