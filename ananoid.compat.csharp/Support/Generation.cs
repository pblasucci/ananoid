/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

namespace pblasucci.Ananoid.Compat.Support;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments

public record NanoIdWithAlphabet(Alphabet Alphabet, NanoId Value);

public static class Generation
{
  public static Arbitrary<Alphabet> GenerateValidAlphabet()
    => Arb.From(
      Gen.Elements(
        KnownAlphabets.Alphanumeric,
        KnownAlphabets.HexadecimalLowercase,
        KnownAlphabets.HexadecimalUppercase,
        KnownAlphabets.Lowercase,
        KnownAlphabets.NoLookalikes,
        KnownAlphabets.NoLookalikesSafe,
        KnownAlphabets.Numbers,
        KnownAlphabets.Uppercase,
        KnownAlphabets.UrlSafe
      )
    );

  public static Arbitrary<NanoIdWithAlphabet> GenerateNanoIdWithAlphabet()
    => Arb.From(
      // generate
      from alphabet in Arb.Generate<Alphabet>()
      from size in Gen.Choose(0, 1024)
      let nanoId = alphabet.MakeNanoId(size: size)
      select new NanoIdWithAlphabet(alphabet, nanoId),
      // shrink
      input => input switch
      {
        { Value.Length: 1 } => Enumerable.Empty<NanoIdWithAlphabet>(),
        { Value.Length: var length } =>
          from size in Arb.Shrink(length)
          let nanoId = input.Alphabet.MakeNanoId(size)
          select input with { Value = nanoId }
      }
    );
}
