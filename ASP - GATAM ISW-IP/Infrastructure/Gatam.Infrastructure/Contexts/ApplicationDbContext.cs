using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gatam.Domain;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.Extensions.Options;
using Gatam.Application.Interfaces;
using System.Net;
using Auth0.ManagementApi.Models;
using Gatam.Application.CQRS;

namespace Gatam.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<ApplicationModule> Modules { get; set; }
       // public DbSet<Question> Questions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Seeding users
            var hasher = new PasswordHasher<ApplicationUser>();

            var GLOBALMODULE = new ApplicationModule()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Solliciteren voor beginners",
                Category = "SollicitatieTraining",
                CreatedAt = DateTime.UtcNow
            };

            builder.Entity<ApplicationModule>().HasData(GLOBALMODULE);
            


        }

        public async Task SyncUsersFromAuth0Async(IManagementApi managementApi)
        {
            var auth0Users = new List<UserDTO>(); // De lijst van Auth0-gebruikers die je ophaalt

            int retryCount = 3; // Aantal pogingen om de gebruikers op te halen
            int delay = 5000; // Vertraging tussen pogingen in milliseconden

            for (int attempt = 0; attempt < retryCount; attempt++)
            {
                try
                {
                    auth0Users = (List<UserDTO>)await managementApi.GetAllUsersAsync(); // Haal de gebruikers op van Auth0
                    break; // Stop met proberen als het lukt
                }
                catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    if (attempt == retryCount - 1)
                    {
                        throw; // Als dit de laatste poging is, gooi de uitzondering opnieuw
                    }

                    await Task.Delay(delay); // Wacht 5 seconden voor de volgende poging
                }
            }

            var existingUserIds = await Users.Select(u => u.Id).ToListAsync(); // Haal bestaande gebruikers op uit de database

            foreach (var auth0User in auth0Users)
            {
                if (!existingUserIds.Contains(auth0User.Id)) // Als de gebruiker nog niet bestaat
                {
                    var newUser = new ApplicationUser
                    {
                        Id = auth0User.Id, // Zet de ID van de gebruiker van Auth0
                        BegeleiderId = null // Zet BegeleiderId op null
                    };

                    Users.Add(newUser); // Voeg de nieuwe gebruiker toe aan de database
                }
            }

            await SaveChangesAsync(); // Sla de wijzigingen op in de database
        }


    }
}