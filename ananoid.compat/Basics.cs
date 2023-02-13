namespace pblasucci.Ananoid.Compat;

using static Console;
using static NanoIdOptions; // has useful presets exposed as static members.

internal static class Basics
{
  public static void NanoIdsCanBeEmpty()
  {
    // NanoId is a struct (value type), whose default value is 'empty'.
    NanoId empty0 = default;

    // ⮟⮟⮟ This is the same as the previous line.
    NanoId empty1 = new();

    // ⮟⮟⮟ This is the preferred idiom.
    var empty2 = NanoId.Empty;

    // An instance may check the `.IsEmpty` property.
    WriteLine($"{nameof(empty0)} is empty? {NanoId.IsEmpty(empty0)}");

    // Empty instances are always equal.
    var bothAreEqual = empty1.Equals(empty2);
    WriteLine($"{nameof(empty1)} equals {nameof(empty2)}? {bothAreEqual}");
  }

  public static void NonEmptyNanoIds()
  {
    // This generates a new instance using default options
    // ("default" = a 21-character long string of URL-safe characters).
    var nanoId1 = NanoId.NewId();

    // Coercing an instance to string reveals its value.
    WriteLine($"{nameof(nanoId1)} = {nanoId1.ToString()}");

    // The type NanoIdOptions can be used to customize the generated NanoId.
    // This is the same as the default behavior.
    var options1 = Of(Alphabet.UrlSafe, size: 21).GetValueOrDefault();
    var nanoId2 = NanoId.NewId(options1);

    // This is the same as the default behavior.
    var nanoId3 = NanoId.NewId(UrlSafe);

    // This one is much shorter.
    var nanoId4 = NanoId.NewId(UrlSafe.Resize(5));

    // This one is empty!
    var empty3 = NanoId.NewId(UrlSafe.Resize(0));

    // This one uses a different pre-defined alphabet.
    var nanoId5 = NanoId.NewId(HexadecimalLowercase);

    // This one uses a different pre-defined alphabet and is very long.
    var options2 = HexadecimalLowercase.Resize(1024);
    var nanoId6 = NanoId.NewId(options2);

    WriteLine("Some different NanoIds:");
    WriteLine($"\t...Url-safe, 21: {nanoId2}");
    WriteLine($"\t...Url-safe, 21: {nanoId3}");
    WriteLine($"\t...Url-safe, 5: {nanoId4}");
    WriteLine($"\t...Url-safe, 0: {empty3}");
    WriteLine($"\t...Hexadecimal, 21: {nanoId5}");
    WriteLine($"\t...Hexadecimal, 1024: {nanoId6}");
  }

  public static void RehydrateExistingValues()
  {
    // A NanoIdParser can validate strings and transform them into NanoIds.
    if (NanoIdParser.UrlSafe.TryParse("ypswLHEC", out var parsed1))
    {
      WriteLine($"Parsed Url-safe: {parsed1}");
    }

    // NanoIdParser instances are pre-defined for all the well-know alphabets,
    // because different alphabets have different validation criteria.
    var numbers = NanoIdParser.Numbers;
    if (numbers.TryParse("!@#$%", out var parsed2) is false)
    {
      WriteLine($"{nameof(parsed2)} never was parsed: '{parsed2}'");
    }

    // Custom alphabets are supported, too.
    var parser = NanoIdParser.Of(new CustomAlphabet1()).GetValueOrDefault();
    if (parser is not null && parser.TryParse("qw3rty", out var parsed3))
    {
      WriteLine($"Parsed custom: {parsed3}");
    }
  }
}
