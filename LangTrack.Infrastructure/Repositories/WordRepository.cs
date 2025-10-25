using Microsoft.EntityFrameworkCore;
using LangTrack.Domain.Entities;
using LangTrack.Infrastructure.Data;
using LangTrack.Application.Interfaces;

namespace LangTrack.Infrastructure.Repositories;

public class WordRepository : IWordRepository
{
    private readonly LangTrackDbContext _context;

    public WordRepository(LangTrackDbContext context)
    {
        _context = context;
    }

    public async Task<Word?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.Words
            .Include(w => w.StudyLogs)
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId && !w.IsDeleted);
    }

    public async Task<Word?> GetByTextAsync(string text, Guid userId)
    {
        return await _context.Words
            .Include(w => w.StudyLogs)
            .FirstOrDefaultAsync(w => w.Text.ToLower() == text.ToLower() && w.UserId == userId && !w.IsDeleted);
    }

    public async Task<Word?> GetRandomAsync(Guid userId)
    {
        var count = await _context.Words.CountAsync(w => w.UserId == userId && !w.IsDeleted);
        if (count == 0) return null;

        var random = new Random();
        var skip = random.Next(0, count);
        
        return await _context.Words
            .Include(w => w.StudyLogs)
            .Where(w => w.UserId == userId && !w.IsDeleted)
            .Skip(skip)
            .FirstOrDefaultAsync();
    }

    public async Task<Word?> GetRandomGlobalAsync()
    {
        var count = await _context.Words.CountAsync(w => !w.IsDeleted);
        if (count == 0) return null;

        var random = new Random();
        var skip = random.Next(0, count);
        
        return await _context.Words
            .Include(w => w.User)
            .Include(w => w.StudyLogs)
            .Where(w => !w.IsDeleted)
            .Skip(skip)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Word>> GetAllAsync(Guid userId, int page = 1, int pageSize = 20, string? searchQuery = null)
    {
        var query = _context.Words.Where(w => w.UserId == userId && !w.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(w => 
                w.Text.Contains(searchQuery) || 
                w.Meaning.Contains(searchQuery));
        }

        return await query
            .Include(w => w.StudyLogs)
            .OrderBy(w => w.Text)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Word>> GetAllWordsAsync(int page = 1, int pageSize = 20, string? searchQuery = null, Guid? userId = null)
    {
        var query = _context.Words
            .Include(w => w.User)
            .Include(w => w.StudyLogs)
            .Where(w => !w.IsDeleted);

        // Belirli bir kullanıcının kelimelerini filtrele
        if (userId.HasValue)
        {
            query = query.Where(w => w.UserId == userId.Value);
        }

        // Arama sorgusu
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(w => 
                w.Text.Contains(searchQuery) || 
                w.Meaning.Contains(searchQuery));
        }

        return await query
            .OrderBy(w => w.Text)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Word> CreateAsync(Word word)
    {
        _context.Words.Add(word);
        await _context.SaveChangesAsync();
        return word;
    }

    public async Task<bool> ExistsByTextAsync(string text, Guid userId)
    {
        return await _context.Words
            .AnyAsync(w => w.Text.ToLower() == text.ToLower() && w.UserId == userId && !w.IsDeleted);
    }

    public async Task<int> GetTotalCountAsync(Guid userId, string? searchQuery = null)
    {
        var query = _context.Words.Where(w => w.UserId == userId && !w.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(w => 
                w.Text.Contains(searchQuery) || 
                w.Meaning.Contains(searchQuery));
        }

        return await query.CountAsync();
    }

    public async Task<int> GetAllWordsTotalCountAsync(string? searchQuery = null, Guid? userId = null)
    {
        var query = _context.Words.Where(w => !w.IsDeleted);

        // Belirli bir kullanıcının kelimelerini filtrele
        if (userId.HasValue)
        {
            query = query.Where(w => w.UserId == userId.Value);
        }

        // Arama sorgusu
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(w => 
                w.Text.Contains(searchQuery) || 
                w.Meaning.Contains(searchQuery));
        }

        return await query.CountAsync();
    }

    public async Task<bool> SoftDeleteAsync(Guid id, Guid userId)
    {
        var word = await _context.Words
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId && !w.IsDeleted);
        
        if (word == null) return false;
        
        word.IsDeleted = true;
        word.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteByAdminAsync(Guid id)
    {
        var word = await _context.Words
            .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        
        if (word == null) return false;
        
        word.IsDeleted = true;
        word.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Word?> GetByIdForAdminAsync(Guid id)
    {
        return await _context.Words
            .Include(w => w.StudyLogs)
            .FirstOrDefaultAsync(w => w.Id == id);
    }
}
