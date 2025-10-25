using Microsoft.AspNetCore.Mvc;
using LangTrack.Application.DTOs;
using LangTrack.Application.Services;

namespace LangTrack.Api.Controllers;

[ApiController]
[Route("api/v1/study")]
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
        try
        {
            var createStudyLogDto = new CreateStudyLogDto
            {
                WordId = wordId,
                StudiedAtUtc = DateTime.UtcNow
            };

            var studyLog = await _studyLogService.CreateStudyLogAsync(createStudyLogDto);
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
        var studyLogs = await _studyLogService.GetStudyLogsByWordIdAsync(wordId);
        return Ok(studyLogs);
    }
}
