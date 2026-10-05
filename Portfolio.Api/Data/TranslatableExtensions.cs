using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Portfolio.Api.Data;

/// <summary>
/// Maps a translatable text field to a jsonb column storing {"id": "...", "en": "..."}.
/// Chosen over a separate translations table: same flexibility for a 2-language site,
/// no extra joins, native Postgres jsonb support.
/// </summary>
public static class TranslatableExtensions
{
    public static void IsTranslatable(this PropertyBuilder<Dictionary<string, string>> builder)
    {
        builder.HasColumnType("jsonb");
        builder.HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null) ?? new());
        builder.Metadata.SetValueComparer(new ValueComparer<Dictionary<string, string>>(
            (a, b) => (a ?? new()).OrderBy(x => x.Key).SequenceEqual((b ?? new()).OrderBy(x => x.Key)),
            d => d.Aggregate(0, (hash, kv) => HashCode.Combine(hash, kv.Key, kv.Value)),
            d => new Dictionary<string, string>(d)));
    }

    public static void IsTranslatableList(this PropertyBuilder<List<Dictionary<string, string>>> builder)
    {
        builder.HasColumnType("jsonb");
        builder.HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<Dictionary<string, string>>>(v, (JsonSerializerOptions?)null) ?? new());
        builder.Metadata.SetValueComparer(new ValueComparer<List<Dictionary<string, string>>>(
            (a, b) => JsonSerializer.Serialize(a, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(b, (JsonSerializerOptions?)null),
            d => JsonSerializer.Serialize(d, (JsonSerializerOptions?)null).GetHashCode(),
            d => d.Select(x => new Dictionary<string, string>(x)).ToList()));
    }

    /// <summary>Resolves a translatable dictionary to a single string for the requested language, falling back to "id".</summary>
    public static string Resolve(this Dictionary<string, string>? dict, string lang)
    {
        if (dict is null || dict.Count == 0) return "";
        if (dict.TryGetValue(lang, out var v) && !string.IsNullOrWhiteSpace(v)) return v;
        if (dict.TryGetValue("id", out var fallback) && !string.IsNullOrWhiteSpace(fallback)) return fallback;
        return dict.Values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? "";
    }

    public static List<string> ResolveList(this List<Dictionary<string, string>>? list, string lang) =>
        list?.Select(d => d.Resolve(lang)).ToList() ?? new();

    /// <summary>Builds a translatable dictionary from id/en values — used by the seeder.</summary>
    public static Dictionary<string, string> Tr(string id, string en) => new() { ["id"] = id, ["en"] = en };

    public static List<Dictionary<string, string>> TrList(params (string Id, string En)[] items) =>
        items.Select(x => Tr(x.Id, x.En)).ToList();
}
