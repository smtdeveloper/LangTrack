using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LangTrack.Application.DTOs;
using LangTrack.Application.Services;
using LangTrack.Api.Attributes;

namespace LangTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Tüm rolleri listeler
    /// </summary>
    [HttpGet]
    [RequirePermission("roles", "read")]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    /// <summary>
    /// ID'ye göre rol getirir
    /// </summary>
    [HttpGet("{id}")]
    [RequirePermission("roles", "read")]
    public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        if (role == null)
        {
            return NotFound(new { error = "NOT_FOUND", resource = "Role", message = "Role not found" });
        }

        return Ok(role);
    }

    /// <summary>
    /// Yeni rol oluşturur
    /// </summary>
    [HttpPost]
    [RequirePermission("roles", "create")]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        // Validation
        var validationErrors = ValidateCreateRoleDto(createRoleDto);
        if (validationErrors.Any())
        {
            return BadRequest(new { error = "VALIDATION_ERROR", details = validationErrors });
        }

        try
        {
            var role = await _roleService.CreateRoleAsync(createRoleDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = "INVALID_OPERATION", message = ex.Message });
        }
    }

    /// <summary>
    /// Kullanıcının rolünü günceller
    /// </summary>
    [HttpPut("users/{userId}/role")]
    [RequirePermission("users", "update")]
    public async Task<ActionResult> UpdateUserRole(Guid userId, [FromBody] UpdateUserRoleDto updateUserRoleDto)
    {
        var success = await _roleService.UpdateUserRoleAsync(userId, updateUserRoleDto);
        if (!success)
        {
            return NotFound(new { error = "NOT_FOUND", resource = "User", message = "User not found" });
        }

        return Ok(new { message = "User role updated successfully" });
    }

    /// <summary>
    /// Tüm izinleri listeler
    /// </summary>
    [HttpGet("permissions")]
    [RequirePermission("permissions", "read")]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions()
    {
        var permissions = await _roleService.GetAllPermissionsAsync();
        return Ok(permissions);
    }

    private static List<string> ValidateCreateRoleDto(CreateRoleDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            errors.Add("Name is required");
        }
        else if (dto.Name.Length < 2 || dto.Name.Length > 50)
        {
            errors.Add("Name must be between 2 and 50 characters");
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            errors.Add("Description is required");
        }
        else if (dto.Description.Length < 5 || dto.Description.Length > 200)
        {
            errors.Add("Description must be between 5 and 200 characters");
        }

        return errors;
    }
}
