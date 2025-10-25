using LangTrack.Application.DTOs;
using LangTrack.Application.Interfaces;
using LangTrack.Domain.Entities;

namespace LangTrack.Application.Services;

public class StudyLogService : IStudyLogService
{
    private readonly IStudyLogRepository _studyLogRepository;
    private readonly IWordRepository _wordRepository;

    public StudyLogService(IStudyLogRepository studyLogRepository, IWordRepository wordRepository)
    {
        _studyLogRepository = studyLogRepository;
        _wordRepository = wordRepository;
    }

    public async Task<StudyLogDto> CreateStudyLogAsync(CreateStudyLogDto createStudyLogDto, Guid userId)
    {
        // Check if word exists and belongs to user
        var word = await _wordRepository.GetByIdAsync(createStudyLogDto.WordId, userId);
        if (word == null)
        {
            throw new InvalidOperationException("Word not found");
        }

        var studyLog = new StudyLog
        {
            UserId = userId,
            WordId = createStudyLogDto.WordId,
            StudiedAtUtc = createStudyLogDto.StudiedAtUtc ?? DateTime.UtcNow
        };

        var createdStudyLog = await _studyLogRepository.CreateAsync(studyLog);
        return MapToDto(createdStudyLog);
    }

    public async Task<IEnumerable<StudyLogDto>> GetStudyLogsByWordIdAsync(Guid wordId, Guid userId)
    {
        var studyLogs = await _studyLogRepository.GetByWordIdAsync(wordId, userId);
        return studyLogs.Select(MapToDto);
    }

    private static StudyLogDto MapToDto(StudyLog studyLog)
    {
        return new StudyLogDto
        {
            Id = studyLog.Id,
            WordId = studyLog.WordId,
            StudiedAtUtc = studyLog.StudiedAtUtc,
            CreatedAt = studyLog.CreatedAt
        };
    }
}
