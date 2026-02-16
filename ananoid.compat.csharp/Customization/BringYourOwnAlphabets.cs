/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#pragma warning disable CS1591 // ⮜⮜⮜ missing XMLDoc comments
namespace pblasucci.Ananoid.Compat.Customization;

using Xunit;

public class BringYourOwnAlphabets
{
  [Property]
  public bool Custom_alphabet_permits_value_it_creates(
    [NonZeroInt(1, 1000)] int inputSize)
  {
    var alphabet = "qwerty123".ToAlphabetOrThrow();
    var nanoId = alphabet.MakeNanoId(inputSize);
    var didParse = alphabet.TryParseNanoId(nanoId.ToString(), out var parsed);
    return didParse && parsed.Equals(nanoId);
  }

  [Fact]
  public void Custom_alphabet_must_be_at_least_one_letter()
  {
    var x = Assert.Throws<AlphabetException>(() => "".ToAlphabetOrThrow());
    Console.WriteLine($"FATAL! {x.Reason}");

    Assert.True(x.Reason.IsAlphabetTooSmall);
  }

  [Fact]
  public void Custom_alphabet_must_be_less_then_256_letters()
  {
    var tooLarge = new string('#', 386);
    var x = Assert.Throws<AlphabetException>(tooLarge.ToAlphabetOrThrow);
    Console.WriteLine($"FATAL! {x.Reason}");

    Assert.True(x.Reason.IsAlphabetTooLarge);
  }
}
