namespace pblasucci.Ananoid.Compat;

using static Console;

internal static class Advanced
{
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
