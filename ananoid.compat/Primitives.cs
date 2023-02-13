namespace pblasucci.Ananoid.Compat;

using static Console;
using static pblasucci.Ananoid.Core;

internal static class Primitives
{
    public static void BasicFunctions()
    {
        // The default NanoId, but as just a string.
        var value1 = NewNanoId();

        // Customize the size, but still use the default alphabet.
        var value2 = NewNanoId(alphabet: Alphabet.UrlSafe.ToString(), size: 9);

        // Or choose your size and use your own alphabet.
        var value3 = NewNanoId(alphabet: "abc123DoeRayMe", size: 99);

        WriteLine("Some primitive NanoIds:");
        WriteLine($"\t...Url-safe, 21: {value1}");
        WriteLine($"\t...Url-safe, 9: {value2}");
        WriteLine($"\t...'abc123DoeRayMe', 99: {value3}");
    }

    public static void BadInputsCauseArgumentOutOfRangeExceptions()
    {
        // as a courtesy, sizes are truncated at 0 -- returning an empty string.
        WriteLine(NewNanoId(alphabet: "abc123DoeRayMe", size: -3));

        try
        {
            // alphabet must be at least one letter
            WriteLine(NewNanoId(alphabet: "", size: 1500));
        }
        catch (ArgumentOutOfRangeException x)
        {
            WriteLine($"Primitive error: '{x.Message}'");
        }

        try
        {
            // alphabet cannot exceed 255 letters
            WriteLine(NewNanoId(alphabet: new('$', 386), size: 15));
        }
        catch (ArgumentOutOfRangeException x)
        {
            WriteLine($"Primitive error: '{x.Message}'");
        }
    }
}
