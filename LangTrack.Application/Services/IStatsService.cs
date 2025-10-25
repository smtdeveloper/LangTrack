using LangTrack.Application.DTOs;

namespace LangTrack.Application.Services;

public interface IStatsService
{
    Task<StatsDto> GetStatsAsync();
}
