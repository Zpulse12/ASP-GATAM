using Gatam.WebAppBegeleider.Components;
using Microsoft.AspNetCore.Authentication.Cookies;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Voeg services toe aan de container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddRazorPages();

        //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //.AddCookie(options =>
        //{
        //    options.Cookie.Name = ".AspNet.SharedCookie";
        //    options.Cookie.Domain = "localhost";
        //    options.Cookie.SameSite = SameSiteMode.Lax;
        //    options.LoginPath = "/Account/Login";  // Redirect to ASP.NET 8 project's login page
        //});



        var app = builder.Build();

        // Middleware voor authenticatie -> omzetten naar eigen middleware file en pas na HTTPS redirect...
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
