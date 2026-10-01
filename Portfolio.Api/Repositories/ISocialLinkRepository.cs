using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface ISocialLinkRepository
{
    Task<List<SocialLink>> GetAllAsync();
    Task<SocialLink> AddAsync(SocialLink link);
    Task<bool> UpdateAsync(SocialLink link);
    Task<bool> DeleteAsync(int id);
}
