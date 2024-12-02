using Gatam.Application.Extensions.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Interfaces
{
    public interface IEncryptionKeyProvider
    {
        public EncryptionKey GetCurrentEncryptionKey();
        public EncryptionKey GetEncryptionKeyById(string keyId);
        public void RotateKey(byte[] newKeyData);
    }
}
