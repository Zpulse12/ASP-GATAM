using Gatam.WebAppVolger.Components;
using Auth0.AspNetCore.Authentication;
using Gatam.WebAppVolger.Extensions;
using Gatam.WebAppVolger.Extensions.EnvironmentHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<EnvironmentWrapper>();
        builder.Services.AddScoped<Auth0UserStateService>();
        builder.Services.RegisterAuth0Authentication();
        builder.Services.RegisterCustomApiClient();
        builder.Services.RegisterPolicies();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();

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
        app.UseAntiforgery();
        app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                    .WithRedirectUri(returnUrl)
                    .Build();

            await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        });
        app.MapGet("/Account/Logout", async (HttpContext httpContext) =>
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                    .WithRedirectUri("/")
                    .Build();

            await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        });

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}