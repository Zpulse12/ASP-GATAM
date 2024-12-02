using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions.Encryption
{
    public class VersionedAesGcmCipherText
    {
        public string KeyId { get; }
        public string Ciphertext { get; }

        public VersionedAesGcmCipherText(string keyId, string ciphertext)
        {
            KeyId = keyId;
            Ciphertext = ciphertext;
        }

        public static VersionedAesGcmCipherText FromString(string versionedCiphertext)
        {
            var parts = versionedCiphertext.Split('$');
            return new VersionedAesGcmCipherText(parts[0], parts[1]);
        }

        public override string ToString()
        {
            return $"{KeyId}${Ciphertext}";
        }
    }
}
