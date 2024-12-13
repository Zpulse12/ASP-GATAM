using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using Gatam.Application.Behaviours;
using Gatam.Application.Extensions.Encryption;
using Gatam.Application.Extensions.EnvironmentHelper;
using System.Text;

namespace Gatam.Application.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }


        public static IServiceCollection RegisterEncryption(this IServiceCollection services) {
            services.AddSingleton<KraEncryptionService>(provider =>
            {
                var environmentWrapper = provider.GetRequiredService<EnvironmentWrapper>();
                var encryptionKey = environmentWrapper.KEY;
                var keyProvider = new EncryptionKeyProvider(new List<byte[]>());
                keyProvider.RotateKey(Encoding.UTF8.GetBytes(encryptionKey));
                return new KraEncryptionService(keyProvider);
            });
            return services;
        }
    }
}
