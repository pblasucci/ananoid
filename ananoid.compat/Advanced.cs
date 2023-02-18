namespace pblasucci.Ananoid.Compat;

using static Console;

internal static class Advanced
{
  public static void DecomposingNanoIdOptions()
  {
    // We can access the data in a NanoIdOptions instance several ways.
    WriteLine("NanoId options for 'NoLookalikes' by:");

    var alphabet1 = NanoIdOptions.NoLookalikes.Alphabet;
    var targetSize1 = NanoIdOptions.NoLookalikes.Size;
    WriteLine($"\t...Member access = ({alphabet1}, {targetSize1})");

    var (alphabet2, targetSize2) = NanoIdOptions.NoLookalikes;
    WriteLine($"\t...Decomposition = ({alphabet2}, {targetSize2})");

    switch (NanoIdOptions.NoLookalikes)
    {
      case { Alphabet: var alphabet3, Size: var targetSize3 }:
        WriteLine($"\t...Pattern matching = ({alphabet3}, {targetSize3})");
        break;
    }
  }

  public static void BypassingNanoIdOptions()
  {
    // We can provide the alphabet now, and the size later.
    var (isOk, factory, _) = Alphabet.Numbers.ToNanoIdFactory();
    if (isOk && factory is not null)
    {
      var delayed1 = factory(9);
      var delayed2 = factory(99);

      WriteLine("Some delayed NanoIds:");
      WriteLine($"\t...Numeric, 9: {delayed1}");
      WriteLine($"\t...Numeric, 99: {delayed2}");
    }

    // We can also investigate failures
    switch (new TooLongAlphabet().ToNanoIdFactory())
    {
      case { IsError: true, ErrorValue: var error }:
        WriteLine($"Alphabet.ToNanoIdFactory error: '{error?.Message}'");
        break;

      case { ResultValue: var newNanoId }:
        var delayed3 = newNanoId(123);
        WriteLine($"\t...Hexadecimal, 123: {delayed3}");
        break;
    }
  }
}
