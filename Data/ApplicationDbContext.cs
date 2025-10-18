using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Predictive_Lifestyle_Project.Controllers;
using Predictive_Lifestyle_Project.Models;

namespace Predictive_Lifestyle_Project.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    public DbSet<HealthController> HealthController { get; set; }
    public DbSet<HealthEntry> HealthEntries => Set<HealthEntry>();
    public DbSet<Prediction> Predictions => Set<Prediction>();
}
