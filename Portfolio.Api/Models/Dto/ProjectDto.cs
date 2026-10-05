namespace Portfolio.Api.Models.Dto;

public record ProjectDto(
    int Id, string Title, string Slug, string ShortDescription, string LongDescription,
    List<string> TechStack, string ImageUrl, string GithubUrl, string DemoUrl,
    bool Featured, string Category);

public record ProjectAdminDto(
    int Id, string Title, string Slug, Dictionary<string, string> ShortDescription, Dictionary<string, string> LongDescription,
    List<string> TechStack, string ImageUrl, string GithubUrl, string DemoUrl,
    bool Featured, Dictionary<string, string> Category);

public record ProjectUpsertDto(
    string Title, string Slug, Dictionary<string, string> ShortDescription, Dictionary<string, string> LongDescription,
    List<string> TechStack, string ImageUrl, string GithubUrl, string DemoUrl,
    bool Featured, Dictionary<string, string> Category);
