using System.ComponentModel.DataAnnotations;

namespace LangTrack.Domain.Entities;

public class User : BaseEntity
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
    
    [Required]
    [MinLength(6)]
    public string PasswordHash { get; set; } = default!;
    
    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string FirstName { get; set; } = default!;
    
    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string LastName { get; set; } = default!;
    
    public bool IsActive { get; set; } = true;
    public Guid RoleId { get; set; }
    
    // Navigation properties
    public Role Role { get; set; } = default!;
    public ICollection<Word> Words { get; set; } = new List<Word>();
    public ICollection<StudyLog> StudyLogs { get; set; } = new List<StudyLog>();
}
