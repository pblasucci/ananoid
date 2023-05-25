#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments

namespace pblasucci.Ananoid.Compat.Support;

public record NanoIdWithOptions(NanoIdOptions Options, NanoId Value);

public static class Generation
{
  public static Arbitrary<NanoIdOptions> GenerateNanoIdOptions()
    => Arb.From(
      gen: Gen.Sized(
        size =>
          Gen.Elements(
            NanoIdOptions.Alphanumeric.Resize(size),
            NanoIdOptions.HexadecimalLowercase.Resize(size),
            NanoIdOptions.HexadecimalUppercase.Resize(size),
            NanoIdOptions.Lowercase.Resize(size),
            NanoIdOptions.NoLookalikes.Resize(size),
            NanoIdOptions.NoLookalikesSafe.Resize(size),
            NanoIdOptions.Numbers.Resize(size),
            NanoIdOptions.Uppercase.Resize(size),
            NanoIdOptions.UrlSafe.Resize(size)
          )
      ),
      shrinker: options => options.Size switch
      {
        0 => Enumerable.Empty<NanoIdOptions>(),
        var size => Arb.Shrink(size).Select(options.Resize)
      }
    );

  public static Arbitrary<NanoIdWithOptions> GenerateNanoIdWithOptions()
    => Arb.From(
      // generate
      from options in Arb.Generate<NanoIdOptions>()
      from size in Gen.Choose(0, 1024)
      let nanoId = NanoId.NewId(options.Resize(size))
      select new NanoIdWithOptions(options, nanoId),
      // shrink
      input => input switch
      {
        { Value.Length: 0 } => Enumerable.Empty<NanoIdWithOptions>(),
        { Value.Length: var length } =>
          from size in Arb.Shrink(length)
          let options = input.Options.Resize((int) size)
          let nanoId = NanoId.NewId(options)
          select new NanoIdWithOptions(options, nanoId)
      }
    );
}
