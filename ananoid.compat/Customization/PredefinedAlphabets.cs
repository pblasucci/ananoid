/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
namespace pblasucci.Ananoid.Compat.Customization;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments

using static Alphabet;
// ⮝⮝⮝ defines many known alphabets as static members


public class PredefinedAlphabets
{
  [Property(MaxTest = 1)]
  public Property Alphanumeric_is_coherent()
  {
    // an instance of `alphabet` is an `IAlphabet`.
    IAlphabet alphabet = Alphanumeric;
    // NOTE `(IAlphabet) Alphabet.Alphanumeric` will always be "safe".
    return alphabet.WillPermit(alphabet.Letters).Collect(alphabet.Letters);
  }

  [Property(MaxTest = 1)]
  public Property HexadecimalLowercase_is_coherent()
  {
    // an instance of `alphabet` is an `IAlphabet`.
    IAlphabet alphabet = HexadecimalLowercase;
    // NOTE `(IAlphabet) Alphabet.HexadecimalLowercase` will always be "safe".
    return alphabet.WillPermit(alphabet.Letters).Collect(alphabet.Letters);
  }

  [Property(MaxTest = 1)]
  public Property HexadecimalUppercase_is_coherent()
  {
    // an instance of `alphabet` is an `IAlphabet`.
    IAlphabet alphabet = HexadecimalUppercase;
    // NOTE `(IAlphabet) Alphabet.HexadecimalUppercase` will always be "safe".
    return alphabet.WillPermit(alphabet.Letters).Collect(alphabet.Letters);
  }

  [Property(MaxTest = 1)]
  public Property Lowercase_is_coherent()
  {
    // an instance of `alphabet` is an `IAlphabet`.
    IAlphabet alphabet = Lowercase;
    // NOTE `(IAlphabet) Alphabet.Lowercase` will always be "safe".
    return alphabet.WillPermit(alphabet.Letters).Collect(alphabet.Letters);
  }

  [Property(MaxTest = 1)]
  public Property NoLookalikesSafe_is_coherent()
  {
    // an instance of `alphabet` is an `IAlphabet`.
    IAlphabet alphabet = NoLookalikesSafe;
    // NOTE `(IAlphabet) Alphabet.NoLookalikesSafe` will always be "safe".
    return alphabet.WillPermit(alphabet.Letters).Collect(alphabet.Letters);
  }

  [Property(MaxTest = 1)]
  public Property Numbers_is_coherent()
  {
    // an instance of `alphabet` is an `IAlphabet`.
    IAlphabet alphabet = Numbers;
    // NOTE `(IAlphabet) Alphabet.Numbers` will always be "safe".
    return alphabet.WillPermit(alphabet.Letters).Collect(alphabet.Letters);
  }

  [Property(MaxTest = 1)]
  public Property NoLookalikes_is_coherent()
  {
    // an instance of `alphabet` is an `IAlphabet`.
    IAlphabet alphabet = NoLookalikes;
    // NOTE `(IAlphabet) Alphabet.NoLookalikes` will always be "safe".
    return alphabet.WillPermit(alphabet.Letters).Collect(alphabet.Letters);
  }

  [Property(MaxTest = 1)]
  public Property UrlSafe_is_coherent()
  {
    // an instance of `alphabet` is an `IAlphabet`.
    IAlphabet alphabet = UrlSafe;
    // NOTE `(IAlphabet) Alphabet.UrlSafe` will always be "safe".
    return alphabet.WillPermit(alphabet.Letters).Collect(alphabet.Letters);
  }
}
