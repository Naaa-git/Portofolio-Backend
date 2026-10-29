using System.Reflection;
using System.Text.Json.Serialization.Metadata;

namespace Portfolio.Api.Json;

public static class JsonTypeInfoModifiers
{
    private static readonly NullabilityInfoContext NullabilityContext = new();

    // Attaches EmptyStringToNullJsonConverter to every `string?` property across
    // every DTO, so "" from the client normalizes to null before DataAnnotations
    // validation runs — without touching non-nullable `string` properties,
    // Dictionary<string,string> values, or List<string> items.
    public static void NormalizeOptionalStrings(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        foreach (var property in typeInfo.Properties)
        {
            if (property.PropertyType != typeof(string))
                continue;

            if (property.AttributeProvider is not PropertyInfo member)
                continue;

            if (NullabilityContext.Create(member).ReadState == NullabilityState.Nullable)
                property.CustomConverter = new EmptyStringToNullJsonConverter();
        }
    }
}
