using System.Text.Json;
using System.Text.Json.Serialization;

namespace Portfolio.Api.Json;

// The frontend sends "" for an unfilled optional field (e.g. DemoUrl), not null,
// but [Url]/[Required] only special-case null — so "" still fails [Url]'s format
// check. Attached per-property to every `string?` DTO property (see
// JsonTypeInfoModifiers), not globally to every `string`, so it never touches
// Dictionary<string,string> translatable values or List<string> items.
public class EmptyStringToNullJsonConverter : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (value is null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value);
    }
}
