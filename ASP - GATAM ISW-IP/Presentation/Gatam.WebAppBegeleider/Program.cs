using Gatam.WebAppBegeleider.Components;
<<<<<<< Updated upstream
=======
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;          
using Microsoft.AspNetCore.Authentication.Cookies;
>>>>>>> Stashed changes

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services
            .AddAuth0WebAppAuthentication(options => {
                options.Domain = builder.Configuration["Auth0:Domain"];
                options.ClientId = builder.Configuration["Auth0:ClientId"];
            });

        // Voeg services toe aan de container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
<<<<<<< Updated upstream
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Middleware voor authenticatie
        app.Use(async (context, next) =>
        {
            // Controleer of de gebruiker geauthenticeerd is
            if (!context.User.Identity.IsAuthenticated && !context.Request.Path.StartsWithSegments("/login"))
            {
                context.Response.Redirect("/login");
                return; // Stop verdere verwerking
            }
            await next.Invoke();
        });

=======
        

        string baseURI = "http://webapi:8080/";
        #if DEBUG
             baseURI = "http://localhost:5000";
        #endif
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseURI) });
        

        var app = builder.Build();

>>>>>>> Stashed changes
        // Configureer de HTTP-request-pijplijn.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
<<<<<<< Updated upstream
        app.UseRouting();
        app.UseAuthentication(); // Zorg ervoor dat authenticatie is ingesteld
        app.UseAuthorization();
=======
        app.UseAntiforgery();
        app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                    .WithRedirectUri(returnUrl)
                    .Build();

            await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        });
        
        //app.UseAuthentication(); // Zorg ervoor dat authenticatie is ingesteld
        //app.UseAuthorization();
>>>>>>> Stashed changes

        app.MapRazorPages(); // Registreer de Razor-pagina's
        app.MapControllers(); // Indien nodig voor API controllers

        // Fallback route naar de loginpagina
        app.MapFallbackToPage("/login");

        app.Run();
    }
}
