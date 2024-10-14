using Gatam.Application.Extensions;
using Gatam.Infrastructure.Contexts;
using Gatam.Infrastructure.Extensions;
using Gatam.Infrastructure.Extensions.Scopes;
using Gatam.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
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
        builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();



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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.Run();
    }
}