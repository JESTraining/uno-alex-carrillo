using Microsoft.AspNetCore.Http;

namespace IssueTracker.Application.Abstractions;

/// <summary>
/// Abstraction for file storage operations
/// </summary>
public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, Guid issueId);
}
