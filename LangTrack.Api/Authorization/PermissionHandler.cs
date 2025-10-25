using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LangTrack.Infrastructure.Data;

namespace LangTrack.Api.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly LangTrackDbContext _context;

    public PermissionHandler(LangTrackDbContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst("userId")?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return;
        }

        var user = await _context.Users
            .Include(u => u.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return;
        }

        var hasPermission = user.Role.RolePermissions
            .Any(rp => rp.Permission.Resource == requirement.Resource && 
                      rp.Permission.Action == requirement.Action);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}
