using pblasucci.Ananoid.Compat.Support;

// ⮟⮟⮟ missing XMLDoc comments
#pragma warning disable CS1591

namespace pblasucci.Ananoid.Compat.Customization;

public class BringYourOwnAlphabets
{
  [Property]
  public Property Custom_alphabet_permits_any_value_it_creates(
    NonNegativeInt input
  )
  {
    var options =
      NanoIdOptions
        .Of(new Qwerty12345Alphabet(), size: (int)input)
        // ⮝⮝⮝ In addition to known alphabets, you can provide your own.
        .GetValueOrThrow(fail => new InvalidProgramException(fail.ToString()));

    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }

  [Property(MaxTest = 1)]
  public bool Custom_alphabet_must_be_at_least_one_letter()
  {
    var result = Alphabet.Validate(new TooShortAlphabet());
    // We can use pattern matching to extract data from an `FSharpResult`.
    return result switch
    {
      { IsOk: false, ErrorValue: var error } => error.IsAlphabetTooSmall,
      _ => false
    };
  }

  [Property(MaxTest = 1)]
  public bool Custom_alphabet_must_be_less_then_256_letters()
  {
    var result = Alphabet.Validate(new TooLongAlphabet());
    // `FSharpResult` can be easily decomposed as a three-tuple.
    return result is (false, _, var error) && error!.IsAlphabetTooLarge;
  }

  [Property(MaxTest = 1)]
  public bool Custom_alphabet_must_be_coherent()
  {
    //NOTE 'coherent' means an alphabet can validate its own letters
    var result = Alphabet.Validate(new IncoherentAlphabet());
    // `.Match` can be a very concise way to work with `FSharpResult`.
    return result.Match(okay: _ => false, error: x => x.IsIncoherentAlphabet);
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
