using Gatam.WebAppBegeleider.Components;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Gatam.Infrastructure.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.RegisterDataProtectionEncryptionMethods();
        builder.Services.AddAntiforgery();
        builder.Services.RegisterAuth0AndCookies(builder);
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                    .WithRedirectUri(returnUrl)
                    .Build();

            await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        });

        app.MapGet("/Account/Signup", async (HttpContext httpContext, string returnUrl = "/") =>
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                    .WithParameter("screen_hint", "signup")
                    .WithRedirectUri(returnUrl)
                    .Build();

            await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        });

        app.MapGet("/Account/Logout", async (httpContext) =>
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                    .WithRedirectUri("/")
                    .Build();

            await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        });

        app.MapGet("/user-claims", (HttpContext context) =>
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Results.Unauthorized();
            }

            var claims = context.User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Results.Ok(claims);
        });

        app.MapGet("/callback", async (HttpContext context) =>
        {
            // This endpoint handles the authentication response
            var accessToken = await context.GetTokenAsync("access_token");
            var idToken = await context.GetTokenAsync("id_token");

            // Log tokens to see if they were retrieved
            Console.WriteLine($"Access Token: {accessToken}");
            Console.WriteLine($"ID Token: {idToken}");

            // Redirect to another route or handle as needed
            return Results.Redirect("/");
        });

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode();

        app.Run();
    }
}