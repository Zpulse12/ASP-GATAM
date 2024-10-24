﻿using Auth0.AspNetCore.Authentication;
using Gatam.Application.Extensions;
using System.Net.Http.Headers;

namespace Gatam.WebAppBegeleider.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterAuth0Authentication(this IServiceCollection services)
        {
            services.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = "skeletonman.eu.auth0.com";
                options.ClientId = "WI2HNZLwffVWq4IFLfL1Vs008fMYQlGc";
                options.ClientSecret = "Gz-Lhg3d9BCPFPxetXSb05lGUSGoVdW2T5Gk1kqo1KO6435XBVP1bPDp6uBvxlwd";
                options.Scope = "openid profile email";
                options.CallbackPath = "/callback";
            }).WithAccessToken(options =>
            {
                options.Audience = "http://localhost:5000/";
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
            });
            return services;
        }
    }
}
