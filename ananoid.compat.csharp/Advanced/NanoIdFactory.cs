/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
namespace pblasucci.Ananoid.Compat.Advanced;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments

using Customization;


public class NanoIdFactory
{
  [Property]
  public bool Factory_created_functions_produce_valid_values(
    NonNegativeInt input1,
    NonNegativeInt input2
  )
  {
    var size1 = (int)input1;
    var size2 = (int)input2;

    IAlphabet alphabet = Alphabet.UrlSafe;
    // If you know the alphabet, but not the desired size, you can
    // partially compute a function for creating NanoId instances.
    var maker = alphabet.ToNanoIdFactory();
    // The desired size can be specified -- and varied -- later!
    return alphabet.WillPermit(maker(size1)) &&
           alphabet.WillPermit(maker(size2));
  }

  [Property(MaxTest = 1)]
  public bool Factory_validates_alphabets_passing()
    => new Qwerty12345Alphabet().ToNanoIdFactory() is not null;

  [Fact]
  public void Factory_validates_alphabets_failing()
  {
    Assert.Throws<AlphabetException>(
      () => new IncoherentAlphabet().ToNanoIdFactory()
    );
  }
}
