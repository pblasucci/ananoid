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
    AlphabetIsReallyIAlphabet()
    BringYourOwnAlphabet()
    CustomAlphabetRequirements()

    ':: Advanced ::
    WriteLine($"{NewLine}::{NameOf(Advanced)}::")
    BypassingNanoIdOptions()

    ':: Primitives ::
    WriteLine($"{NewLine}::{NameOf(Primitives)}::")
    BasicFunctions()
    BadInputsCauseArgumentOutOfRangeExceptions()
  End Sub
End Module
