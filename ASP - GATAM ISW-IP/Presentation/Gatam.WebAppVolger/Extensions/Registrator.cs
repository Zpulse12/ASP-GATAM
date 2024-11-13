using Auth0.AspNetCore.Authentication;
using Gatam.WebAppBegeleider.Extensions.EnvironmentHelper;

namespace Gatam.WebAppVolger.Extensions
{
    public static class Registrator
    {
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
