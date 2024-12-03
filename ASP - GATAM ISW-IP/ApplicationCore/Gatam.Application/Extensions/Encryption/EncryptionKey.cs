using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions.Encryption
{
    public class EncryptionKey
    {
        public string KeyId { get; }
        public byte[] KeyData { get; }

        public EncryptionKey(string keyId, byte[] keyData)
        {
            KeyId = keyId;
            KeyData = keyData;
        }
    }
}
