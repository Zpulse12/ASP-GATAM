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
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect("Auth0", options => {
                options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";

                options.ClientId = "WI2HNZLwffVWq4IFLfL1Vs008fMYQlGc";
                options.ClientSecret = "secret";

                options.ResponseType = OpenIdConnectResponseType.Code;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile"); // <- Optional extra
                options.Scope.Add("email");   // <- Optional extra

                options.CallbackPath = new PathString("/callback");
                options.ClaimsIssuer = "Auth0";
                options.SaveTokens = true;

                // Add handling of lo
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                    {
                        var logoutUri = $"https://{builder.Configuration["Auth0:Domain"]}/v2/logout?client_id=WI2HNZLwffVWq4IFLfL1Vs008fMYQlGc";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }
                            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }
        public static IServiceCollection RegisterJWTAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {

            string domain = builder.Configuration["Auth0:Domain"] ?? "";
            string audience = builder.Configuration["Auth0:Audience"] ?? "";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = builder.Configuration["Auth0:Audience"];
                options.RequireHttpsMetadata = false; // REMOVE IN PRODUCTION!!!
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
            services.AddAuthorization(options => 
                options.AddPolicy("read:admin", policy => policy.Requirements.Add(
                    new HasScopeRequirement("read:admin", domain)
                    )
                )
            );
            return services;
        }

        public static IServiceCollection RegisterRedisDataProtectionKeys(this IServiceCollection services)
        {
            #if DEBUG
            DirectoryInfo rootDirectory = SolutionWrapper.GetSolutionDirectoryPath();
            string dotenvPath = Path.Combine(rootDirectory.FullName, "debug.env");
            DotEnvLoader.Load(dotenvPath);
            #endif
            string REDIS = Environment.GetEnvironmentVariable("REDIS") ?? "";

            if (REDIS.IsNullOrEmpty()) { throw new MissingEnvironmentVariableException(nameof(REDIS)); }

            if (!REDIS.Contains(":")) { throw new InvalidEnvironmentVariableException($"{nameof(REDIS)} missing port seperator"); }
            Match m = DotEnvLoader.ValidateWithExpression("/(:\\d{4})$/g", REDIS);
            if (m.Success) { throw new InvalidEnvironmentVariableException($"{nameof(REDIS)} Invalid port. Check your environment file..."); }

            services.AddDataProtection().PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(REDIS));

            return services;
        }
    }

}
