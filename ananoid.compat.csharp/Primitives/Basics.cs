/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
namespace pblasucci.Ananoid.Compat.Primitives;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments

using System.Text.RegularExpressions;

using static Core;
// ⮝⮝⮝ primitive functions are in the `Core` module


public class Basics
{
  static readonly Regex Urlsafe21 = new(
    "^[a-zA-Z0-9_-]{21}$",
    RegexOptions.Compiled,
    TimeSpan.FromSeconds(1)
  );

  static readonly Regex Numeric128 = new(
    "^[0-9]{128}$",
    RegexOptions.Compiled,
    TimeSpan.FromSeconds(1)
  );

  [Property(MaxTest = 1)]
  public Property default_is_UrlSafe_alphabet_and_size_21()
  {
    var value = NewNanoId();
    return Urlsafe21.IsMatch(value).Label($"Not {Urlsafe21}: {value}");
  }

  [Property(MaxTest = 1)]
  public Property both_alphabet_and_size_can_be_changed()
  {
    var value = NewNanoId(alphabet: "0123456789", size: 128);
    return Numeric128.IsMatch(value).Label($"Not {Numeric128}: {value}");
  }
}
