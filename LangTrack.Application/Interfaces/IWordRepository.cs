using LangTrack.Domain.Entities;

namespace LangTrack.Application.Interfaces;

public interface IWordRepository
{
    Task<Word?> GetByIdAsync(Guid id);
    Task<Word?> GetByTextAsync(string text);
    Task<Word?> GetRandomAsync();
    Task<IEnumerable<Word>> GetAllAsync(int page = 1, int pageSize = 20, string? searchQuery = null);
    Task<Word> CreateAsync(Word word);
    Task<bool> ExistsByTextAsync(string text);
    Task<int> GetTotalCountAsync(string? searchQuery = null);
}
