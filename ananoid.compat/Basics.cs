namespace MulberryLabs.Ananoid.Compat;

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
        NanoIdOptions options1 = Of(Alphabet.UrlSafe, size: 21).ResultValue;
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
        // Parsing can be done safely, via TryParse. Note: this method
        // assumes the input conforms to the default ("URL Safe") alphabet.
        if (NanoId.TryParse("ypswLHEC", out var parsed1))
        {
            WriteLine($"Parsed Url-safe: {parsed1}");
        }

        // An alphabet can be specified for more stringent processing.
        if (!NanoId.TryParse("!@#$%", Alphabet.Numbers, out var parsed2))
        {
            WriteLine($"{nameof(parsed2)} never was parsed: '{parsed2}'");
        }

        // Explicit failure can also be tracked, via Parse.
        switch (NanoId.Parse("!@#$%", Alphabet.Numbers))
        {
            case { IsOk: false, ErrorValue: var error }:
                Error.WriteLine($"Parsing failed with: '{error.Message}'");
                break;

            case { ResultValue: var parsed3 }:
                WriteLine($"{nameof(parsed3)} = {parsed3}");
                break;
        }
    }
}
