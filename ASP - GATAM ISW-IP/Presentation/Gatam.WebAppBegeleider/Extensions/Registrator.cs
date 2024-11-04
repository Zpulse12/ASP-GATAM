using Auth0.AspNetCore.Authentication;
using Gatam.WebAppBegeleider.Extensions.EnvironmentHelper;
using System.Net.Http.Headers;

namespace Gatam.WebAppBegeleider.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterAuth0Authentication(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            EnvironmentWrapper env = serviceProvider.GetRequiredService<EnvironmentWrapper>();
            services.AddAuth0WebAppAuthentication(options =>
            {

                options.Domain = env.AUTH0DOMAIN;
                options.ClientId = env.AUTH0CLIENTID;
                options.ClientSecret = env.AUTH0CLIENTSECRET;
                options.Scope = "openid profile email";
                options.CallbackPath = "/callback";
            }).WithAccessToken(options =>
            {
                options.Audience = env.AUTH0AUDIENCE;
            }); ;
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            return services;
        }
        public static IServiceCollection RegisterCustomApiClient(this IServiceCollection services) 
        {
            services.AddHttpContextAccessor();
            services.AddScoped<TokenService>();
            services.AddScoped<HeaderHandler>();
            services.AddHttpClient<ApiClient>((httpClient) =>
            {
                httpClient.BaseAddress = new Uri("http://webapi:8080/"); //http://localhost/winchester

#if DEBUG
                httpClient.BaseAddress = new Uri("http://localhost:5000");
#endif
            })
            .AddHttpMessageHandler<HeaderHandler>();
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
    }
}
