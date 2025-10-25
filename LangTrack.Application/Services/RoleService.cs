using LangTrack.Application.DTOs;
using LangTrack.Application.Interfaces;
using LangTrack.Domain.Entities;

namespace LangTrack.Application.Services;

public class RoleService : IRoleService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public RoleService(IUserRepository userRepository, IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Where(r => r.IsActive).Select(MapToRoleDto);
    }

    public async Task<RoleDto?> GetRoleByIdAsync(Guid roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        return role != null ? MapToRoleDto(role) : null;
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        var role = new Role
        {
            Name = createRoleDto.Name,
            Description = createRoleDto.Description
        };

        var createdRole = await _roleRepository.CreateAsync(role);
        return MapToRoleDto(createdRole);
    }

    public async Task<bool> UpdateUserRoleAsync(Guid userId, UpdateUserRoleDto updateUserRoleDto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        user.RoleId = updateUserRoleDto.RoleId;
        return await _userRepository.UpdateAsync(user);
    }

    public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
    {
        var permissions = await _permissionRepository.GetAllAsync();
        return permissions.Select(MapToPermissionDto);
    }

    private static RoleDto MapToRoleDto(Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt,
            Permissions = role.RolePermissions.Select(rp => MapToPermissionDto(rp.Permission)).ToList()
        };
    }

    private static PermissionDto MapToPermissionDto(Permission permission)
    {
        return new PermissionDto
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description,
            Resource = permission.Resource,
            Action = permission.Action
        };
    }
}
