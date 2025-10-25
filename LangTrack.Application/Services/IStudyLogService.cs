using LangTrack.Application.DTOs;

namespace LangTrack.Application.Services;

public interface IStudyLogService
{
    Task<StudyLogDto> CreateStudyLogAsync(CreateStudyLogDto createStudyLogDto);
    Task<IEnumerable<StudyLogDto>> GetStudyLogsByWordIdAsync(Guid wordId);
}
