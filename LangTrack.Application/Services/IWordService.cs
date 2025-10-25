using LangTrack.Application.DTOs;

namespace LangTrack.Application.Services;

public interface IWordService
{
    Task<WordDto> CreateWordAsync(CreateWordDto createWordDto, Guid userId);
    Task<WordListDto> GetWordsAsync(Guid userId, int page = 1, int pageSize = 20, string? searchQuery = null);
    Task<WordListDto> GetAllWordsAsync(int page = 1, int pageSize = 20, string? searchQuery = null, Guid? userId = null);
    Task<WordDto?> GetRandomWordAsync(Guid userId);
    Task<WordDto?> GetRandomGlobalWordAsync();
    Task<WordDto?> GetWordByIdAsync(Guid id, Guid userId);
    Task<bool> DeleteWordAsync(Guid id, Guid userId);
    Task<bool> DeleteWordByAdminAsync(Guid id);
}
