/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
namespace pblasucci.Ananoid.Compat.Primitives;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments

using static pblasucci.Ananoid.Core;
// ⮝⮝⮝ primitive functions are in the `Core` module


public class Failures
{
  [Property(MaxTest = 1)]
  public Property negative_sizes_produce_an_empty_string()
  {
    var value = NewNanoId(alphabet: "abcdefghijklmnopqrstuvwxyz", size: -3);
    return (value is { Length: 0 }).Label("Not empty: {value}");
  }

  [Fact]
  public void alphabet_must_be_at_least_one_character()
  {
    Assert.Throws<ArgumentOutOfRangeException>(
      () => NewNanoId(alphabet: string.Empty, size: 21)
    );
  }

  [Fact]
  public void alphabet_cannot_be_more_than_255_characters()
  {
    Assert.Throws<ArgumentOutOfRangeException>(
      () => NewNanoId(alphabet: new('!', 1024), size: 21)
    );
  }
}
