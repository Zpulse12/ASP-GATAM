using Gatam.Application.Extensions;
using Gatam.Infrastructure.Contexts;
using Gatam.Infrastructure.Extensions;
using Gatam.Infrastructure.Extensions.Scopes;
using Gatam.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
internal class Program {
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.RegisterApplication();
        builder.Services.RegisterInfrastructure();
        builder.Services.AddControllers();
        builder.Services.RegisterJWTAuthentication(builder);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        var app = builder.Build();

        using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
        {
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        } 

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseErrorHandlingMiddleware();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapGet("/secure", async (HttpContext context) =>
        {
            // Get the access token from the authentication properties
            var accessToken = await context.GetTokenAsync("access_token");
            var idToken = await context.GetTokenAsync("id_token");

            if (string.IsNullOrEmpty(accessToken) && string.IsNullOrEmpty(idToken))
            {
                return Results.BadRequest("No tokens found.");
            }

            var handler = new JwtSecurityTokenHandler();

          

            var accessTokenClaims = new List<CookieMetaData>();
            var idTokenClaims = new List<CookieMetaData>();

            if (!string.IsNullOrEmpty(accessToken))
            {
                var jwtAccessToken = handler.ReadJwtToken(accessToken);
                accessTokenClaims = jwtAccessToken.Claims
                    .Select(c => new CookieMetaData { Type = c.Type, Value = c.Value })
                    .ToList();
            }

            if (!string.IsNullOrEmpty(idToken))
            {
                var jwtIdToken = handler.ReadJwtToken(idToken);
                idTokenClaims = jwtIdToken.Claims
                    .Select(c => new CookieMetaData { Type = c.Type, Value = c.Value })
                    .ToList();
            }

            return Results.Ok(new
            {
                AccessTokenClaims = accessTokenClaims,
                IdTokenClaims = idTokenClaims
            });
        });


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.Run();
    }
}