// ⮟⮟⮟ missing XMLDoc comments

using pblasucci.Ananoid.Compat.Customization;
using pblasucci.Ananoid.Compat.Support;

#pragma warning disable CS1591

namespace pblasucci.Ananoid.Compat.Advanced;

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
    var maker = alphabet.ToNanoIdFactory().GetValueOrDefault()!;
    // The desired size can be specified -- and varied -- later!
    return alphabet.WillPermit(maker(size1)) &&
           alphabet.WillPermit(maker(size2));
  }

  [Property(MaxTest = 1)]
  public bool Factory_validates_alphabets_passing()
    => new Qwerty12345Alphabet().ToNanoIdFactory() is { IsOk: true };

  [Property(MaxTest = 1)]
  public bool Factory_validates_alphabets_failing()
    => new IncoherentAlphabet().ToNanoIdFactory() is { IsOk: false };

}
