using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gatam.Application.Extensions.Attributes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gatam.Application.Extensions.Encryption
{
    public static class ModelPropertyEncryptionExtension
    {
        public static void UseEncryption(this ModelBuilder modelBuilder, KraEncryptionService encryptionService, ConverterMappingHints mappingHints = null)
        {
            if (encryptionService == null)
            {
                throw new ArgumentNullException(nameof(encryptionService), "KraEncryptionService cannot be null.");
            }

            var converter = new EncryptionEFConvertor(encryptionService, mappingHints);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && !IsDiscriminator((IProperty)property))
                    {
                        var attributes = property.PropertyInfo?.GetCustomAttributes(typeof(EncryptedAttribute), false);
                        if (attributes != null && attributes.Any())
                        {
                            property.SetValueConverter(converter);
                        }
                    }
                }
            }
        }
        private static bool IsDiscriminator(IProperty property)
        {
            return property.IsShadowProperty() && property.Name == "Discriminator";
        }
    }
}
