using JobPortal.Common.Helpers;
using JobPortal.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Data
{
    public class JobPortalDbContext : DbContext
    {
        public JobPortalDbContext(DbContextOptions<JobPortalDbContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hashedPassword = PasswordHelper.HashPassword("Admin123!");

            // Kullanıcı seed datası
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "Admin User",
                Email = "admin@example.com",
                PhoneNumber = "+123456789",
                Address = "Admin Address",
                CreatedDate = DateTime.UtcNow,
                Password = hashedPassword
            });

            // Şirket seed datası
            modelBuilder.Entity<Company>().HasData(new Company
            {
                Id = 1,
                Name = "Example Company",
                PhoneNumber = "+987654321",
                Address = "Company Address",
                JobPostingLimit = 10,
                CreatedUserId = 1,
                CreatedDate = DateTime.UtcNow
            });

            // İş seed datası
            modelBuilder.Entity<Job>().HasData(new Job
            {
                Id = 1,
                CompanyId = 1,
                UserId = 1,
                Position = "Software Engineer",
                Description = "Looking for an experienced software engineer.",
                ExpirationDate = DateTime.UtcNow.AddDays(15),
                QualityScore = 5,
                CreatedUserId = 1,
                CreatedDate = DateTime.UtcNow
            });
        }

    }

}
