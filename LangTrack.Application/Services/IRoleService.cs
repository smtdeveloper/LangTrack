using LangTrack.Application.DTOs;

namespace LangTrack.Application.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleDto?> GetRoleByIdAsync(Guid roleId);
    Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
    Task<bool> UpdateUserRoleAsync(Guid userId, UpdateUserRoleDto updateUserRoleDto);
    Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
}
