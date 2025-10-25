namespace LangTrack.Application.DTOs;

public class StudyLogDto
{
    public Guid Id { get; set; }
    public Guid WordId { get; set; }
    public DateTime StudiedAtUtc { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateStudyLogDto
{
    public Guid WordId { get; set; }
    public DateTime? StudiedAtUtc { get; set; }
}
