using Microsoft.EntityFrameworkCore;
using LangTrack.Domain.Entities;
using LangTrack.Infrastructure.Data;
using LangTrack.Application.Interfaces;

namespace LangTrack.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LangTrackDbContext _context;

    public UserRepository(LangTrackDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Words)
            .Include(u => u.StudyLogs)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Words)
            .Include(u => u.StudyLogs)
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
    }

    public async Task<User> CreateAsync(User user)
    {
        // Assign Student role if no role is specified
        if (user.RoleId == Guid.Empty)
        {
            var studentRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Student");
            if (studentRole != null)
            {
                user.RoleId = studentRole.Id;
            }
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email.ToLowerInvariant());
    }

    public async Task<bool> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
