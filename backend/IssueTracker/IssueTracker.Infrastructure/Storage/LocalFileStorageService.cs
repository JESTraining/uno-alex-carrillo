using Microsoft.AspNetCore.Http;
using IssueTracker.Domain.Exceptions;
using IAppFileStorageService = IssueTracker.Application.Abstractions.IFileStorageService;

namespace IssueTracker.Infrastructure.Storage;

public class LocalFileStorageService
    : IAppFileStorageService
{
    public async Task<string> SaveFileAsync(
        IFormFile file,
        Guid issueId)
    {
        const long maxFileSize = 5 * 1024 * 1024;
        var allowedExtensions = new[]
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        if (file.Length > maxFileSize)
        {
            throw new BadRequestException(
                "File size cannot exceed 5 MB.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            throw new BadRequestException(
                "Invalid file type.");
        }

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "uploads",
            issueId.ToString());

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName =
            $"{Guid.NewGuid()}_{file.FileName}";

        var filePath = Path.Combine(
            uploadsFolder,
            fileName);

        await using var stream =
            new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);

        return filePath;
    }
}
