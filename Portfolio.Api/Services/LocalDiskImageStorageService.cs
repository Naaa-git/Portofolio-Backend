namespace Portfolio.Api.Services;

/// <summary>
/// Dev-only mock of blob storage: saves files under wwwroot/uploads and serves them as static files.
/// Swap this for an Azure Blob Storage implementation later by registering it in Program.cs instead —
/// IImageStorageService's contract stays the same, so no controller/service code needs to change.
/// </summary>
public class LocalDiskImageStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    : IImageStorageService
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    public async Task<string> UploadAsync(IFormFile file)
    {
        if (file.Length == 0)
            throw new ArgumentException("File is empty.");

        if (file.Length > MaxFileSizeBytes)
            throw new ArgumentException("File exceeds the 5 MB limit.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
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
