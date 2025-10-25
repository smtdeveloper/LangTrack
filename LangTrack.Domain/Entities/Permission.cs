namespace LangTrack.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Resource { get; set; } = default!; // "words", "users", "stats" etc.
    public string Action { get; set; } = default!; // "create", "read", "update", "delete"
    
    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
