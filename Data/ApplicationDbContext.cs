using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Predictive_Lifestyle_Project.Models;

public sealed class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<HealthEntry> HealthEntries { get; set; } = default!;
    public DbSet<Prediction> Predictions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<HealthEntry>(e =>
        {
            e.Property(x => x.CreatedUtc)
             .HasDefaultValueSql("SYSUTCDATETIME()")
             .ValueGeneratedOnAdd();
        });

        b.Entity<Prediction>(e =>
        {
            e.Property(x => x.CreatedUtc)
             .HasDefaultValueSql("SYSUTCDATETIME()")
             .ValueGeneratedOnAdd();

            e.HasOne(p => p.HealthEntry)
             .WithMany()
             .HasForeignKey(p => p.HealthEntryId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
