/*
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
namespace pblasucci.Ananoid.Compat.Basics;

#pragma warning disable CS1591
// ⮝⮝⮝ missing XMLDoc comments

using pblasucci.Ananoid.Compat.Support;


[Properties(Arbitrary = new[] { typeof(Generation) })]
public class Creation
{
  [Property(MaxTest = 1)]
  public bool all_forms_of_empty_are_equal()
  {
    // By default, a NanoId is empty.
    NanoId emptyId = default;

    // But using the static member `Empty` is preferred.
    return emptyId.Equals(NanoId.Empty);
  }

  [Property(MaxTest = 1)]
  public bool being_empty_means_having_length_zero()
  {
    // NanoId has a fixed length -- even as a string.
    return NanoId.Empty.Length is 0 && NanoId.Empty.ToString()!.Length is 0;
  }

  [Property]
  public Property any_number_of_empties_are_equal(PositiveInt input)
  {
    var count = (int)input;
    var items =
      from _ in Enumerable.Range(start: 1, count) select default(NanoId);

    // The static method `NanoId.IsEmpty()` can check for emptiness, too.
    return items.All(NanoId.IsEmpty).Collect(count);
  }

  [Property]
  public Property nano_id_length_is_options_size(
    NanoIdOptions options,
    NonNegativeInt input
  )
  {
    var size = (int)input;
    // NanoIdOptions for known alphabets default to generating 21-character
    // strings, but can easily to customized to generate larger or smaller ones.
    var resized = options.Resize(size);
    var nanoId = NanoId.NewId(resized);

    var lengthSize = nanoId.Length == size && nanoId.ToString()!.Length == size;
    return lengthSize.Collect((resized, nanoId));
  }

  [Property]
  public Property resizing_options_does_not_change_alphabet(
    NanoIdOptions options,
    PositiveInt input
  )
  {
    var newSize = (int)input;
    // A NanoIdOptions instance can be decomposed into a tuple.
    var (alphabet, size) = options;

    return Test().When(newSize != size).Collect((alphabet, size, newSize));

    bool Test()
    {
      var resized = options.Resize(newSize);
      return resized.Alphabet.Equals(alphabet);
    }
  }

  [Property(MaxTest = 1)]
  public bool options_resized_to_zero_generate_empty_nano_ids(
    NanoIdOptions options
  )
  {
    var resized = options.Resize(0);
    var nanoId = NanoId.NewId(resized);
    return NanoId.IsEmpty(nanoId);
  }

  [Property(MaxTest = 1)]
  public Property by_default_new_nanoid_is_21_url_safe_characters()
  {
    // This is the most common way to get a (non-empty) NanoId instance.
    var nanoId = NanoId.NewId();

    return NanoIdOptions.UrlSafe switch
    {
      // Should you wish to patten match, `NanoIdOptions` instances
      // expose their alphabet and target size as read-only properties.
      { Alphabet: var alphabet, Size: var size } =>
        alphabet
          .WillPermit(nanoId)
          .And(nanoId.Length == size)
          .Collect(nanoId),

      // ⮟⮟⮟ Will never match!
      _ => false.ToProperty()
    };
  }
}
