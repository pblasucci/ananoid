/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using Xunit.Abstractions;

namespace pblasucci.Ananoid.Compat.Customization;

// ⮟⮟⮟ missing XMLDoc comments
#pragma warning disable CS1591
using Xunit;

public class BringYourOwnAlphabets
{
  private readonly ITestOutputHelper output;
  public BringYourOwnAlphabets(ITestOutputHelper output)
  {
    this.output = output;
  }

  [Property]
  public Property Custom_alphabet_permits_value_it_creates(NonNegativeInt input)
  {
    switch (Alphabet.Validate("qwerty123"))
    {
      case { IsOk: false, ErrorValue: var error }:
        return false.Label($"{error.Message} for '{error.Letters}'");

      case { IsOk: true, ResultValue: var alphabet }:
        var nanoId = alphabet.MakeNanoId(size: (int)input);
        var okay = alphabet.TryParseNanoId(nanoId.ToString(), out var parsed);
        return (okay && nanoId.Equals(parsed)).Collect(nanoId);
    }
  }

  [Property(MaxTest = 1)]
  public void Custom_alphabet_must_be_at_least_one_letter()
  {
    var x = Assert.Throws<AlphabetException>(() => "".ToAlphabetOrThrow());
    output.WriteLine($"FATAL! {x.Alphabet}");

    Assert.True(x.Reason.IsAlphabetTooSmall);
  }

  [Fact]
  public void Custom_alphabet_must_be_less_then_256_letters()
  {
    var x = Assert.Throws<AlphabetException>(
      () => new string('$', 386).ToAlphabetOrThrow()
    );
    output.WriteLine($"FATAL! {x.Alphabet}");

    Assert.True(x.Reason.IsAlphabetTooLarge);
  }
}
