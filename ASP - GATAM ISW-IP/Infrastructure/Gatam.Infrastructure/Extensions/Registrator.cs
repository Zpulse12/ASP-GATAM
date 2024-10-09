using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Gatam.Infrastructure.Exceptions;
using Gatam.Infrastructure.Repositories;
using Gatam.Infrastructure.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace Gatam.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            #if DEBUG
            DirectoryInfo rootDirectory = VisualStudioWrapper.GetSolutionDirectoryPath();
            string dotenvPath = Path.Combine(rootDirectory.FullName, "debug.env");
            DotEnvLoader.Load(dotenvPath);
            #endif
            string SAPASSWORD = Environment.GetEnvironmentVariable("SA_PASSWORD");
            string DATABASENAME = Environment.GetEnvironmentVariable("DATABASE_NAME");
            string DATABASEHOST = Environment.GetEnvironmentVariable("DATABASE_HOST");
            string DATABASEUSER = Environment.GetEnvironmentVariable("DATABASE_USER");

            /// NULL CHECKS
            if (DATABASEHOST.IsNullOrEmpty()) { throw new MissingEnvironmentVariableException(nameof(DATABASEHOST)); }
            if (DATABASENAME.IsNullOrEmpty()) { throw new MissingEnvironmentVariableException(nameof(DATABASENAME)); }
            if (DATABASEUSER.IsNullOrEmpty()) { throw new MissingEnvironmentVariableException(nameof(DATABASEUSER)); }
            if (SAPASSWORD.IsNullOrEmpty()) { throw new MissingEnvironmentVariableException(nameof(SAPASSWORD)); }
            
            // VALID CHECKS
            if (!DATABASEHOST.Contains(",")) { throw new InvalidEnvironmentVariableException($"{nameof(DATABASEHOST)} missing port seperator"); }
            string pattern = "/(,\\d{4})$/g";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = reg.Match(DATABASEHOST);
            if (m.Success) {throw new InvalidEnvironmentVariableException($"{nameof(DATABASEHOST)} Invalid port. Check your environment file...");}


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer($"Server={DATABASEHOST};Database={DATABASENAME};User={DATABASEUSER};Password={SAPASSWORD};MultipleActiveResultSets=true;TrustServerCertificate=true"));

            return services;
        }
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<ApplicationUser>, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.RegisterDbContext();
            return services;
        }
    }
}
