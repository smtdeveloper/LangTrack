using Microsoft.AspNetCore.Mvc;
using LangTrack.Application.DTOs;
using LangTrack.Application.Services;

namespace LangTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
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
        var stats = await _statsService.GetStatsAsync();
        return Ok(stats);
    }
}
