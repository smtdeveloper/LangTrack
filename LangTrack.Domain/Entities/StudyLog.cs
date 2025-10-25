namespace LangTrack.Domain.Entities;

public class StudyLog : BaseEntity
{
    public Guid WordId { get; set; }
    public DateTime StudiedAtUtc { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Word Word { get; set; } = default!;
}
