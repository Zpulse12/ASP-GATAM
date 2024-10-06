using Gatam.Authentication.Data;
using Gatam.Domain;
using Gatam.WebAppBegeleider.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{

    private static void Main(string[] args)
    {        
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddHttpClient();
        //builder.Services.AddAuthentication(options =>
        //{
        //    options.DefaultScheme = IdentityConstants.ApplicationScheme;
        //    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        //}).AddIdentityCookies();
        //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        //builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //    options.UseSqlServer(connectionString));

        //builder.Services.AddIdentityCore<ApplicationUser>(options =>
        //{
        //    options.SignIn.RequireConfirmedAccount = true;
        //}).AddEntityFrameworkStores<ApplicationDbContext>()
        //.AddDefaultTokenProviders();

        //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //.AddCookie();

        var app = builder.Build();

        // Middleware voor authenticatie -> omzetten naar eigen middleware file en pas na HTTPS redirect...
        //app.Use(async (context, next) =>
        //{
        //    if(context.User.Identity is not null)
        //    {
        //        if (!context.User.Identity.IsAuthenticated && !context.Request.Path.StartsWithSegments("/login"))
        //        {
        //            context.Response.Redirect("/login");
        //            return; // Stop verdere verwerking
        //        }
        //    }
        //    // Controleer of de gebruiker geauthenticeerd is
        //    await next.Invoke();
        //});

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
