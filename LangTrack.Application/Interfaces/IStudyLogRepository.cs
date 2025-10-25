using LangTrack.Domain.Entities;

namespace LangTrack.Application.Interfaces;

public interface IStudyLogRepository
{
    Task<StudyLog> CreateAsync(StudyLog studyLog);
    Task<IEnumerable<StudyLog>> GetByWordIdAsync(Guid wordId);
    Task<IEnumerable<StudyLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<int> GetStudiedTodayCountAsync();
    Task<int> GetStreakDaysAsync();
}
