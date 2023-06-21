/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
namespace pblasucci.Ananoid.Compat.Customization;

// ⮟⮟⮟ missing XMLDoc comments
#pragma warning disable CS1591

using Xunit;

public class BringYourOwnAlphabets
{
  [Property]
  public Property Custom_alphabet_permits_value_it_creates(NonNegativeInt input)
  {
    var options = NanoIdOptions.CreateOrThrow(
      new Qwerty12345Alphabet(),
      size: (int)input
    );
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString()).Collect(nanoId);
  }

  [Fact]
  public void Custom_alphabet_must_be_at_least_one_letter()
  {
    var x = Assert.Throws<AlphabetException>(
      () => Alphabet.ValidateOrThrow(new TooShortAlphabet())
    );

    Assert.True(x.Reason.IsAlphabetTooSmall);
  }

  [Fact]
  public void Custom_alphabet_must_be_less_then_256_letters()
  {
    var x = Assert.Throws<AlphabetException>(
      () => Alphabet.ValidateOrThrow(new TooLongAlphabet())
    );

    Assert.True(x.Reason.IsAlphabetTooLarge);
  }

  [Fact]
  public void Custom_alphabet_must_be_coherent()
  {
    var x = Assert.Throws<AlphabetException>(
      () => Alphabet.ValidateOrThrow(new IncoherentAlphabet())
    );

    Assert.True(x.Reason.IsIncoherentAlphabet);
  }
}

internal class Qwerty12345Alphabet : IAlphabet
{
  /// <inheritdoc />
  public bool WillPermit(string value) => value.All(Letters.Contains);

  /// <inheritdoc />
  public string Letters => "qwerty123";
}

internal class TooShortAlphabet : IAlphabet
{
  /// <inheritdoc />
  public bool WillPermit(string value) => value is { Length: 0 };

  /// <inheritdoc />
  public string Letters => "";
}

internal class TooLongAlphabet : IAlphabet
{
  /// <inheritdoc />
  public bool WillPermit(string value) => value is { Length: 0 };

  /// <inheritdoc />
  public string Letters => new('$', 386);
}

internal class IncoherentAlphabet : IAlphabet
{
  /// <inheritdoc />
  public bool WillPermit(string value) => String.IsNullOrWhiteSpace(value);

  /// <inheritdoc />
  public string Letters => "abc123DoReMe";
}
