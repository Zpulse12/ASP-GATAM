using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions.Encryption
{
    public class EncryptionKeyProvider : IEncryptionKeyProvider
    {
        private readonly IList<byte[]> encryptionKeys;
        public EncryptionKeyProvider(IList<byte[]> encryptionKeys)
        {
            this.encryptionKeys = encryptionKeys;
        }
        public EncryptionKey GetCurrentEncryptionKey()
        {
            return new EncryptionKey($"v{encryptionKeys.Count - 1}", encryptionKeys.Last());
        }
        public EncryptionKey GetEncryptionKeyById(string keyId)
        {
            var keyIndex = int.Parse(keyId[1..]);
            return new EncryptionKey(keyId, encryptionKeys[keyIndex]);
        }
        public void RotateKey(byte[] newKeyData)
        {
            encryptionKeys.Add(newKeyData);
        }

    }
}
