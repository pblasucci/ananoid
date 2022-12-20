namespace MulberryLabs.Ananoid.Compat;

using static Console;
using static Alphabet; // defines all presets as static members.

internal static class Advanced
{
    public static void DelayedCreation()
    {
        // We can provide the alphabet now, and the size later.
        if (NanoId.TryDelay(Numbers, out var makeNanoId))
        {
            var delayed1 = makeNanoId(9);
            var delayed2 = makeNanoId(99);

            WriteLine("Some delayed NanoIds:");
            WriteLine($"\tdelayed, 9: {delayed1}");
            WriteLine($"\tdelayed, 99: {delayed2}");
        }

        // We can also investigate failures
        switch (NanoId.Delay(HexadecimalLowercase))
        {
            case { IsError: true, ErrorValue: var error }:
                WriteLine($"NanoId.Delay error: '{error.Message}'");
                break;

            case { ResultValue: var newNanoId }:
                var delayed3 = newNanoId(123);
                WriteLine($"\tdelayed, 123: {delayed3}");
                break;
        }
    }
}
