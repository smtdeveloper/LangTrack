using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LangTrack.Application.DTOs;
using LangTrack.Application.Services;
using LangTrack.Api.Validators;
using LangTrack.Api.Attributes;

namespace LangTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class WordsController : ControllerBase
{
    private readonly IWordService _wordService;

    public WordsController(IWordService wordService)
    {
        _wordService = wordService;
    }

    /// <summary>
    /// Yeni kelime ekler
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<WordDto>> CreateWord([FromBody] CreateWordDto createWordDto)
    {
        // Get current user ID
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = "Invalid token" });
        }

        // Validation
        var validationErrors = CreateWordValidator.Validate(createWordDto);
        if (validationErrors.Any())
        {
            return BadRequest(new { error = "VALIDATION_ERROR", details = validationErrors });
        }

        try
        {
            var word = await _wordService.CreateWordAsync(createWordDto, userId.Value);
            return CreatedAtAction(nameof(GetWordById), new { id = word.Id }, word);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = "DUPLICATE", field = "Text", message = ex.Message });
        }
    }

    /// <summary>
    /// Kelimeleri listeler (sayfalama ve arama ile)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<WordListDto>> GetWords(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? q = null)
    {
        // Get current user ID
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = "Invalid token" });
        }

        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        var words = await _wordService.GetWordsAsync(userId.Value, page, pageSize, q);
        return Ok(words);
    }

    /// <summary>
    /// Rastgele kelime getirir
    /// </summary>
    [HttpGet("random")]
    public async Task<ActionResult<WordDto>> GetRandomWord()
    {
        // Get current user ID
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = "Invalid token" });
        }

        var word = await _wordService.GetRandomWordAsync(userId.Value);
        if (word == null)
        {
            return NotFound(new { error = "NOT_FOUND", resource = "Word", message = "No words found" });
        }

        return Ok(word);
    }

    /// <summary>
    /// ID'ye göre kelime getirir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<WordDto>> GetWordById(Guid id)
    {
        // Get current user ID
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = "Invalid token" });
        }

        var word = await _wordService.GetWordByIdAsync(id, userId.Value);
        if (word == null)
        {
            return NotFound(new { error = "NOT_FOUND", resource = "Word", message = "Word not found" });
        }

        return Ok(word);
    }

    /// <summary>
    /// Kelime siler (soft delete) - Kullanıcı sadece kendi kelimelerini, admin tüm kelimeleri silebilir
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWord(Guid id)
    {
        // Get current user ID
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = "Invalid token" });
        }

        // Check if user is admin
        var isAdmin = User.IsInRole("Admin");
        
        bool result;
        if (isAdmin)
        {
            // Admin can delete any word
            result = await _wordService.DeleteWordByAdminAsync(id);
        }
        else
        {
            // Regular user can only delete their own words
            result = await _wordService.DeleteWordAsync(id, userId.Value);
        }

        if (!result)
        {
            return NotFound(new { error = "NOT_FOUND", resource = "Word", message = "Word not found or already deleted" });
        }

        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
