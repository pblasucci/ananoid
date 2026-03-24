/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#pragma warning disable CS1591 // ⮜⮜⮜ missing XMLDoc comments
namespace pblasucci.Ananoid.Compat.Basics;

using Support;
using static KnownAlphabets; // ⮜⮜⮜ several common alphabets are defined here

[Properties(AutoGenConfig = typeof(Generation))]
public class Rehydrating
{
  private static void TestParser(Alphabet alphabet, int size)
  {
    var nanoId = alphabet.MakeNanoId(size);
    bool didParse = alphabet.TryParseNanoId(nanoId.ToString(), out var parsed);

    Assert.Multiple(
      () => Assert.True(didParse),
      () => Assert.Equal(nanoId, parsed)
    );
  }

  [Fact]
  public void Alphanumeric_parser_succeeds_against_known_value()
    => TestParser(Alphanumeric, Core.Defaults.Size);

  [Fact]
  public void HexadecimalLowercase_parser_succeeds_against_known_value()
    => TestParser(HexadecimalLowercase, Core.Defaults.Size);

  [Fact]
  public void HexadecimalUppercase_parser_succeeds_against_known_value()
    => TestParser(HexadecimalUppercase, Core.Defaults.Size);

  [Fact]
  public void Lowercase_parser_succeeds_against_known_value()
    => TestParser(Lowercase, Core.Defaults.Size);

  [Fact]
  public void NoLookalikes_parser_succeeds_against_known_value()
    => TestParser(NoLookalikes, Core.Defaults.Size);

  [Fact]
  public void NoLookalikesSafe_parser_succeeds_against_known_value()
    => TestParser(NoLookalikesSafe, Core.Defaults.Size);

  [Fact]
  public void Numbers_parser_succeeds_against_known_value()
    => TestParser(Numbers, Core.Defaults.Size);

  [Fact]
  public void Uppercase_parser_succeeds_against_known_value()
    => TestParser(Uppercase, Core.Defaults.Size);

  [Fact]
  public void UrlSafe_parser_succeeds_against_known_value()
    => TestParser(UrlSafe, Core.Defaults.Size);

  [Fact]
  public void Custom_parser_succeeds_against_known_value()
    => TestParser("qwerty123".ToAlphabetOrThrow(), 6);

  [Fact]
  public void Custom_parser_fails_against_known_incorrect_value()
  {
    var valid = "qwerty123".ToAlphabetOrThrow();
    var original = valid.MakeNanoId(size: 6);
    var didParse = Numbers.TryParseNanoId(original.ToString(), out _);
    // ⮝⮝⮝ `Numbers` alphabet is only `0123456789`
    // ... so parsing a value derived from alphabet `qwerty123` *should fail*.
    Assert.False(didParse);
  }

  [Property]
  public Property<bool> Any_parser_succeeds_against_known_value(
    Alphabet alphabet, [PositiveInt(100000)] int length
  )
  {
    var nanoId = alphabet.MakeNanoId(length);
    var didParse = alphabet.TryParseNanoId(nanoId.ToString(), out var parsed);
    var result =
      from _ in Property.CounterExample(() => $"{nanoId} != {parsed}")
      select didParse && nanoId.Equals(parsed);
    return result;
  }

  [Property]
  public bool Can_explicitly_fail_to_parse_empty_input(Alphabet alphabet)
    => alphabet.TryParseNonEmptyNanoId(string.Empty, out _) is not true;
}
