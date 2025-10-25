namespace LangTrack.Domain.Entities;

public class Word : BaseEntity
{
    public string Text { get; set; } = default!;
    public string Meaning { get; set; } = default!;
    public string? Example { get; set; }
    public string? Tags { get; set; } // "phrasal,verb,b2" gibi
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}