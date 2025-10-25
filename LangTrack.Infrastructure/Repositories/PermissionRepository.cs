using Microsoft.EntityFrameworkCore;
using LangTrack.Application.Interfaces;
using LangTrack.Domain.Entities;
using LangTrack.Infrastructure.Data;

namespace LangTrack.Infrastructure.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly LangTrackDbContext _context;

    public PermissionRepository(LangTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Permission>> GetAllAsync()
    {
        return await _context.Permissions.ToListAsync();
    }

    public async Task<Permission?> GetByIdAsync(Guid id)
    {
        return await _context.Permissions.FindAsync(id);
    }

    public async Task<Permission> CreateAsync(Permission permission)
    {
        _context.Permissions.Add(permission);
        await _context.SaveChangesAsync();
        return permission;
    }

    public async Task<bool> UpdateAsync(Permission permission)
    {
        _context.Permissions.Update(permission);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission == null) return false;

        _context.Permissions.Remove(permission);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
