/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
namespace pblasucci.Ananoid.Compat.Advanced;

// ⮟⮟⮟ missing XMLDoc comments
#pragma warning disable CS1591


public class CustomizingNanoIdOptions
{
  [Property(MaxTest = 1)]
  public Property Alphanumeric_options_permits_any_value_it_creates()
  {
    var options = NanoIdOptions.Alphanumeric;
    // A NanoId can be parameterized by alphabet and size (of generated value),
    // and there are NanoIdOptions instances for several known alphabets.
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }

  [Property(MaxTest = 1)]
  public Property HexadecimalLowercase_options_permits_any_value_it_creates()
  {
    var options = NanoIdOptions.HexadecimalLowercase;
    // A NanoId can be parameterized by alphabet and size (of generated value),
    // and there are NanoIdOptions instances for several known alphabets.
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }

  [Property(MaxTest = 1)]
  public Property HexadecimalUppercase_options_permits_any_value_it_creates()
  {
    var options = NanoIdOptions.HexadecimalUppercase;
    // A NanoId can be parameterized by alphabet and size (of generated value),
    // and there are NanoIdOptions instances for several known alphabets.
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }

  [Property(MaxTest = 1)]
  public Property Lowercase_options_permits_any_value_it_creates()
  {
    var options = NanoIdOptions.Lowercase;
    // A NanoId can be parameterized by alphabet and size (of generated value),
    // and there are NanoIdOptions instances for several known alphabets.
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }

  [Property(MaxTest = 1)]
  public Property NoLookalikes_options_permits_any_value_it_creates()
  {
    var options = NanoIdOptions.NoLookalikes;
    // A NanoId can be parameterized by alphabet and size (of generated value),
    // and there are NanoIdOptions instances for several known alphabets.
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }

  [Property(MaxTest = 1)]
  public Property NoLookalikesSafe_options_permits_any_value_it_creates()
  {
    var options = NanoIdOptions.NoLookalikesSafe;
    // A NanoId can be parameterized by alphabet and size (of generated value),
    // and there are NanoIdOptions instances for several known alphabets.
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }

  [Property(MaxTest = 1)]
  public Property Numbers_options_permits_any_value_it_creates()
  {
    var options = NanoIdOptions.Numbers;
    // A NanoId can be parameterized by alphabet and size (of generated value),
    // and there are NanoIdOptions instances for several known alphabets.
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }

  [Property(MaxTest = 1)]
  public Property Uppercase_options_permits_any_value_it_creates()
  {
    var options = NanoIdOptions.Uppercase;
    // A NanoId can be parameterized by alphabet and size (of generated value),
    // and there are NanoIdOptions instances for several known alphabets.
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }

  [Property(MaxTest = 1)]
  public Property UrlSafe_options_permits_any_value_it_creates()
  {
    var options = NanoIdOptions.UrlSafe;
    // A NanoId can be parameterized by alphabet and size (of generated value),
    // and there are NanoIdOptions instances for several known alphabets.
    var nanoId = NanoId.NewId(options);

    return options.Alphabet.WillPermit(nanoId.ToString())
      .Collect(nanoId);
  }
}
