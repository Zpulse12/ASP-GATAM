using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions.Encryption
{
    internal class AuthEncryptionService
    {
        private readonly byte[] _encryptionKey;

        public AuthEncryptionService(byte[] encryptionKey)
        {
            this._encryptionKey = encryptionKey;
        }

        public string Encrypt(string plaintext)
        {
            using var aes = new AesCcm(_encryptionKey);
            var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
            RandomNumberGenerator.Fill(nonce);
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            var ciphertextBytes = new byte[plaintextBytes.Length];
            var tag = new byte[AesGcm.TagByteSizes.MaxSize];
            aes.Encrypt(nonce, plaintextBytes, ciphertextBytes, tag);
            return new AesGcmCipherText(nonce, tag, ciphertextBytes).ToString();
        }

        public string Decrypt(string ciphertext)
        {
            var gcmCiphertext = AesGcmCipherText.FromBase64String(ciphertext);
            using var aes = new AesCcm(_encryptionKey);
            var plaintextBytes = new byte[gcmCiphertext.CiphertextBytes.Length];
            aes.Decrypt(gcmCiphertext.Nonce, gcmCiphertext.CiphertextBytes, gcmCiphertext.Tag, plaintextBytes);
            return Encoding.UTF8.GetString(plaintextBytes);
        }
    }
}
