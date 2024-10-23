using Gatam.WebAppBegeleider.Components;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Gatam.WebAppBegeleider.Interfaces;
using Gatam.WebAppBegeleider.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddDebug();
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddRazorPages();

        builder.Services.AddAuth0WebAppAuthentication(options =>
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
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddHttpClient<ApiClient>();
        builder.Services.AddScoped<ApiClient>(serviceProvider =>
        {
            IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            HttpClient httpClient = httpClientFactory.CreateClient(nameof(ApiClient)); 
            TokenService tokenService = serviceProvider.GetRequiredService<TokenService>();
            return new ApiClient(httpClient, tokenService);
        });
        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();
        app.MapRazorPages();
        app.MapGet("account/login", async (HttpContext httpContext, string redirectUri = "/") =>
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(redirectUri)
            .Build();

            await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }).AllowAnonymous();
        app.MapGet("account/logout", async (httpContext) =>
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri("/")
                .Build();
            await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        });
        app.Run();
    }
}