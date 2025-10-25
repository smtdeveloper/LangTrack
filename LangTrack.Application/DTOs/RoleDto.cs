namespace LangTrack.Application.DTOs;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<PermissionDto> Permissions { get; set; } = new();
}

public class PermissionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Resource { get; set; } = default!;
    public string Action { get; set; } = default!;
}

public class CreateRoleDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<Guid> PermissionIds { get; set; } = new();
}

public class UpdateUserRoleDto
{
    public Guid RoleId { get; set; }
}
