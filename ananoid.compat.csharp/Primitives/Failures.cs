/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#pragma warning disable CS1591 // ⮜⮜⮜ missing XMLDoc comments
namespace pblasucci.Ananoid.Compat.Primitives;

using static Core; // ⮜⮜⮜ primitive functions are in the `Core` module

public class Failures
{
  [Fact]
  public void Negative_sizes_produce_an_empty_string()
  {
    Assert.Empty(NewNanoId(alphabet: "abcdefghijklmnopqrstuvwxyz", size: -3));
  }

  [Fact]
  public void Alphabet_must_be_at_least_one_character()
  {
    Assert.Throws<ArgumentOutOfRangeException>(() =>
    {
      NewNanoId(alphabet: string.Empty, size: 21);
    });
  }

  [Fact]
  public void Alphabet_cannot_be_more_than_255_characters()
  {
    Assert.Throws<ArgumentOutOfRangeException>(() =>
    {
      NewNanoId(alphabet: new string('!', 1024), size: 21);
    });
  }
}
