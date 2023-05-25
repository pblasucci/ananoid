using System.Text.RegularExpressions;

// ⮟⮟⮟ missing XMLDoc comments
#pragma warning disable CS1591

namespace pblasucci.Ananoid.Compat.Primitives;

// ⮟⮟⮟ primitive functions are in the `Core` module
using static pblasucci.Ananoid.Core;

public class Basics
{
  static Regex Default = new(
    "^[a-zA-Z0-9_-]{21}$",
    RegexOptions.Compiled,
    TimeSpan.FromSeconds(1)
  );

  static Regex Numeric128 = new(
    "^[0-9]{128}$",
    RegexOptions.Compiled,
    TimeSpan.FromSeconds(1)
  );

  [Property(MaxTest = 1)]
  public Property default_is_UrlSafe_alphabet_and_size_21()
  {
    var value = NewNanoId();
    return Default.IsMatch(value).Label($"Not {Default}: {value}");
  }

  [Property(MaxTest = 1)]
  public Property both_alphabet_and_size_can_be_changed()
  {
    var value = NewNanoId(alphabet: "0123456789", size: 128);
    return Numeric128.IsMatch(value).Label($"Not {Numeric128}: {value}");
  }
}
