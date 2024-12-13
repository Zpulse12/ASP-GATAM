using Gatam.WebAPI.Extensions.Filters;
using Gatam.WebAPI.Middleware;

namespace Gatam.WebAPI.Extensions
{
    public static class Registrator
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
        public static IServiceCollection RegisterFilters(this IServiceCollection services)
        {
            services.AddScoped<IsAuthenticatedApiKey>();
            return services;
        }
    }
}
