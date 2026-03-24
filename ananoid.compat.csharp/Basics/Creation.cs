/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#pragma warning disable CS1591 // ⮜⮜⮜ missing XMLDoc comments
namespace pblasucci.Ananoid.Compat.Basics;

using Support;
using static KnownAlphabets; // ⮜⮜⮜ several common alphabets are defined here

[Properties(typeof(Generation))]
public class Creation
{
  [Property]
  public bool All_forms_of_empty_are_equal([SmallNegativeInt] int input)
  {
    // By default, a NanoId is empty.
    NanoId empty1 = default;

    // But using the static member `Empty` is preferred.
    var empty2 = NanoId.Empty;

    // An empty instance can be created from sizes less than one.
    var empty3 = UrlSafe.MakeNanoId(size: input);

    return empty1.Equals(empty2) &&
           empty2.Equals(empty3) &&
           empty3.Equals(empty1);
  }

  [Fact]
  public void Being_empty_means_having_length_zero()
  {
    // NanoId has a fixed length -- even as a string.
    Assert.Multiple(
      () => Assert.Equal(0, NanoId.Empty.Length),
      () => Assert.Equal(0, NanoId.Empty.ToString()!.Length)
    );
  }

  [Property]
  public bool Any_number_of_empties_are_equal([PositiveInt(1000000)] int count)
  {
    return Enumerable.Range(start: 1, count).Select(_ => default(NanoId))
      .All(NanoId.IsEmpty);
  }

  [Property]
  public bool Nano_id_length_is_input_size(
    Alphabet alphabet,
    [NonNegativeInt(1000000)] int inputSize
  )
  {
    // An alphabet is really just a factory for generating NanoId instances.
    var nanoId = alphabet.MakeNanoId(inputSize);
    return (nanoId.Length == inputSize);
  }

  [Fact]
  public void By_default_new_nanoid_is_21_url_safe_characters()
  {
    var nanoId = NanoId.NewId().ToString();
    var isUrlSafe = UrlSafe.TryParseNanoId(nanoId, out var parsed);
    // ⮝⮝⮝ if the alphabet can successfully parse it,
    // ... that's a good-enough proxy for "is of this alphabet".
    Assert.True(isUrlSafe && parsed.Length is 21);
  }
}
