using Gatam.WebAppBegeleider.Components;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Gatam.WebAppBegeleider.Interfaces;
using Gatam.WebAppBegeleider.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Repositories;

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
        builder.Services.AddSingleton<EnvironmentWrapper>();
        builder.Services.RegisterAuth0Authentication();
        builder.Services.RegisterCustomApiClient();
        builder.Services.RegisterPolicies();

        builder.Services.AddScoped<ManagementApiRepository>();
        builder.Services.AddHttpClient<IManagementApi, ManagementApiRepository>();


        var app = builder.Build();
        app.Use(async (context, next) =>
        {
            context.Response.Headers.CacheControl= "no-store, no-cache, must-revalidate, max-age=0";
            context.Response.Headers.Pragma= "no-cache";
            context.Response.Headers.Expires= "0";
            await next();
        });

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedProto
        });

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();
        app.MapRazorPages();
        app.MapGet("/Account/GetAccessToken", async (HttpContext httpContext) =>
        {
            var accessToken = await httpContext.GetTokenAsync("access_token");

            if (string.IsNullOrEmpty(accessToken))
            {
                return Results.BadRequest("Access token is not available.");
            }

            return Results.Ok(new { AccessToken = accessToken });
        });
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