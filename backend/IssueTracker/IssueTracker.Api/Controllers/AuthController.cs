using IssueTracker.Api.DTOs;
using IssueTracker.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ITokenService tokenService) : ControllerBase
{
    // Test user hardcoded para demostración
    private const string TestUserId = "12345678-1234-1234-1234-123456789012";
    private const string TestUserEmail = "test@issuetracker.com";
    private const string TestUserPassword = "test123";
    private const string TestUserName = "Test User";

    /// <summary>
    /// Login with test credentials
    /// </summary>
    /// <param name="loginDto">Login credentials</param>
    /// <returns>JWT token</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login(LoginDto loginDto)
    {
        // Simple hardcoded test user validation
        if (loginDto.Email != TestUserEmail || loginDto.Password != TestUserPassword)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        // Generate JWT token
        var token = tokenService.GenerateToken(TestUserId, TestUserEmail, TestUserName);
        var expirationMinutes = 60;

        return Ok(new TokenResponseDto
        {
            AccessToken = token,
            ExpiresIn = expirationMinutes * 60,
            User = new UserInfoDto
            {
                Id = TestUserId,
                Email = TestUserEmail,
                Name = TestUserName
            }
        });
    }

    /// <summary>
    /// Get test user credentials for demonstration
    /// </summary>
    /// <returns>Test user info</returns>
    [HttpGet("test-credentials")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult GetTestCredentials()
    {
        return Ok(new
        {
            email = TestUserEmail,
            password = TestUserPassword,
            message = "Use these credentials to test the API"
        });
    }
}
