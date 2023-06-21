Module Program
  Sub Main()
    WriteLine("ananoid.compat")

    ':: Basics ::
    WriteLine($"{NewLine}::{NameOf(Basics)}::")
    Call NanoIdsCanBeEmpty
    Call NonEmptyNanoIds
    Call RehydrateExistingValues

    ':: Customization ::
    WriteLine($"{NewLine}::{NameOf(Customization)}::")
    Call PredefinedAlphabets
    Call AlphabetIsReallyIAlphabet
    Call BringYourOwnAlphabet
    Call CustomAlphabetRequirements

    ':: Advanced ::
    WriteLine($"{NewLine}::{NameOf(Advanced)}::")
    Call BypassingNanoIdOptions

    ':: Primitives ::
    WriteLine($"{NewLine}::{NameOf(Primitives)}::")
    Call BasicFunctions
    Call BadInputsCauseArgumentOutOfRangeExceptions
  End Sub
End Module
