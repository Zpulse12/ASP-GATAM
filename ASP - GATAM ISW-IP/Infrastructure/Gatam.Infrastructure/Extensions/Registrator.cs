using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Gatam.Infrastructure.Exceptions;
using Gatam.Infrastructure.Repositories;
using Gatam.Infrastructure.UOW;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Gatam.Infrastructure.Extensions.Scopes;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Auth0Net.DependencyInjection;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

namespace Gatam.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            #if DEBUG
            DirectoryInfo rootDirectory = SolutionWrapper.GetSolutionDirectoryPath();
            string dotenvPath = Path.Combine(rootDirectory.FullName, "debug.env");
            DotEnvLoader.Load(dotenvPath);
            #endif
            string SAPASSWORD = Environment.GetEnvironmentVariable("SA_PASSWORD") ?? "";
            string DATABASENAME = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "";
            string DATABASEHOST = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "";
            string DATABASEUSER = Environment.GetEnvironmentVariable("DATABASE_USER") ?? "";

            /// NULL CHECKS
            if (DATABASEHOST.IsNullOrEmpty()) { throw new MissingEnvironmentVariableException(nameof(DATABASEHOST)); }
            if (DATABASENAME.IsNullOrEmpty()) { throw new MissingEnvironmentVariableException(nameof(DATABASENAME)); }
            if (DATABASEUSER.IsNullOrEmpty()) { throw new MissingEnvironmentVariableException(nameof(DATABASEUSER)); }
            if (SAPASSWORD.IsNullOrEmpty()) { throw new MissingEnvironmentVariableException(nameof(SAPASSWORD)); }
            
            // VALID CHECKS
            if (!DATABASEHOST.Contains(",")) { throw new InvalidEnvironmentVariableException($"{nameof(DATABASEHOST)} missing port seperator"); }
            Match m = DotEnvLoader.ValidateWithExpression("/(,\\d{4})$/g", DATABASEHOST);
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

        public static IServiceCollection RegisterAuth0AndCookies(this IServiceCollection services, WebApplicationBuilder builder) {

            string domain = builder.Configuration["Auth0:Domain"] ?? "";
            string audience = builder.Configuration["Auth0:Audience"] ?? "";
            string clientId = builder.Configuration["Auth0:ClientId"] ?? "";
            string clientSecret = builder.Configuration["Auth0:ClientSecret"] ?? "";

            services.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = builder.Configuration["Auth0:Domain"];
                options.ClientId = builder.Configuration["Auth0:ClientId"];
                options.Scope = "openid profile email";
                options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
            })
              .WithAccessToken(options =>
              {
                  options.Audience = builder.Configuration["Auth0:Audience"];
                  options.UseRefreshTokens = true;
              });
            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.Authority = $"https://{domain}";
                options.Audience = audience;
            });
            services.AddAuthorization();
            return services;
        }
        public static IServiceCollection RegisterJWTAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {

            string domain = builder.Configuration["Auth0:Domain"] ?? "";
            string audience = builder.Configuration["Auth0:Audience"] ?? "";
            string clientId = builder.Configuration["Auth0:ClientId"] ?? "";
            string clientSecret = builder.Configuration["Auth0:ClientSecret"] ?? "";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{domain}";
                options.Audience = audience; // This should be the Identifier of your API in Auth0
            });
            // Add authorization
            services.AddAuthorization(options =>
                options.AddPolicy("read:admin", policy => policy.Requirements.Add(
                    new HasScopeRequirement("read:admin", domain)
                    )
                )
            );

            return services;
        }

        public static IServiceCollection RegisterDataProtectionEncryptionMethods(this IServiceCollection services)
        {
            services.AddDataProtection().UseCryptographicAlgorithms(
                new AuthenticatedEncryptorConfiguration
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });
            return services;
        }
    }

}
