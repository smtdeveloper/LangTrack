using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LangTrack.Application.DTOs;
using LangTrack.Application.Services;

namespace LangTrack.Api.Controllers;

[ApiController]
[Route("api/v1/study")]
[Authorize]
public class StudyController : ControllerBase
{
    private readonly IStudyLogService _studyLogService;

    public StudyController(IStudyLogService studyLogService)
    {
        _studyLogService = studyLogService;
    }

    /// <summary>
    /// Kelime çalışma kaydı ekler
    /// </summary>
    [HttpPost("{wordId}")]
    public async Task<ActionResult<StudyLogDto>> CreateStudyLog(Guid wordId)
    {
        // Get current user ID
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = "Invalid token" });
        }

        try
        {
            var createStudyLogDto = new CreateStudyLogDto
            {
                WordId = wordId,
                StudiedAtUtc = DateTime.UtcNow
            };

            var studyLog = await _studyLogService.CreateStudyLogAsync(createStudyLogDto, userId.Value);
            return Ok(studyLog);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = "NOT_FOUND", resource = "Word", message = ex.Message });
        }
    }

    /// <summary>
    /// Belirli bir kelime için çalışma kayıtlarını getirir
    /// </summary>
    [HttpGet("{wordId}/logs")]
    public async Task<ActionResult<IEnumerable<StudyLogDto>>> GetStudyLogsByWordId(Guid wordId)
    {
        // Get current user ID
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = "Invalid token" });
        }

        var studyLogs = await _studyLogService.GetStudyLogsByWordIdAsync(wordId, userId.Value);
        return Ok(studyLogs);
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
