using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Predictive_Lifestyle_Project.Controllers;

namespace Predictive_Lifestyle_Project.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    public DbSet<UserInfoController>  UserInfoControllers { get; set; }
}
