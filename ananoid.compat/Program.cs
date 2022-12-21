using MulberryLabs.Ananoid.Compat;

Console.WriteLine("ananoid.compat");

// basics
Console.WriteLine($"\n::{nameof(Basics)}::");
Basics.NanoIdsCanBeEmpty();
Basics.NonEmptyNanoIds();
Basics.RehydrateExistingValues();

// customization
Console.WriteLine($"\n::{nameof(Customization)}::");
Customization.PredefinedAlphabets();
Customization.AlphabetIsReallyIAlphabet();
Customization.BringYourOwnAlphabet();
Customization.CustomAlphabetRequirements();

// advanced
Console.WriteLine($"\n::{nameof(Advanced)}::");
Advanced.BypassingNanoIdOptions();

// primitives
Console.WriteLine($"\n::{nameof(Primitives)}::");
Primitives.BasicFunctions();
Primitives.BadInputsCauseArgumentOutOfRangeExceptions();
