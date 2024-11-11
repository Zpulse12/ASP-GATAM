using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gatam.Domain;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.Extensions.Options;
using Gatam.Application.Extensions;
using Auth0.ManagementApi.Models;

namespace Gatam.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationModule> Modules { get; set; }
        // public DbSet<Question> Questions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Seeding users
            var hasher = new PasswordHasher<ApplicationUser>();
            // SETUP VAN USER IN DB
            ApplicationUser GLOBALTESTUSER = new ApplicationUser() {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                Surname = "Suradmin",
                Username = "admin" + "Suradmin",
                PhoneNumber = "09966554411",
                Email = "admin@app.com",
                RolesIds =  new List<string> { RoleMapper.Roles["BEHEERDER"] },
                PasswordHash = hasher.HashPassword(null, "root")
                

            };

            ApplicationUser john = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "JohnDoe",
                Surname = "JOHNDOE",
                Username = "JohnDoe" + "JOHNDOE",
                PhoneNumber = "0456789166",
                Email = "john.doe@example.com",
                RolesIds =  new List<string> { RoleMapper.Roles["VOLGER"] },
                PasswordHash = hasher.HashPassword(null, "Test@1234"),
                IsActive = false
            };

            ApplicationUser jane = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "JaneDoe",
                Surname = "JANEDOE",
                Username = "JaneDoe" + "JANEDOE",
                PhoneNumber = "0568779633",
                Email = "jane.doe@example.com",
                RolesIds =  new List<string> { RoleMapper.Roles["MAKER"] },
                PasswordHash = hasher.HashPassword(null, "Test@1234"),
                IsActive = false
            };
           

            ApplicationUser lauren = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Lautje",
                Surname = "LAUTJE",
                Username = "Lautje" + "LAUTJE",
                PhoneNumber = "07896544336",
                Email = "lautje.doe@example.com",
                RolesIds =  new List<string> { RoleMapper.Roles["MAKER"] },
                PasswordHash = hasher.HashPassword(null, "Test@1234"),
                IsActive = false
            };

            builder.Entity<ApplicationUser>().HasData(
                GLOBALTESTUSER, john, jane, lauren

            );


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