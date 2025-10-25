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

    public async Task<Word?> GetByIdAsync(Guid id)
    {
        return await _context.Words
            .Include(w => w.StudyLogs)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Word?> GetByTextAsync(string text)
    {
        return await _context.Words
            .Include(w => w.StudyLogs)
            .FirstOrDefaultAsync(w => w.Text.ToLower() == text.ToLower());
    }

    public async Task<Word?> GetRandomAsync()
    {
        var count = await _context.Words.CountAsync();
        if (count == 0) return null;

        var random = new Random();
        var skip = random.Next(0, count);
        
        return await _context.Words
            .Include(w => w.StudyLogs)
            .Skip(skip)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Word>> GetAllAsync(int page = 1, int pageSize = 20, string? searchQuery = null)
    {
        var query = _context.Words.AsQueryable();

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

    public async Task<Word> CreateAsync(Word word)
    {
        _context.Words.Add(word);
        await _context.SaveChangesAsync();
        return word;
    }

    public async Task<bool> ExistsByTextAsync(string text)
    {
        return await _context.Words
            .AnyAsync(w => w.Text.ToLower() == text.ToLower());
    }

    public async Task<int> GetTotalCountAsync(string? searchQuery = null)
    {
        var query = _context.Words.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(w => 
                w.Text.Contains(searchQuery) || 
                w.Meaning.Contains(searchQuery));
        }

        return await query.CountAsync();
    }
}
