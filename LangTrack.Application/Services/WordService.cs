using LangTrack.Application.DTOs;
using LangTrack.Application.Interfaces;
using LangTrack.Domain.Entities;

namespace LangTrack.Application.Services;

public class WordService : IWordService
{
    private readonly IWordRepository _wordRepository;

    public WordService(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    public async Task<WordDto> CreateWordAsync(CreateWordDto createWordDto, Guid userId)
    {
        // Check if word already exists for this user
        if (await _wordRepository.ExistsByTextAsync(createWordDto.Text, userId))
        {
            throw new InvalidOperationException("Word with this text already exists");
        }

        var word = new Word
        {
            UserId = userId,
            Text = createWordDto.Text.Trim(),
            Meaning = createWordDto.Meaning.Trim(),
            Example = createWordDto.Example?.Trim(),
            Tags = createWordDto.Tags?.Trim()
        };

        var createdWord = await _wordRepository.CreateAsync(word);
        return MapToDto(createdWord);
    }

    public async Task<WordListDto> GetWordsAsync(Guid userId, int page = 1, int pageSize = 20, string? searchQuery = null)
    {
        var words = await _wordRepository.GetAllAsync(userId, page, pageSize, searchQuery);
        var totalCount = await _wordRepository.GetTotalCountAsync(userId, searchQuery);

        return new WordListDto
        {
            Words = words.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<WordListDto> GetAllWordsAsync(int page = 1, int pageSize = 20, string? searchQuery = null, Guid? userId = null)
    {
        var words = await _wordRepository.GetAllWordsAsync(page, pageSize, searchQuery, userId);
        var totalCount = await _wordRepository.GetAllWordsTotalCountAsync(searchQuery, userId);

        return new WordListDto
        {
            Words = words.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<WordDto?> GetRandomWordAsync(Guid userId)
    {
        var word = await _wordRepository.GetRandomAsync(userId);
        return word != null ? MapToDto(word) : null;
    }

    public async Task<WordDto?> GetRandomGlobalWordAsync()
    {
        var word = await _wordRepository.GetRandomGlobalAsync();
        return word != null ? MapToDto(word) : null;
    }

    public async Task<WordDto?> GetWordByIdAsync(Guid id, Guid userId)
    {
        var word = await _wordRepository.GetByIdAsync(id, userId);
        return word != null ? MapToDto(word) : null;
    }

    public async Task<bool> DeleteWordAsync(Guid id, Guid userId)
    {
        return await _wordRepository.SoftDeleteAsync(id, userId);
    }

    public async Task<bool> DeleteWordByAdminAsync(Guid id)
    {
        return await _wordRepository.SoftDeleteByAdminAsync(id);
    }

    private static WordDto MapToDto(Word word)
    {
        return new WordDto
        {
            Id = word.Id,
            Text = word.Text,
            Meaning = word.Meaning,
            Example = word.Example,
            Tags = word.Tags,
            CreatedAt = word.CreatedAt,
            UpdatedAt = word.UpdatedAt,
            UserId = word.UserId,
            UserEmail = word.User?.Email,
            UserName = word.User != null ? $"{word.User.FirstName} {word.User.LastName}" : null
        };
    }
}
