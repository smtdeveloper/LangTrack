namespace LangTrack.Application.DTOs;

public class WordDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
    public string Meaning { get; set; } = default!;
    public string? Example { get; set; }
    public string? Tags { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateWordDto
{
    public string Text { get; set; } = default!;
    public string Meaning { get; set; } = default!;
    public string? Example { get; set; }
    public string? Tags { get; set; }
}

public class WordListDto
{
    public IEnumerable<WordDto> Words { get; set; } = new List<WordDto>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
