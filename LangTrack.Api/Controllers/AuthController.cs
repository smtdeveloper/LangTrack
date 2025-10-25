using Microsoft.AspNetCore.Mvc;
using LangTrack.Application.DTOs;
using LangTrack.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace LangTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Yeni kullanıcı kaydı
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        // Validation
        var validationErrors = ValidateRegisterDto(registerDto);
        if (validationErrors.Any())
        {
            return BadRequest(new { error = "VALIDATION_ERROR", details = validationErrors });
        }

        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = "DUPLICATE", field = "Email", message = ex.Message });
        }
    }

    /// <summary>
    /// Kullanıcı girişi
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        // Validation
        var validationErrors = ValidateLoginDto(loginDto);
        if (validationErrors.Any())
        {
            return BadRequest(new { error = "VALIDATION_ERROR", details = validationErrors });
        }

        try
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = ex.Message });
        }
    }

    /// <summary>
    /// Kullanıcı bilgilerini getir (JWT token gerekli)
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = "Invalid token" });
        }

        var user = await _authService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { error = "NOT_FOUND", resource = "User", message = "User not found" });
        }

        return Ok(user);
    }

    private static List<string> ValidateRegisterDto(RegisterDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            errors.Add("Email is required");
        }
        else if (!IsValidEmail(dto.Email))
        {
            errors.Add("Email format is invalid");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            errors.Add("Password is required");
        }
        else if (dto.Password.Length < 6)
        {
            errors.Add("Password must be at least 6 characters");
        }

        if (string.IsNullOrWhiteSpace(dto.FirstName))
        {
            errors.Add("FirstName is required");
        }
        else if (dto.FirstName.Length < 2 || dto.FirstName.Length > 50)
        {
            errors.Add("FirstName must be between 2 and 50 characters");
        }

        if (string.IsNullOrWhiteSpace(dto.LastName))
        {
            errors.Add("LastName is required");
        }
        else if (dto.LastName.Length < 2 || dto.LastName.Length > 50)
        {
            errors.Add("LastName must be between 2 and 50 characters");
        }

        return errors;
    }

    private static List<string> ValidateLoginDto(LoginDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            errors.Add("Email is required");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            errors.Add("Password is required");
        }

        return errors;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
