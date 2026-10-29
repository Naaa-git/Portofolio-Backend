using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Portfolio.Api.Json;

namespace Portfolio.Api.Tests;

public class EmptyStringToNullJsonConverterTests
{
    private record SampleDto([Url] string? DemoUrl, [Required] string Title, Dictionary<string, string> Translated);

    private static readonly JsonSerializerOptions Options = new()
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver { Modifiers = { JsonTypeInfoModifiers.NormalizeOptionalStrings } },
    };

    [Fact]
    public void EmptyOptionalUrl_DeserializesToNull_SoItPassesUrlValidation()
    {
        var dto = JsonSerializer.Deserialize<SampleDto>(
            """{"DemoUrl":"","Title":"x","Translated":{}}""", Options)!;

        Assert.Null(dto.DemoUrl);
        Assert.True(Validator.TryValidateProperty(dto.DemoUrl,
            new ValidationContext(dto) { MemberName = nameof(SampleDto.DemoUrl) }, null));
    }

    [Fact]
    public void NonEmptyOptionalUrl_IsUnaffected()
    {
        var dto = JsonSerializer.Deserialize<SampleDto>(
            """{"DemoUrl":"https://example.com","Title":"x","Translated":{}}""", Options)!;

        Assert.Equal("https://example.com", dto.DemoUrl);
    }

    [Fact]
    public void RequiredNonNullableString_EmptyStaysEmpty_SoRequiredStillCatchesIt()
    {
        var dto = JsonSerializer.Deserialize<SampleDto>(
            """{"DemoUrl":null,"Title":"","Translated":{}}""", Options)!;

        Assert.Equal("", dto.Title);
    }

    [Fact]
    public void DictionaryStringValues_AreNotNormalized()
    {
        var dto = JsonSerializer.Deserialize<SampleDto>(
            """{"DemoUrl":null,"Title":"x","Translated":{"id":""}}""", Options)!;

        Assert.Equal("", dto.Translated["id"]);
    }
}
