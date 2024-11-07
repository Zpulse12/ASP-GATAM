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
         

            var GLOBALMODULE = new ApplicationModule()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Solliciteren voor beginners",
                Category = "SollicitatieTraining",
                CreatedAt = DateTime.UtcNow
            };

            builder.Entity<ApplicationModule>().HasData(GLOBALMODULE);

            Question GLOBALQUESTION = new Question()
            {
                QuestionType = QuestionType.OPEN,
                QuestionTitle = "Wat wil je later bereiken? ",
                ApplicationModules = new List<ApplicationModule> { GLOBALMODULE },
                QuestionAnswer = "OPEN"
            };

            builder.Entity<Question>().HasData(GLOBALQUESTION);
        }
    }
}