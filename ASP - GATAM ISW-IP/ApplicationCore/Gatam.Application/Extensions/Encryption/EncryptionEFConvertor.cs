using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions.Encryption
{
    public class EncryptionEFConvertor : ValueConverter<string, string>
    {
        public EncryptionEFConvertor(KraEncryptionService encryptionService, ConverterMappingHints mappingHints = null)
            : base(
                  v => encryptionService.Encrypt(v),
                  v => encryptionService.Decrypt(v), mappingHints) 
        { }
    }
}
