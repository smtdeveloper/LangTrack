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

    public async Task<WordDto> CreateWordAsync(CreateWordDto createWordDto)
    {
        // Check if word already exists
        if (await _wordRepository.ExistsByTextAsync(createWordDto.Text))
        {
            throw new InvalidOperationException("Word with this text already exists");
        }

        var word = new Word
        {
            Text = createWordDto.Text.Trim(),
            Meaning = createWordDto.Meaning.Trim(),
            Example = createWordDto.Example?.Trim(),
            Tags = createWordDto.Tags?.Trim()
        };

        var createdWord = await _wordRepository.CreateAsync(word);
        return MapToDto(createdWord);
    }

    public async Task<WordListDto> GetWordsAsync(int page = 1, int pageSize = 20, string? searchQuery = null)
    {
        var words = await _wordRepository.GetAllAsync(page, pageSize, searchQuery);
        var totalCount = await _wordRepository.GetTotalCountAsync(searchQuery);

        return new WordListDto
        {
            Words = words.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<WordDto?> GetRandomWordAsync()
    {
        var word = await _wordRepository.GetRandomAsync();
        return word != null ? MapToDto(word) : null;
    }

    public async Task<WordDto?> GetWordByIdAsync(Guid id)
    {
        var word = await _wordRepository.GetByIdAsync(id);
        return word != null ? MapToDto(word) : null;
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
            UpdatedAt = word.UpdatedAt
        };
    }
}
