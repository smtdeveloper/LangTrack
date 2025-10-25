using LangTrack.Domain.Entities;

namespace LangTrack.Application.Interfaces;

public interface IStudyLogRepository
{
    Task<StudyLog> CreateAsync(StudyLog studyLog);
    Task<IEnumerable<StudyLog>> GetByWordIdAsync(Guid wordId, Guid userId);
    Task<IEnumerable<StudyLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid userId);
    Task<int> GetStudiedTodayCountAsync(Guid userId);
    Task<int> GetStreakDaysAsync(Guid userId);
}
