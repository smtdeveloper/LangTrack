using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LangTrack.Application.DTOs;
using LangTrack.Application.Services;

namespace LangTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;

    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    /// <summary>
    /// İstatistikleri getirir (toplam kelime, bugün çalışılan, streak)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<StatsDto>> GetStats()
    {
        // Get current user ID
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = "UNAUTHORIZED", message = "Invalid token" });
        }

        var stats = await _statsService.GetStatsAsync(userId.Value);
        return Ok(stats);
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
