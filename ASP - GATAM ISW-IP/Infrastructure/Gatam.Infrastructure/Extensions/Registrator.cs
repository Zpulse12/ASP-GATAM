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

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect("Auth0", options => {
                options.Authority = $"https://{domain}";
                options.ClientId = clientId;
                options.CallbackPath = new PathString("/callback");
                options.ClientSecret = clientSecret;
                options.SaveTokens = true;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                };
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                    {
                        var logoutUri = $"https://{domain}/v2/logout?client_id={clientId}";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }
                            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;

                    },
                    OnRedirectToIdentityProvider = context =>
                    {
                        context.ProtocolMessage.SetParameter("audience", audience);
                        return Task.FromResult(0);
                    }
                };
            });
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
                options.Authority = domain;
                options.Audience = audience; // This should be the Identifier of your API in Auth0
            });
            // Add authorization
            services.AddAuthorization();
            return services;
        }
    }

}
