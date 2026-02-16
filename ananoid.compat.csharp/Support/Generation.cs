/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using Hedgehog;
using Range = Hedgehog.Linq.Range;

namespace pblasucci.Ananoid.Compat.Support;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments

public static class Generation
{
  public static Gen<Alphabet> Alphabet =
    Gen.Item(
      KnownAlphabets.Alphanumeric,
      KnownAlphabets.HexadecimalLowercase,
      KnownAlphabets.HexadecimalUppercase,
      KnownAlphabets.Lowercase,
      KnownAlphabets.NoLookalikes,
      KnownAlphabets.NoLookalikesSafe,
      KnownAlphabets.Numbers,
      KnownAlphabets.Uppercase,
      KnownAlphabets.UrlSafe
    );

  public static IAutoGenConfig Config =>
    AutoGenConfig.Defaults.AddGenerator(Alphabet);
}

public class SmallNegativeIntAttribute : GenAttribute<int>
{
  public override Gen<int> Generator => Gen.Int32(Range.Constant(-100, -1));
}
