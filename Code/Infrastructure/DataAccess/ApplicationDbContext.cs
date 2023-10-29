using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }
    }
}
