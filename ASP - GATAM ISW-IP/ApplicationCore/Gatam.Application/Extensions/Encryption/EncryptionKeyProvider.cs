using Gatam.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions.Encryption
{
    public class EncryptionKeyProvider : IEncryptionKeyProvider
    {
        private readonly IList<byte[]> _encryptionKeys;
        public EncryptionKeyProvider(IList<byte[]> encryptionKeys)
        {
            this._encryptionKeys = encryptionKeys;
        }
        public EncryptionKey GetCurrentEncryptionKey()
        {
            return new EncryptionKey($"v{_encryptionKeys.Count - 1}", _encryptionKeys.Last());
        }
        public EncryptionKey GetEncryptionKeyById(string keyId)
        {
            var keyIndex = int.Parse(keyId[1..]);
            return new EncryptionKey(keyId, _encryptionKeys[keyIndex]);
        }
        public void RotateKey(byte[] newKeyData)
        {
            _encryptionKeys.Add(newKeyData);
        }

    }
}
