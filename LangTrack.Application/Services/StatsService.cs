using LangTrack.Application.DTOs;
using LangTrack.Application.Interfaces;

namespace LangTrack.Application.Services;

public class StatsService : IStatsService
{
    private readonly IWordRepository _wordRepository;
    private readonly IStudyLogRepository _studyLogRepository;

    public StatsService(IWordRepository wordRepository, IStudyLogRepository studyLogRepository)
    {
        _wordRepository = wordRepository;
        _studyLogRepository = studyLogRepository;
    }

    public async Task<StatsDto> GetStatsAsync(Guid userId)
    {
        var totalWords = await _wordRepository.GetTotalCountAsync(userId);
        var studiedToday = await _studyLogRepository.GetStudiedTodayCountAsync(userId);
        var streakDays = await _studyLogRepository.GetStreakDaysAsync(userId);

        return new StatsDto
        {
            TotalWords = totalWords,
            StudiedToday = studiedToday,
            StreakDays = streakDays
        };
    }
}
