using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Gatam.Infrastructure.Repositories;
using Gatam.Infrastructure.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Gatam.Application.Extensions;
using Gatam.Application.Extensions.EnvironmentHelper;

namespace Gatam.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            services.AddSingleton<EnvironmentWrapper>();
            ServiceProvider provider = services.BuildServiceProvider();
            EnvironmentWrapper env = provider.GetService<EnvironmentWrapper>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer($"Server={env.DATABASEHOST};Database={env.DATABASENAME};User={env.DATABASEUSER};Password={env.SAPASSWORD};MultipleActiveResultSets=true;TrustServerCertificate=true"));

            return services;
        }
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IGenericRepository<ApplicationUser>, UserRepository>();
            services.AddScoped<IGenericRepository<ApplicationTeam>, TeamRepository>();
            services.AddScoped<IGenericRepository<TeamInvitation>, TeamInvitationRepository>();
            services.AddScoped<IGenericRepository<ApplicationModule>, ModuleRepository>();
            services.AddScoped<IManagementApi, ManagementApiRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.RegisterDbContext();
            return services;
        }
        public static IServiceCollection RegisterJWTAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            EnvironmentWrapper env = provider.GetService<EnvironmentWrapper>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{env.AUTH0DOMAIN}";
                options.Audience = env.AUTH0AUDIENCE; // This should be the Identifier of your API in Auth0
            });

            return services;
        }
        public static IServiceCollection RegisterPolicies(this IServiceCollection services)
        {

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireManagementRole", policy =>
                {
                    policy.RequireRole(RoleMapper.Admin, RoleMapper.Begeleider);
                });
                options.AddPolicy("RequireMakerRole", policy =>
                {
                    policy.RequireRole(RoleMapper.Admin, RoleMapper.ContentMaker);
                });
            });
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
