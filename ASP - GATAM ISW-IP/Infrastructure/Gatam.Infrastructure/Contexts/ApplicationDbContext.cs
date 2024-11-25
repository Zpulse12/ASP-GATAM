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
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

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
                RolesIds =  new List<string> { RoleMapper.Roles[CustomRoles.BEHEERDER].Name },
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
                RolesIds =  new List<string> { RoleMapper.Roles[CustomRoles.VOLGER].Name },
                PasswordHash = hasher.HashPassword(null, "Test@1234"),
                IsActive = false,
                BegeleiderId = GLOBALTESTUSER.Id
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
                RolesIds =  new List<string> { RoleMapper.Roles[CustomRoles.MAKER].Name },
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
                RolesIds =  new List<string> { RoleMapper.Roles[CustomRoles.MAKER].Name },
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


            Question v1 = new Question()
            {
                QuestionType = (short)QuestionType.OPEN,
                QuestionTitle = "Omschrijf kort waarom het belangrijk is om jezelf goed voor te bereiden op een sollicitatiegesprek.",
                CreatedUserId = GLOBALTESTUSER.Id,
                LastUpdatedUserId = GLOBALTESTUSER.Id,
                ApplicationModuleId = GLOBALMODULE.Id,
            };
            Question v2 = new Question()
            {
                QuestionType = (short)QuestionType.MULTIPLE_CHOICE,
                QuestionTitle = "Wat moet je altijd meenemen naar een sollicitatiegesprek?",
                CreatedUserId = "Admin",
                LastUpdatedUserId = "Admin",
                ApplicationModuleId = GLOBALMODULE.Id,
            };
            Question v3 = new Question()
            {
                QuestionType = (short)QuestionType.TRUE_OR_FALSE,
                QuestionTitle = "Je hoeft geen vragen te stellen aan het einde van een sollicitatiegesprek. (WAAR/NIET WAAR)",
                CreatedUserId = "Admin",
                LastUpdatedUserId = "Admin",
                ApplicationModuleId = GLOBALMODULE.Id,
            };
            Question v4 = new Question
            {
                QuestionType = (short)QuestionType.CHOICE_LIST,
                QuestionTitle = "Kies alle correcte voorbereidingsstappen voor een sollicitatiegesprek.",
                CreatedUserId = "Admin",
                LastUpdatedUserId = "Admin",
                ApplicationModuleId = GLOBALMODULE.Id,
            };
            Question GLOBALQUESTION = new Question()
            {
                QuestionType = (short)QuestionType.OPEN,
                QuestionTitle = "Wat wil je later bereiken? ",
                CreatedUserId = "123",
                LastUpdatedUserId = "123",
                ApplicationModuleId = GLOBALMODULE.Id
            };
            builder.Entity<Question>().HasData(GLOBALQUESTION,v1,v2,v3,v4);
            QuestionAnswer GLOBALQUESTIONANSWER = new QuestionAnswer() { Answer = "OPEN", QuestionId = GLOBALQUESTION.Id };
           
            QuestionAnswer a1 = new QuestionAnswer() { Answer = "OPEN", QuestionId = v1.Id };

            QuestionAnswer a2 = new QuestionAnswer { Answer = "Identiteitsbewijs", AnswerValue = "true", QuestionId = v2.Id };
            QuestionAnswer a3 = new QuestionAnswer { Answer = "Een cadeautje voor de interviewer", AnswerValue = "false", QuestionId = v2.Id };
            QuestionAnswer a4 = new QuestionAnswer { Answer = "Een laptop", AnswerValue = "true",  QuestionId = v2.Id };
            QuestionAnswer a5 = new QuestionAnswer { Answer = "Een pak koffie", AnswerValue = "false", QuestionId = v2.Id };

            QuestionAnswer a6 = new QuestionAnswer { Answer = "WAAR", AnswerValue = "true", QuestionId = v3.Id };
            QuestionAnswer a7 = new QuestionAnswer { Answer = "NIET WAAR", AnswerValue = "false", QuestionId = v3.Id };

            QuestionAnswer a8 = new QuestionAnswer{Answer = "Onderzoek het bedrijf",AnswerValue = "true",QuestionId = v4.Id};
            QuestionAnswer a9 = new QuestionAnswer { Answer = "Lees de vacature nog eens goed door", AnswerValue = "false", QuestionId = v4.Id };
            QuestionAnswer a10 = new QuestionAnswer {Answer = "Vraag vrienden om je voor te stellen",AnswerValue = "false", QuestionId = v4.Id };
            QuestionAnswer a11 = new QuestionAnswer{Answer = "Print je CV",AnswerValue = "false", QuestionId = v4.Id};
            builder.Entity<QuestionAnswer>().HasData(GLOBALQUESTIONANSWER,a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11);

            //UserModule GLOBALTESTUSERMODULE = new UserModule() { ModuleId = GLOBALMODULE.Id, UserId = "auth0|673db87bad3e9e2bc46edef7" };
            //builder.Entity<UserModule>().HasData(GLOBALTESTUSERMODULE);
            builder.Entity<ApplicationModule>()
            .HasMany(am => am.Questions)
            .WithOne(q => q.ApplicationModule)
            .HasForeignKey(q => q.ApplicationModuleId);

            builder.Entity<Question>()
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId).IsRequired(false);
            
            builder.Entity<UserModule>()
               .HasOne(um => um.User)
               .WithMany(u => u.UserModules)
               .HasForeignKey(um => um.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserModule>()
                .HasOne(um => um.Module)
                .WithMany(m => m.UserModules)
                .HasForeignKey(um => um.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserModule>()
            .HasMany(q => q.UserGivenAnswers)
            .WithOne(a => a.UserModule)
            .HasForeignKey(a => a.UserModuleId).IsRequired(false);

            builder.Entity<QuestionAnswer>()
            .HasMany(x => x.GivenUserAnswers)
            .WithOne(x => x.QuestionAnswer)
            .HasForeignKey(aa => aa.QuestionAnswerId);

            builder.Entity<UserAnswer>()
            .HasOne(x => x.QuestionAnswer)
            .WithMany(x => x.GivenUserAnswers)
            .HasForeignKey(x => x.QuestionAnswerId);

            builder.Entity<UserAnswer>()
                .HasOne(x => x.UserModule)
                .WithMany(x => x.UserGivenAnswers)
                .HasForeignKey(x => x.UserModuleId); 

           
        }
    }
}