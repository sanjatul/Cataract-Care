using Cataract_Care.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cataract_Care.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Package> Packages { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Diagnosis> Diagnosis { get; set; }
    }
}
