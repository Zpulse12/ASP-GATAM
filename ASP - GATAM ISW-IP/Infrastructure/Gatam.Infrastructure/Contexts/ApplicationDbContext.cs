using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gatam.Domain;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace Gatam.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationTeam> ApplicationTeams { get; set; }

        public DbSet<TeamInvitation> TeamInvitations { get; set; }

        public DbSet<ApplicationModule> Modules { get; set; }
        public DbSet<Question> Questions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Seeding users
            var hasher = new PasswordHasher<ApplicationUser>();
            // SETUP VAN USER IN DB
            ApplicationUser GLOBALTESTUSER = new ApplicationUser() { UserName = "admin", Email = "admin@app.com", PasswordHash = hasher.HashPassword(null, "root") };
            ApplicationUser john = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "JohnDoe",
                NormalizedUserName = "JOHNDOE",
                Email = "john.doe@example.com",
                NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
                PasswordHash = hasher.HashPassword(null, "Test@1234"),
                IsActive = false// A hashed password
            };
            ApplicationUser jane = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "JaneDoe",
                NormalizedUserName = "JANEDOE",
                Email = "jane.doe@example.com",
                NormalizedEmail = "JANE.DOE@EXAMPLE.COM",
                PasswordHash = hasher.HashPassword(null, "Test@1234"),
                IsActive = false
            };
            ApplicationUser lauren = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Lautje",
                NormalizedUserName = "LAUTJE",
                Email = "lautje.doe@example.com",
                NormalizedEmail = "LAUTJE.DOE@EXAMPLE.COM",
                PasswordHash = hasher.HashPassword(null, "Test@1234"),
                IsActive = false
            };
            builder.Entity<ApplicationUser>().HasData(
                GLOBALTESTUSER, john, jane, lauren

            );

            ApplicationTeam GLOBALTESTTEAM = new ApplicationTeam()
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "TeamTest1",
                TeamCreatorId = GLOBALTESTUSER.Id,
                IsDeleted = false,
                CreatedAt = DateTime.UnixEpoch,
            };
            ApplicationTeam TESTTEAM2 = new ApplicationTeam()
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "TeamTest2",
                TeamCreatorId = GLOBALTESTUSER.Id,
                IsDeleted = false,
                CreatedAt = DateTime.UnixEpoch,
            };
            // SETUP VAN TEAM IN DB
            builder.Entity<ApplicationTeam>().HasData(GLOBALTESTTEAM, TESTTEAM2);

            //            builder.Entity<TeamInvitation>().HasData(new TeamInvitation { ApplicationTeamId = GLOBALTESTTEAM.Id, UserId = GLOBALTESTUSER.Id});

            // Seeding team invitations for John Doe and Jane Doe
            builder.Entity<TeamInvitation>().HasData(
                new TeamInvitation()
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationTeamId = GLOBALTESTTEAM.Id,
                    UserId = john.Id,
                    isAccepted = true,
                    CreatedAt = DateTime.UtcNow,
                    ResponseDateTime = DateTime.UtcNow,
                },
                new TeamInvitation()
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationTeamId = GLOBALTESTTEAM.Id,
                    UserId = jane.Id,
                    isAccepted = false,
                    CreatedAt = DateTime.UtcNow,
                    ResponseDateTime = DateTime.UtcNow
                }
                
            );
            // RELATIES

            builder.Entity<ApplicationUser>().HasMany(user => user.OwnedApplicationTeams).WithOne(team => team.TeamCreator).HasForeignKey(team => team.TeamCreatorId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationTeam>().HasMany(team => team.TeamInvitations).WithOne(invitation => invitation.applicationTeam).HasForeignKey(invitation => invitation.ApplicationTeamId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationUser>().HasMany(user => user.InvitationsRequests).WithOne(invitation => invitation.applicationUser).HasForeignKey(invitation => invitation.UserId).OnDelete(DeleteBehavior.Restrict);


            var GLOBALMODULE = new ApplicationModule()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Solliciteren voor beginners",
                Category = "SollicitatieTraining",
                CreatedAt = DateTime.UtcNow
            };

            builder.Entity<ApplicationModule>().HasData(GLOBALMODULE);
            


        }
    }
}