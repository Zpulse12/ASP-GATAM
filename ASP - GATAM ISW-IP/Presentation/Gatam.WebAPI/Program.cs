using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Infrastructure.Extensions;
using Gatam.Infrastructure.Repositories;
using Gatam.Infrastructure.UOW;
using Gatam.WebAPI.Extensions;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.RegisterApplication();
        builder.Services.RegisterInfrastructure();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IGenericRepository<ApplicationTeam>, TeamRepository>();
        builder.Services.AddScoped<IGenericRepository<TeamInvitation>, TeamInvitationRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseErrorHandlingMiddleware();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}