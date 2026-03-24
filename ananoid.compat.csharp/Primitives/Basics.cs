/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#pragma warning disable CS1591 // ⮜⮜⮜ missing XMLDoc comments
namespace pblasucci.Ananoid.Compat.Primitives;

using System.Text.RegularExpressions;
using static Core; // ⮜⮜⮜ primitive functions are in the `Core` module

public class Basics
{
  private static readonly Regex Urlsafe21 = new(
    "^[a-zA-Z0-9_-]{21}$",
    RegexOptions.Compiled,
    TimeSpan.FromSeconds(1)
  );

  private static readonly Regex Numeric128 = new(
    "^[0-9]{128}$",
    RegexOptions.Compiled,
    TimeSpan.FromSeconds(1)
  );

  [Fact]
  public void Default_is_UrlSafe_alphabet_and_size_21()
  {
    Assert.Matches(Urlsafe21, NewNanoId());
  }

  [Fact]
  public void Both_alphabet_and_size_can_be_changed()
  {
    Assert.Matches(Numeric128, NewNanoId(alphabet: "0123456789", size: 128));
  }
}
