namespace Portfolio.Api.Services;

public interface IImageStorageService
{
    /// <summary>Stores the file and returns a publicly accessible URL for it.</summary>
    Task<string> UploadAsync(IFormFile file);
}
