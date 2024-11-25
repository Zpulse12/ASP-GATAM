using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gatam.Domain;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Microsoft.Extensions.Options;
using Gatam.Application.Extensions;
using Auth0.ManagementApi.Models;

namespace Gatam.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationModule> Modules { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserModule> UserModule { get; set; }

        public DbSet<QuestionAnswer> Answers { get; set; }
        public DbSet<UserQuestion> UserQuestion { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


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
                Username = "adminSuradmin",
                PhoneNumber = "+32 9966554411",
                Email = "admin@app.com",
                Picture = "png",
                RolesIds =  new List<string> { RoleMapper.Roles["BEHEERDER"] },
                PasswordHash = hasher.HashPassword(null, "root"),
                IsActive = false

            };

            ApplicationUser john = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "JohnDoe",
                Surname = "JOHNDOE",
                Username = "JohnDoeJOHNDOE",
                PhoneNumber = "+32 456789166",
                Picture = "png",
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
                Username = "JaneDoeJANEDOE",
                PhoneNumber = "+32 568779633",
                Email = "jane.doe@example.com",
                Picture = "png",
                RolesIds =  new List<string> { RoleMapper.Roles["MAKER"] },
                PasswordHash = hasher.HashPassword(null, "Test@1234"),
                IsActive = false
            };
           

            ApplicationUser lauren = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Lautje",
                Surname = "LAUTJE",
                Username = "LautjeLAUTJE",
                PhoneNumber = "+23 7896544336",
                Email = "lautje.doe@example.com",
                Picture = "png",
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

            builder.Entity<UserModule>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserModules)
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserModule>()
                .HasOne(um => um.Module)
                .WithMany(u => u.UserModules)
                .HasForeignKey(um => um.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);



            Question GLOBALQUESTION = new Question()
            {
                QuestionType = (short)QuestionType.OPEN,
                QuestionTitle = "Wat wil je later bereiken? ",
                CreatedUserId = "123",
                LastUpdatedUserId = "123",
                ApplicationModuleId = GLOBALMODULE.Id
            };

            builder.Entity<Question>().HasData(GLOBALQUESTION);

            QuestionAnswer GLOBALQUESTIONANSWER = new QuestionAnswer() { Answer = "OPEN", QuestionId = GLOBALQUESTION.Id };
            builder.Entity<QuestionAnswer>().HasData(GLOBALQUESTIONANSWER);


            builder.Entity<ApplicationModule>()
            .HasMany(am => am.Questions)
            .WithOne(q => q.ApplicationModule)
            .HasForeignKey(q => q.ApplicationModuleId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Question>()
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId).IsRequired(false);

            builder.Entity<UserQuestion>()
                .HasOne(umqs => umqs.UserModule)
                .WithMany(um => um.UserQuestions)
                .HasForeignKey(umqs => new { umqs.UserModuleId })
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserQuestion>()
                .HasOne(umqs => umqs.Question)
                .WithMany(q => q.UserQuestions)
                .HasForeignKey(umqs => umqs.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}