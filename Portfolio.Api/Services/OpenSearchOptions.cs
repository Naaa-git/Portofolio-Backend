namespace Portfolio.Api.Services;

public class OpenSearchOptions
{
    public const string SectionName = "OpenSearch";

    public string Url { get; set; } = "http://localhost:9200";
}
