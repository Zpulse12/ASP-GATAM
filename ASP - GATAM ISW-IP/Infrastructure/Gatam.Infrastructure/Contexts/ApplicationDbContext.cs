using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gatam.Domain;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;
using System.Reflection;

namespace Gatam.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<ApplicationTeam> applicationTeams { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Seeding users
            var hasher = new PasswordHasher<ApplicationUser>();
            // SETUP VAN USER IN DB
            ApplicationUser GLOBALTESTUSER = new ApplicationUser() { UserName = "admin", Email = "admin@app.com", PasswordHash = hasher.HashPassword(null, "root") };

            builder.Entity<ApplicationUser>().HasData(
                GLOBALTESTUSER,
                new ApplicationUser
                {
                    UserName = "JohnDoe",
                    NormalizedUserName = "JOHNDOE",
                    Email = "john.doe@example.com",
                    NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Test@1234") // A hashed password
                },
                new ApplicationUser
                {
                    UserName = "JaneDoe",
                    NormalizedUserName = "JANEDOE",
                    Email = "jane.doe@example.com",
                    NormalizedEmail = "JANE.DOE@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Test@1234")
                }
            );
            // SETUP VAN TEAM IN DB
            builder.Entity<ApplicationTeam>().HasData(
                new ApplicationTeam()
                {
                    Id = Guid.NewGuid().ToString(),
                    TeamName = "test team",
                    TeamCreatorId = GLOBALTESTUSER.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.UnixEpoch,
                }
                );
            builder.Entity<ApplicationUser>().HasMany(user => user.OwnedApplicationTeams).WithOne(team => team.TeamCreator).HasForeignKey(team => team.TeamCreatorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
