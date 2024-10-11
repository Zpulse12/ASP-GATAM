using Gatam.Infrastructure.Contexts;
using Gatam.Domain;
using Gatam.WebAppBegeleider.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

        // Add services to the container.
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
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();
        //app.UseAuthentication(); // Zorg ervoor dat authenticatie is ingesteld
        //app.UseAuthorization();

        app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();// Registreer de Razor-pagina's

        // Fallback route naar de loginpagina
        //app.MapFallbackToPage("/login");

        app.Run();
    }
}
