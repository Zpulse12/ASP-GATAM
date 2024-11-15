using Auth0.AspNetCore.Authentication;
using Gatam.WebAppBegeleider.Extensions.EnvironmentHelper;

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
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            EnvironmentWrapper env = serviceProvider.GetRequiredService<EnvironmentWrapper>();
            services.AddHttpContextAccessor();
            services.AddScoped<TokenService>();
            services.AddScoped<HeaderHandler>();

            string _host = $"http://{env.ENVIRONMENT}-api:8080/";

            services.AddHttpClient<ApiClient>((httpClient) =>
            {
                httpClient.BaseAddress = new Uri(_host);

#if DEBUG
                if(env.ENVIRONMENT == "development")
                {
                    httpClient.BaseAddress = new Uri("http://localhost:5292");
                } else
                {
                    httpClient.BaseAddress = new Uri("http://localhost:5000");
                }
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
                    var requiredRoleIds = RoleMapper.GetRoleValues("BEHEERDER", "BEGELEIDER");
                    policy.RequireRole(requiredRoleIds);

                });
                options.AddPolicy("RequireMakerRole", policy =>
                {
                    var requiredRoleIds = RoleMapper.GetRoleValues("BEHEERDER", "MAKER");
                    policy.RequireRole(requiredRoleIds);
                });
                options.AddPolicy("RequireAdminRole", policy =>
                {
                    var requiredRoleIds = RoleMapper.GetRoleValues("BEHEERDER");
                    policy.RequireRole(requiredRoleIds);
                });
            });
            return services;
        }
    }
}
