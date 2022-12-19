using MulberryLabs.Ananoid.Compat;

Console.WriteLine("ananoid.compat");

// basics
Console.WriteLine("\n::Basics::");
Basics.NanoIdsCanBeEmpty();
Basics.NonEmptyNanoIds();
Basics.RehydrateExistingValues();

// customization
Console.WriteLine("\n::Customization::");
Customization.PredefinedAlphabets();
Customization.AlphabetIsReallyIAlphabet();
Customization.BringYourOwnAlphabet();
Customization.CustomAlphabetRequirements();

// primitives
Console.WriteLine("\n::Primitives::");
Primitives.BasicFunctions();
Primitives.BadInputsCauseArgumentOutOfRangeExceptions();
