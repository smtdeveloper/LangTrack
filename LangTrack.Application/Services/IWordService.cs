using LangTrack.Application.DTOs;

namespace LangTrack.Application.Services;

public interface IWordService
{
    Task<WordDto> CreateWordAsync(CreateWordDto createWordDto);
    Task<WordListDto> GetWordsAsync(int page = 1, int pageSize = 20, string? searchQuery = null);
    Task<WordDto?> GetRandomWordAsync();
    Task<WordDto?> GetWordByIdAsync(Guid id);
}
