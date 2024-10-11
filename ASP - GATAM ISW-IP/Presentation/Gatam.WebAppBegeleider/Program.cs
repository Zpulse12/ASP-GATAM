using Gatam.WebAppBegeleider.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;          
using Microsoft.AspNetCore.Authentication.Cookies;

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

        
        string baseURI = "http://webapi:8080/";
        #if DEBUG
             baseURI = "http://localhost:5000";
        #endif
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseURI) });
        

        var app = builder.Build();

        // Configureer de HTTP-request-pijplijn.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication(); // Zorg ervoor dat authenticatie is ingesteld
        app.UseAuthorization();

        app.MapRazorPages(); // Registreer de Razor-pagina's
        app.MapControllers(); // Indien nodig voor API controllers

        // Fallback route naar de loginpagina
        app.MapFallbackToPage("/login");

        app.Run();
    }
}
