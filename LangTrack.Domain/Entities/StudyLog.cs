namespace LangTrack.Domain.Entities;

public class StudyLog : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid WordId { get; set; }
    public DateTime StudiedAtUtc { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public User User { get; set; } = default!;
    public Word Word { get; set; } = default!;
}
