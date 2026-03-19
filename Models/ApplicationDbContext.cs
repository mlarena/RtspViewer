using Microsoft.EntityFrameworkCore;
using RtspViewer.Models;

namespace RtspViewer.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Camera> Cameras { get; set; } = null!;
    public DbSet<MonitoringPost> MonitoringPosts { get; set; } = null!;
    public DbSet<Snapshot> Snapshots { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<MonitoringPost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Snapshot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FilePath).IsRequired();
            entity.HasOne(d => d.Camera)
                .WithMany()
                .HasForeignKey(d => d.CameraId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Camera>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.RtspUrl).IsRequired();
            
            entity.HasOne(d => d.MonitoringPost)
                .WithMany(p => p.Cameras)
                .HasForeignKey(d => d.MonitoringPostId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
