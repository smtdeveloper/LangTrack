using Microsoft.EntityFrameworkCore;
using LangTrack.Domain.Entities;
using LangTrack.Infrastructure.Data;
using LangTrack.Application.Interfaces;

namespace LangTrack.Infrastructure.Repositories;

public class StudyLogRepository : IStudyLogRepository
{
    private readonly LangTrackDbContext _context;

    public StudyLogRepository(LangTrackDbContext context)
    {
        _context = context;
    }

    public async Task<StudyLog> CreateAsync(StudyLog studyLog)
    {
        _context.StudyLogs.Add(studyLog);
        await _context.SaveChangesAsync();
        return studyLog;
    }

    public async Task<IEnumerable<StudyLog>> GetByWordIdAsync(Guid wordId)
    {
        return await _context.StudyLogs
            .Where(sl => sl.WordId == wordId)
            .OrderByDescending(sl => sl.StudiedAtUtc)
            .ToListAsync();
    }

    public async Task<IEnumerable<StudyLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.StudyLogs
            .Where(sl => sl.StudiedAtUtc >= startDate && sl.StudiedAtUtc <= endDate)
            .OrderByDescending(sl => sl.StudiedAtUtc)
            .ToListAsync();
    }

    public async Task<int> GetStudiedTodayCountAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        
        return await _context.StudyLogs
            .CountAsync(sl => sl.StudiedAtUtc >= today && sl.StudiedAtUtc < tomorrow);
    }

    public async Task<int> GetStreakDaysAsync()
    {
        var today = DateTime.UtcNow.Date;
        var streakDays = 0;
        
        // Get all unique study dates, ordered by date descending
        var studyDates = await _context.StudyLogs
            .Select(sl => sl.StudiedAtUtc.Date)
            .Distinct()
            .OrderByDescending(date => date)
            .ToListAsync();

        // Calculate streak by checking consecutive days
        var currentDate = today;
        
        foreach (var studyDate in studyDates)
        {
            if (studyDate == currentDate)
            {
                streakDays++;
                currentDate = currentDate.AddDays(-1);
            }
            else if (studyDate < currentDate)
            {
                // Gap found, streak ends
                break;
            }
        }

        return streakDays;
    }
}
