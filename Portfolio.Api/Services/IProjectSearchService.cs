using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Services;

public interface IProjectSearchService
{
    /// <summary>Creates the "projects" index with its mapping if it doesn't exist yet.</summary>
    Task EnsureIndexAsync();

    /// <summary>Rebuilds the index from scratch from the given projects. Used at startup so the index never drifts out of sync with Postgres.</summary>
    Task ReindexAllAsync(IEnumerable<Project> projects);

    Task IndexAsync(Project project);
    Task DeleteAsync(int id);
    Task<List<ProjectDto>> SearchAsync(string query, string lang);
}
