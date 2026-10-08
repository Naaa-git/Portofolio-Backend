using Microsoft.Extensions.Options;
using OpenSearch.Client;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Models.Search;

namespace Portfolio.Api.Services;

public class ProjectSearchService : IProjectSearchService
{
    private const string IndexName = "projects";
    private readonly OpenSearchClient _client;
    private readonly ILogger<ProjectSearchService> _logger;

    public ProjectSearchService(IOptions<OpenSearchOptions> options, ILogger<ProjectSearchService> logger)
    {
        _logger = logger;
        var settings = new ConnectionSettings(new Uri(options.Value.Url))
            .DefaultIndex(IndexName);
        _client = new OpenSearchClient(settings);
    }

    public async Task EnsureIndexAsync()
    {
        var exists = await _client.Indices.ExistsAsync(IndexName);
        if (exists.Exists) return;

        await _client.Indices.CreateAsync(IndexName, c => c
            .Map<ProjectSearchDocument>(m => m.AutoMap()));
    }

    public async Task ReindexAllAsync(IEnumerable<Project> projects)
    {
        var docs = projects.Select(ToDocument).ToList();
        if (docs.Count == 0) return;

        var response = await _client.IndexManyAsync(docs, IndexName);
        if (!response.IsValid)
        {
            // Search is a secondary, best-effort read path — if OpenSearch is down or
            // misbehaving at startup, log it and keep the app running on Postgres alone
            // rather than failing the whole API.
            _logger.LogWarning("OpenSearch reindex failed: {Reason}", response.DebugInformation);
        }
    }

    public async Task IndexAsync(Project project)
    {
        try
        {
            await _client.IndexAsync(ToDocument(project), i => i.Index(IndexName).Id(project.Id));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to index project {ProjectId} into OpenSearch", project.Id);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _client.DeleteAsync<ProjectSearchDocument>(id, d => d.Index(IndexName));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete project {ProjectId} from OpenSearch", id);
        }
    }

    public async Task<List<ProjectDto>> SearchAsync(string query, string lang)
    {
        // Two "should" clauses, OR'd together — a hit on either is enough:
        //   1. Fuzzy whole-word match (typo tolerance, e.g. "redsi" -> "redis")
        //   2. Prefix match on the last word being typed (search-as-you-type,
        //      e.g. "red" -> "redis", without needing to re-index anything)
        var response = await _client.SearchAsync<ProjectSearchDocument>(s => s
            .Index(IndexName)
            .Query(q => q
                .Bool(b => b
                    .Should(
                        sh => sh.MultiMatch(m => m
                            .Query(query)
                            .Fuzziness(Fuzziness.Auto)
                            .Fields(FieldsWithBoost)),
                        sh => sh.MultiMatch(m => m
                            .Query(query)
                            .Type(TextQueryType.PhrasePrefix)
                            .Fields(FieldsWithBoost)))
                    .MinimumShouldMatch(1))));

        if (!response.IsValid)
        {
            _logger.LogWarning("OpenSearch query failed: {Reason}", response.DebugInformation);
            return [];
        }

        return response.Documents.Select(d => ToDto(d, lang)).ToList();
    }

    private static Func<FieldsDescriptor<ProjectSearchDocument>, IPromise<Fields>> FieldsWithBoost => f => f
        .Field(d => d.Title, boost: 3)
        .Field(d => d.TechStack, boost: 2)
        .Field(d => d.CategoryId)
        .Field(d => d.CategoryEn)
        .Field(d => d.ShortDescriptionId)
        .Field(d => d.ShortDescriptionEn)
        .Field(d => d.LongDescriptionId)
        .Field(d => d.LongDescriptionEn);

    private static ProjectSearchDocument ToDocument(Project p) => new()
    {
        Id = p.Id,
        Title = p.Title,
        Slug = p.Slug,
        ShortDescriptionId = p.ShortDescription.Resolve("id"),
        ShortDescriptionEn = p.ShortDescription.Resolve("en"),
        LongDescriptionId = p.LongDescription.Resolve("id"),
        LongDescriptionEn = p.LongDescription.Resolve("en"),
        CategoryId = p.Category.Resolve("id"),
        CategoryEn = p.Category.Resolve("en"),
        TechStack = p.TechStack,
        ImageUrl = p.ImageUrl,
        GithubUrl = p.GithubUrl,
        DemoUrl = p.DemoUrl,
        Featured = p.Featured,
    };

    private static ProjectDto ToDto(ProjectSearchDocument d, string lang) => new(
        d.Id, d.Title, d.Slug,
        lang == "en" ? d.ShortDescriptionEn : d.ShortDescriptionId,
        lang == "en" ? d.LongDescriptionEn : d.LongDescriptionId,
        d.TechStack, d.ImageUrl, d.GithubUrl, d.DemoUrl, d.Featured,
        lang == "en" ? d.CategoryEn : d.CategoryId);
}
