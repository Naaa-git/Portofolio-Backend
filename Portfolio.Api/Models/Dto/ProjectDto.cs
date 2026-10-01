namespace Portfolio.Api.Models.Dto;

public record ProjectDto(
    int Id, string Title, string Slug, string ShortDescription, string LongDescription,
    List<string> TechStack, string ImageUrl, string GithubUrl, string DemoUrl,
    bool Featured, string Category);

public record ProjectUpsertDto(
    string Title, string Slug, string ShortDescription, string LongDescription,
    List<string> TechStack, string ImageUrl, string GithubUrl, string DemoUrl,
    bool Featured, string Category);
