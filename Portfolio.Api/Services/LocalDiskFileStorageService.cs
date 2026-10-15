namespace Portfolio.Api.Services;

/// <summary>
/// Saves uploaded files under wwwroot/uploads and serves them as static files. This is the
/// real production storage mechanism (VPS local disk) — not a mock standing in for something
/// else. If this ever needs to move to object storage (S3/Azure Blob/etc.), swap the
/// registration in Program.cs — IFileStorageService's contract stays the same, so no
/// controller/service code needs to change.
/// </summary>
public class LocalDiskFileStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    : IFileStorageService
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5 MB

    private static readonly string[] AllowedDocumentExtensions = [".pdf"];
    private const long MaxDocumentSizeBytes = 10 * 1024 * 1024; // 10 MB

    public Task<string> UploadImageAsync(IFormFile file) =>
        SaveAsync(file, AllowedImageExtensions, MaxImageSizeBytes);

    public Task<string> UploadDocumentAsync(IFormFile file) =>
        SaveAsync(file, AllowedDocumentExtensions, MaxDocumentSizeBytes);

    private async Task<string> SaveAsync(IFormFile file, string[] allowedExtensions, long maxSizeBytes)
    {
        if (file.Length == 0)
            throw new ArgumentException("File is empty.");

        if (file.Length > maxSizeBytes)
            throw new ArgumentException($"File exceeds the {maxSizeBytes / (1024 * 1024)} MB limit.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            throw new ArgumentException($"File type '{extension}' is not allowed.");

        var fileName = $"{Guid.NewGuid()}{extension}";
        var uploadsDir = Path.Combine(env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsDir);

        var filePath = Path.Combine(uploadsDir, fileName);
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var request = httpContextAccessor.HttpContext!.Request;
        return $"{request.Scheme}://{request.Host}/uploads/{fileName}";
    }
}
