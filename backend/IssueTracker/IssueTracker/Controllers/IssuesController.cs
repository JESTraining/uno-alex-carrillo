using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;

namespace IssueTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IssuesController(IIssueService issueService) : ControllerBase
{
    /// <summary>
    /// Gets a paginated list of issues with optional status filtering
    /// </summary>
    /// <param name="page">Page number (default 1)</param>
    /// <param name="pageSize">Number of items per page (default 10, max 100)</param>
    /// <param name="status">Filter by status: Open, InProgress, or Closed (optional)</param>
    /// <returns>Paginated list of issues</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<IssueDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PaginatedResponseDto<IssueDto>>> GetIssues(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null)
    {
        var result = await issueService.GetIssuesAsync(page, pageSize, status);
        return Ok(result);
    }

    /// <summary>
    /// Gets a single issue by ID
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <returns>Issue details</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IssueDto>> GetIssue(Guid id)
    {
        var result = await issueService.GetIssueByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new issue
    /// </summary>
    /// <param name="createIssueDto">Issue creation details</param>
    /// <returns>Created issue</returns>
    [HttpPost]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IssueDto>> CreateIssue(CreateIssueDto createIssueDto)
    {
        var result = await issueService.CreateIssueAsync(createIssueDto);
        return CreatedAtAction(nameof(GetIssue), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing issue
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <param name="updateIssueDto">Update details (optional fields)</param>
    /// <returns>Updated issue</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IssueDto>> UpdateIssue(Guid id, UpdateIssueDto updateIssueDto)
    {
        var result = await issueService.UpdateIssueAsync(id, updateIssueDto);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an issue
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteIssue(Guid id)
    {
        await issueService.DeleteIssueAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Uploads an attachment to an issue (max 5MB, images only)
    /// </summary>
    /// <param name="issueId">Issue ID</param>
    /// <param name="file">File to upload (jpg, jpeg, png)</param>
    /// <returns>Attachment details</returns>
    [HttpPost("{issueId:guid}/attachments")]
    [ProducesResponseType(typeof(AttachmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [RequestSizeLimit(5 * 1024 * 1024)] // 5MB
    public async Task<ActionResult<AttachmentDto>> UploadAttachment(
        Guid issueId,
        IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is required");
        }

        var result = await issueService.UploadAttachmentAsync(issueId, file);
        return CreatedAtAction(nameof(DownloadAttachment), 
            new { issueId, fileId = result.Id }, result);
    }

    /// <summary>
    /// Downloads an attachment from an issue
    /// </summary>
    /// <param name="issueId">Issue ID</param>
    /// <param name="fileId">Attachment ID</param>
    /// <returns>File stream</returns>
    [HttpGet("{issueId:guid}/attachments/{fileId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DownloadAttachment(Guid issueId, Guid fileId)
    {
        var fileStream = await issueService.DownloadAttachmentAsync(issueId, fileId);

        // Return file with proper content type
        return File(fileStream, "application/octet-stream", 
            $"attachment_{fileId}");
    }

    /// <summary>
    /// Deletes an attachment from an issue
    /// </summary>
    /// <param name="issueId">Issue ID</param>
    /// <param name="fileId">Attachment ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{issueId:guid}/attachments/{fileId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteAttachment(Guid issueId, Guid fileId)
    {
        await issueService.DeleteAttachmentAsync(issueId, fileId);
        return NoContent();
    }
}
