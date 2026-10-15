namespace Portfolio.Api.Services;

public interface IFileStorageService
{
    /// <summary>Stores an image and returns a publicly accessible URL for it.</summary>
    Task<string> UploadImageAsync(IFormFile file);

    /// <summary>Stores a document (e.g. a CV PDF) and returns a publicly accessible URL for it.</summary>
    Task<string> UploadDocumentAsync(IFormFile file);
}
