namespace pblasucci.Ananoid.Compat;

using static Console;
using static Alphabet; // defines all presets as static members.

internal static class Customization
{
  public static void PredefinedAlphabets()
  {
    WriteLine("Some pre-defined alphabets:");
    WriteLine($"\t{nameof(Alphanumeric)}: {Alphanumeric}");
    WriteLine($"\t{nameof(HexadecimalLowercase)}: {HexadecimalLowercase}");
    WriteLine($"\t{nameof(HexadecimalUppercase)}: {HexadecimalUppercase}");
    WriteLine($"\t{nameof(Lowercase)}: {Lowercase}");
    WriteLine($"\t{nameof(NoLookalikes)}: {NoLookalikes}");
    WriteLine($"\t{nameof(NoLookalikesSafe)}: {NoLookalikesSafe}");
    WriteLine($"\t{nameof(Numbers)}: {Numbers}");
    WriteLine($"\t{nameof(Uppercase)}: {Uppercase}");
    WriteLine($"\t{nameof(UrlSafe)}: {UrlSafe}");
  }

  public static void AlphabetIsReallyIAlphabet()
  {
    IAlphabet alphabet1 = Numbers;
    // NOTE `(IAlphabet) Alphabet.Numbers` will always be "safe".
    var isCoherent = alphabet1.IncludesAll(alphabet1.Letters);
    WriteLine($"{nameof(Numbers)} [0-9] okay? {isCoherent}");

    if (NoLookalikesSafe is IAlphabet alphabet2)
    {
      var areEqual = NoLookalikesSafe.ToString() == alphabet2.Letters;
      WriteLine($"Alphabet.ToString() == IAlphabet.Letters? {areEqual}");
    }
  }

  public static void BringYourOwnAlphabet()
  {
    var options = NanoIdOptions.Of(new CustomAlphabet1(), size: 5)
      .GetValueOrThrow(x => new ArgumentException(x?.Message));

    var custom1 = NanoId.NewId(options);
    WriteLine($"Custom alphabet, 5: {custom1}");
  }

  public static void CustomAlphabetRequirements()
  {
    // size can not be negative
    var options = NanoIdOptions.Of(new CustomAlphabet1(), size: -3);
    if (options is (false, _, var error1))
    {
      WriteLine($"NanoIdOptions error: '{error1?.Message}'");
    }

    // alphabet must be at least one letter
    var error2 =
      NanoIdOptions.Of(new TooShortAlphabet(), size: 512) switch
      {
        (false, _, var error) => error,
        (true, _, _) => default
      };
    WriteLine($"NanoIdOptions error: '{error2?.Message}'");


    // alphabet cannot exceed 255 letters
    var failed3 = NanoIdOptions.Of(new TooLongAlphabet(), size: 15);
    if (failed3.IsError)
    {
      WriteLine($"NanoIdOptions error: '{failed3.ErrorValue?.Message}'");
    }
  }
}

internal class CustomAlphabet1 : IAlphabet
{
  /// <inheritdoc />
  public bool IncludesAll(string value) => value.All(Letters.Contains);

  /// <inheritdoc />
  public string Letters => "qwerty123";
}

internal class TooShortAlphabet : IAlphabet
{
  /// <inheritdoc />
  public bool IncludesAll(string value) => value is { Length: 0 };

  /// <inheritdoc />
  public string Letters => "";
}

internal class TooLongAlphabet : IAlphabet
{
  /// <inheritdoc />
  public bool IncludesAll(string value) => value is { Length: 0 };

  /// <inheritdoc />
  public string Letters => new('$', 386);
}
