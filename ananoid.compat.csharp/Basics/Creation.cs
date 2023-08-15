/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

namespace pblasucci.Ananoid.Compat.Basics;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments
using Support;

[Properties(Arbitrary = new[] { typeof(Generation) })]
public class Creation
{
  [Property]
  public bool All_forms_of_empty_are_equal(NegativeInt input)
  {
    // By default, a NanoId is empty.
    NanoId empty1 = default;

    // But using the static member `Empty` is preferred.
    var empty2 = NanoId.Empty;

    // empty instance can be created from sizes less than one.
    var empty3 = KnownAlphabets.UrlSafe.MakeNanoId(size: (int)input);

    return empty1.Equals(empty2) &&
           empty2.Equals(empty3) &&
           empty3.Equals(empty1);
  }

  [Property(MaxTest = 1)]
  public bool Being_empty_means_having_length_zero()
  {
    // NanoId has a fixed length -- even as a string.
    return NanoId.Empty.Length is 0 && NanoId.Empty.ToString()!.Length is 0;
  }

  [Property]
  public Property Any_number_of_empties_are_equal(PositiveInt input)
  {
    var count = (int)input;
    var items =
      from _ in Enumerable.Range(start: 1, count) select default(NanoId);

    // The static method `NanoId.IsEmpty()` can check for emptiness, too.
    return items.All(n => n.Length < 1).Collect(count);
  }

  [Property]
  public Property Nano_id_length_is_input_size(
    Alphabet alphabet,
    NonNegativeInt input
  )
  {
    var size = (int)input;
    // An alphabet is really just a factory for generating NanoId instances.
    var nanoId = alphabet.MakeNanoId(size);

    return (nanoId.Length == size).Collect((size, nanoId));
  }

  [Property(MaxTest = 1)]
  public Property By_default_new_nanoid_is_21_url_safe_characters()
  {
    // This is the most common way to get a (non-empty) NanoId instance.
    var nanoId = NanoId.NewId();

    return KnownAlphabets.UrlSafe
      .TryParseNanoId(nanoId.ToString(), out _)
      // ⮝⮝⮝ if the alphabet can successfully parse it,
      // ... that's a good-enough proxy for "is of this alphabet".
      .And(nanoId.Length is 21)
      .Collect(nanoId);
  }
}
