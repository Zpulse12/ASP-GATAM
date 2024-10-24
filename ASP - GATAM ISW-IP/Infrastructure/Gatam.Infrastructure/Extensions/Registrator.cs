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
using System.Diagnostics;
using Gatam.Application.Extensions;

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

            services.AddScoped<IGenericRepository<ApplicationUser>, UserRepository>();
            services.AddScoped<IGenericRepository<ApplicationTeam>, TeamRepository>();
            services.AddScoped<IGenericRepository<TeamInvitation>, TeamInvitationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.RegisterDbContext();
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
            {
                options.AddPolicy("RequireManagementRole", policy =>
                {
                    policy.RequireRole(RoleMapper.Admin, RoleMapper.Begeleider);
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
