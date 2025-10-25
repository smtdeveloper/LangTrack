namespace LangTrack.Domain.Entities;

public class Word : BaseEntity
{
    public Guid UserId { get; set; }
    public string Text { get; set; } = default!;
    public string Meaning { get; set; } = default!;
    public string? Example { get; set; }
    public string? Tags { get; set; } // "phrasal,verb,b2" gibi
    
    // Navigation properties
    public User User { get; set; } = default!;
    public ICollection<StudyLog> StudyLogs { get; set; } = new List<StudyLog>();
}