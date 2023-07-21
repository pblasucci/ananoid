/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
namespace pblasucci.Ananoid.Compat.Basics;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments

using Customization;
using Support;

using static NanoIdParser;
// ⮝⮝⮝ defines parsers for many known alphabets as static members

[Properties(Arbitrary = new[] { typeof(Generation) })]
public class Rehydrating
{
  private static Property TestParser(NanoIdOptions options, NanoIdParser parser)
  {
    var nanoId1 = NanoId.NewId(options);
    var didParse = parser.TryParse(nanoId1, out var nanoId2);
    // NOTE a NanoId is implicitly a string ⮝⮝⮝
    return (didParse && nanoId1.Equals(nanoId2)).Collect((nanoId1, nanoId2));
  }


  [Property(MaxTest = 1)]
  public Property Alphanumeric_parser_succeeds_against_known_value()
    => TestParser(NanoIdOptions.Alphanumeric, Alphanumeric);

  [Property(MaxTest = 1)]
  public Property HexadecimalLowercase_parser_succeeds_against_known_value()
    => TestParser(NanoIdOptions.HexadecimalLowercase, HexadecimalLowercase);

  [Property(MaxTest = 1)]
  public Property HexadecimalUppercase_parser_succeeds_against_known_value()
    => TestParser(NanoIdOptions.HexadecimalUppercase, HexadecimalUppercase);

  [Property(MaxTest = 1)]
  public Property Lowercase_parser_succeeds_against_known_value()
    => TestParser(NanoIdOptions.Lowercase, Lowercase);

  [Property(MaxTest = 1)]
  public Property NoLookalikes_parser_succeeds_against_known_value()
    => TestParser(NanoIdOptions.NoLookalikes, NoLookalikes);

  [Property(MaxTest = 1)]
  public Property NoLookalikesSafe_parser_succeeds_against_known_value()
    => TestParser(NanoIdOptions.NoLookalikesSafe, NoLookalikesSafe);

  [Property(MaxTest = 1)]
  public Property Numbers_parser_succeeds_against_known_value()
    => TestParser(NanoIdOptions.Numbers, Numbers);

  [Property(MaxTest = 1)]
  public Property Uppercase_parser_succeeds_against_known_value()
    => TestParser(NanoIdOptions.Uppercase, Uppercase);

  [Property(MaxTest = 1)]
  public Property UrlSafe_parser_succeeds_against_known_value()
    => TestParser(NanoIdOptions.UrlSafe, UrlSafe);

  [Property(MaxTest = 1)]
  public Property Custom_parser_succeeds_against_known_value()
  {
    Qwerty12345Alphabet alphabet = new();

    var options = NanoIdOptions.CreateOrThrow(alphabet, size: 6);
    var parser = NanoIdParser.CreateOrThrow(alphabet);

    return TestParser(options, parser);
  }

  [Property(MaxTest = 1)]
  public Property Custom_parser_fails_against_known_incorrect_value()
  {
    Qwerty12345Alphabet alphabet = new();

    var options = NanoIdOptions.CreateOrThrow(alphabet, size: 6);
    var didParse = Numbers.TryParse(NanoId.NewId(options), out var parsed);

    return (didParse == false).Label($"Parsed: {parsed}");
  }

  [Property]
  public Property Any_parser_succeeds_against_known_value(
    NanoIdWithOptions input
  )
  {
    var (options, nanoId) = input;

    var parser = NanoIdParser.CreateOrThrow(options.Alphabet);
    var didParse = parser.TryParse(nanoId, out var parsed);

    return (didParse && parsed.Equals(nanoId)).Label($"{nanoId} != {parsed}");
  }
}