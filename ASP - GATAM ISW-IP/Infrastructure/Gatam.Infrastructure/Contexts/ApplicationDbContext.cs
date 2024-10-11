using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gatam.Domain;
using Microsoft.AspNetCore.Identity;

namespace Gatam.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seeding users
            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "1", // Ensure unique IDs
                    UserName = "JohnDoe",
                    NormalizedUserName = "JOHNDOE",
                    Email = "john.doe@example.com",
                    NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Test@1234"), // A hashed password
                    IsActive = false
                    
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "JaneDoe",
                    NormalizedUserName = "JANEDOE",
                    Email = "jane.doe@example.com",
                    NormalizedEmail = "JANE.DOE@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Test@1234"),
                    IsActive = false
                }
            );
        }
    }
}
