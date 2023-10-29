using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<UserModel> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.ToTable("MST_User");
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.UserName, "UQ_MST_User_UserName").IsUnique();
            });
        }
            
    }
}
