/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

namespace pblasucci.Ananoid.Compat.Basics;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments
using Support;
using static KnownAlphabets;

[Properties(Arbitrary = new[] { typeof(Generation) })]
public class Rehydrating
{
  private static Property TestParser(Alphabet alphabet, int size)
  {
    var nanoId1 = alphabet.MakeNanoId(size);
    var didParse = alphabet.TryParseNanoId(nanoId1.ToString(), out var nanoId2);

    return (didParse && nanoId1.Equals(nanoId2)).Collect((nanoId1, nanoId2));
  }

  [Property(MaxTest = 1)]
  public Property Alphanumeric_parser_succeeds_against_known_value()
    => TestParser(Alphanumeric, Core.Defaults.Size);

  [Property(MaxTest = 1)]
  public Property HexadecimalLowercase_parser_succeeds_against_known_value()
    => TestParser(HexadecimalLowercase, Core.Defaults.Size);

  [Property(MaxTest = 1)]
  public Property HexadecimalUppercase_parser_succeeds_against_known_value()
    => TestParser(HexadecimalUppercase, Core.Defaults.Size);

  [Property(MaxTest = 1)]
  public Property Lowercase_parser_succeeds_against_known_value()
    => TestParser(Lowercase, Core.Defaults.Size);

  [Property(MaxTest = 1)]
  public Property NoLookalikes_parser_succeeds_against_known_value()
    => TestParser(NoLookalikes, Core.Defaults.Size);

  [Property(MaxTest = 1)]
  public Property NoLookalikesSafe_parser_succeeds_against_known_value()
    => TestParser(NoLookalikesSafe, Core.Defaults.Size);

  [Property(MaxTest = 1)]
  public Property Numbers_parser_succeeds_against_known_value()
    => TestParser(Numbers, Core.Defaults.Size);

  [Property(MaxTest = 1)]
  public Property Uppercase_parser_succeeds_against_known_value()
    => TestParser(Uppercase, Core.Defaults.Size);

  [Property(MaxTest = 1)]
  public Property UrlSafe_parser_succeeds_against_known_value()
    => TestParser(UrlSafe, Core.Defaults.Size);

  [Property(MaxTest = 1)]
  public Property Custom_parser_succeeds_against_known_value()
    => TestParser("qwerty123".ToAlphabetOrThrow(), 6);

  [Property(MaxTest = 1)]
  public Property Custom_parser_fails_against_known_incorrect_value()
  {
    var valid = "qwerty123".ToAlphabetOrThrow();
    var original = valid.MakeNanoId(size: 6);
    var didParse = Numbers.TryParseNanoId(original.ToString(), out var parsed);

    return (didParse == false).Label($"{original} <> {parsed}");
  }

  [Property]
  public Property Any_parser_succeeds_against_known_value(
    NanoIdWithAlphabet input
  )
  {
    var (alphabet, nanoId) = input;
    var didParse = alphabet.TryParseNanoId(nanoId.ToString(), out var parsed);
    return (didParse && nanoId.Equals(parsed)).Label($"{nanoId} != {parsed}");
  }

  [Property]
  public bool Can_explicitly_fail_to_parse_empty_input(Alphabet alphabet)
    => alphabet.TryParseNonEmptyNanoId(string.Empty, out _) is false;
}
