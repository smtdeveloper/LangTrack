using Microsoft.EntityFrameworkCore;
using LangTrack.Domain.Entities;

namespace LangTrack.Infrastructure.Data;

public class LangTrackDbContext : DbContext
{
    public LangTrackDbContext(DbContextOptions<LangTrackDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<StudyLog> StudyLogs { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.RoleId).IsRequired();
            
            // Foreign key relationship
            entity.HasOne(e => e.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(e => e.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            // Unique constraint for Email
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Word entity configuration
        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Text).IsRequired().HasMaxLength(128);
            entity.Property(e => e.Meaning).IsRequired().HasMaxLength(512);
            entity.Property(e => e.Example).HasMaxLength(512);
            entity.Property(e => e.Tags).HasMaxLength(256);
            
            // Foreign key relationship
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Words)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Unique constraint for Text per User (case-insensitive)
            entity.HasIndex(e => new { e.UserId, e.Text }).IsUnique();
        });

        // StudyLog entity configuration
        modelBuilder.Entity<StudyLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.WordId).IsRequired();
            entity.Property(e => e.StudiedAtUtc).IsRequired();
            
            // Foreign key relationships
            entity.HasOne(e => e.User)
                  .WithMany(u => u.StudyLogs)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Word)
                  .WithMany(w => w.StudyLogs)
                  .HasForeignKey(e => e.WordId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Index for performance on StudiedAtUtc
            entity.HasIndex(e => e.StudiedAtUtc);
        });

        // Role entity configuration
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
            
            // Unique constraint for Name
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Permission entity configuration
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Resource).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
            
            // Unique constraint for Resource:Action combination
            entity.HasIndex(e => new { e.Resource, e.Action }).IsUnique();
        });

        // RolePermission entity configuration
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RoleId).IsRequired();
            entity.Property(e => e.PermissionId).IsRequired();
            
            // Foreign key relationships
            entity.HasOne(e => e.Role)
                  .WithMany(r => r.RolePermissions)
                  .HasForeignKey(e => e.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Permission)
                  .WithMany(p => p.RolePermissions)
                  .HasForeignKey(e => e.PermissionId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Unique constraint for Role:Permission combination
            entity.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique();
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
