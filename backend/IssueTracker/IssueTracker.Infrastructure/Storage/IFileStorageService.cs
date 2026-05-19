using Microsoft.AspNetCore.Http;

namespace IssueTracker.Infrastructure.Storage;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(
        IFormFile file,
        Guid issueId);
}