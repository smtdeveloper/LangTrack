using Microsoft.EntityFrameworkCore;
using LangTrack.Domain.Entities;

namespace LangTrack.Infrastructure.Data;

public class LangTrackDbContext : DbContext
{
    public LangTrackDbContext(DbContextOptions<LangTrackDbContext> options) : base(options)
    {
    }

    public DbSet<Word> Words { get; set; }
    public DbSet<StudyLog> StudyLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Word entity configuration
        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Text).IsRequired().HasMaxLength(128);
            entity.Property(e => e.Meaning).IsRequired().HasMaxLength(512);
            entity.Property(e => e.Example).HasMaxLength(512);
            entity.Property(e => e.Tags).HasMaxLength(256);
            
            // Unique constraint for Text (case-insensitive)
            entity.HasIndex(e => e.Text).IsUnique();
        });

        // StudyLog entity configuration
        modelBuilder.Entity<StudyLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StudiedAtUtc).IsRequired();
            
            // Foreign key relationship
            entity.HasOne(e => e.Word)
                  .WithMany(w => w.StudyLogs)
                  .HasForeignKey(e => e.WordId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Index for performance on StudiedAtUtc
            entity.HasIndex(e => e.StudiedAtUtc);
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
