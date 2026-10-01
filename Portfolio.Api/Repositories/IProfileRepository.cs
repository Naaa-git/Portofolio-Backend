using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface IProfileRepository
{
    Task<Profile?> GetAsync();
    Task<Profile> UpsertAsync(Profile profile);
}
