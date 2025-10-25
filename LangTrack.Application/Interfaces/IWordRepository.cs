using LangTrack.Domain.Entities;

namespace LangTrack.Application.Interfaces;

public interface IWordRepository
{
    Task<Word?> GetByIdAsync(Guid id, Guid userId);
    Task<Word?> GetByTextAsync(string text, Guid userId);
    Task<Word?> GetRandomAsync(Guid userId);
    Task<IEnumerable<Word>> GetAllAsync(Guid userId, int page = 1, int pageSize = 20, string? searchQuery = null);
    Task<Word> CreateAsync(Word word);
    Task<bool> ExistsByTextAsync(string text, Guid userId);
    Task<int> GetTotalCountAsync(Guid userId, string? searchQuery = null);
    Task<bool> SoftDeleteAsync(Guid id, Guid userId);
    Task<bool> SoftDeleteByAdminAsync(Guid id);
    Task<Word?> GetByIdForAdminAsync(Guid id);
}
