using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Validation;

// [StringLength] only works on `string` — translatable fields are
// Dictionary<string,string> (or List<Dictionary<string,string>> for the list
// variant), so neither shape can carry it directly. This attribute mirrors
// [StringLength(max, MinimumLength = min)]'s exact semantics — minimumLength > 0
// doubles as "required" (an empty/missing value fails the minlength check),
// same as the ContactMessageSubmitDto pattern — just applied per-value across
// whichever of the two shapes is passed in.
public class TranslatableStringLengthAttribute(int maximumLength) : ValidationAttribute
{
    private static readonly string[] Languages = ["id", "en"];

    // Named-argument property, mirroring [StringLength(max, MinimumLength = min)]'s
    // exact ergonomics — a constructor-only parameter can't be set via `Name = value`
    // attribute syntax unless it's a property.
    public int MinimumLength { get; set; }

    public override bool IsValid(object? value)
    {
        switch (value)
        {
            case null:
                return MinimumLength == 0;
            case Dictionary<string, string> dict:
                return DictInRange(dict);
            case List<Dictionary<string, string>> list:
                return list.All(DictInRange);
            default:
                return false;
        }
    }

    // Checks both known language keys explicitly (not just whatever's present) —
    // a dict missing "id" or "en" entirely must fail a MinimumLength > 0 check
    // the same as an empty string would, not pass vacuously because there was
    // nothing to iterate.
    private bool DictInRange(Dictionary<string, string> dict) =>
        Languages.All(lang => ValueInRange(dict.GetValueOrDefault(lang, "")));

    private bool ValueInRange(string value) =>
        value.Length >= MinimumLength && value.Length <= maximumLength;
}
