using LangTrack.Domain.Entities;
using LangTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LangTrack.Api.Data;

public static class SeedData
{
    public static async Task SeedAsync(LangTrackDbContext context)
    {
        // Seed Roles and Permissions first
        await SeedRolesAndPermissionsAsync(context);
        
        // Seed test user and A1 words
        await SeedTestUserAndWordsAsync(context);
    }

    private static async Task SeedRolesAndPermissionsAsync(LangTrackDbContext context)
    {
        if (context.Roles.Any()) return; // Already seeded

        // Create Permissions
        var permissions = new List<Permission>
        {
            // Words permissions
            new Permission { Name = "Create Words", Description = "Create new words", Resource = "words", Action = "create" },
            new Permission { Name = "Read Words", Description = "View words", Resource = "words", Action = "read" },
            new Permission { Name = "Update Words", Description = "Update words", Resource = "words", Action = "update" },
            new Permission { Name = "Delete Words", Description = "Delete words", Resource = "words", Action = "delete" },
            
            // Study permissions
            new Permission { Name = "Create Study Logs", Description = "Create study logs", Resource = "study", Action = "create" },
            new Permission { Name = "Read Study Logs", Description = "View study logs", Resource = "study", Action = "read" },
            
            // Stats permissions
            new Permission { Name = "Read Stats", Description = "View statistics", Resource = "stats", Action = "read" },
            
            // User permissions
            new Permission { Name = "Read Users", Description = "View users", Resource = "users", Action = "read" },
            new Permission { Name = "Update Users", Description = "Update users", Resource = "users", Action = "update" },
            new Permission { Name = "Delete Users", Description = "Delete users", Resource = "users", Action = "delete" },
            
            // Role permissions
            new Permission { Name = "Create Roles", Description = "Create roles", Resource = "roles", Action = "create" },
            new Permission { Name = "Read Roles", Description = "View roles", Resource = "roles", Action = "read" },
            new Permission { Name = "Update Roles", Description = "Update roles", Resource = "roles", Action = "update" },
            new Permission { Name = "Delete Roles", Description = "Delete roles", Resource = "roles", Action = "delete" },
            
            // Permission permissions
            new Permission { Name = "Read Permissions", Description = "View permissions", Resource = "permissions", Action = "read" }
        };

        context.Permissions.AddRange(permissions);
        await context.SaveChangesAsync();

        // Create Roles
        var studentRole = new Role
        {
            Name = "Student",
            Description = "Regular student with basic permissions"
        };

        var adminRole = new Role
        {
            Name = "Admin",
            Description = "Administrator with management permissions"
        };

        var superAdminRole = new Role
        {
            Name = "SuperAdmin",
            Description = "Super administrator with all permissions"
        };

        context.Roles.AddRange(studentRole, adminRole, superAdminRole);
        await context.SaveChangesAsync();

        // Assign permissions to roles
        var rolePermissions = new List<RolePermission>();

        // Student permissions (basic)
        var studentPermissions = permissions.Where(p => 
            (p.Resource == "words" && p.Action == "create") ||
            (p.Resource == "words" && p.Action == "read") ||
            (p.Resource == "study" && p.Action == "create") ||
            (p.Resource == "study" && p.Action == "read") ||
            (p.Resource == "stats" && p.Action == "read")
        ).ToList();

        foreach (var permission in studentPermissions)
        {
            rolePermissions.Add(new RolePermission { RoleId = studentRole.Id, PermissionId = permission.Id });
        }

        // Admin permissions (management)
        var adminPermissions = permissions.Where(p => 
            p.Resource != "users" || p.Action != "delete"
        ).ToList();

        foreach (var permission in adminPermissions)
        {
            rolePermissions.Add(new RolePermission { RoleId = adminRole.Id, PermissionId = permission.Id });
        }

        // SuperAdmin permissions (all)
        foreach (var permission in permissions)
        {
            rolePermissions.Add(new RolePermission { RoleId = superAdminRole.Id, PermissionId = permission.Id });
        }

        context.RolePermissions.AddRange(rolePermissions);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTestUserAndWordsAsync(LangTrackDbContext context)
    {
        // Test kullanıcısı ekle (eğer yoksa)
        var testUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        if (testUser == null)
        {
            // Önce Student rolünü al
            var studentRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Student");
            if (studentRole == null) return; // Role yoksa çık

            testUser = new User
            {
                Email = "test@example.com",
                PasswordHash = "Test123!", // Basit hash için şimdilik plain text
                FirstName = "Test",
                LastName = "User",
                IsActive = true,
                RoleId = studentRole.Id // RoleId'yi baştan ata
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
        }

        // A1 seviyesinde test kelimeleri ekle (eğer yoksa)
        if (!context.Words.Any(w => w.UserId == testUser.Id))
        {
            var a1Words = new List<Word>
            {
                new Word
                {
                    UserId = testUser.Id,
                    Text = "hello",
                    Meaning = "merhaba",
                    Example = "Hello, how are you?",
                    Tags = "greeting,a1,basic"
                }
            };

            context.Words.AddRange(a1Words);
            await context.SaveChangesAsync();
        }
    }
}
