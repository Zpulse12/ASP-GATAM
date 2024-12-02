using Gatam.Application.Interfaces;

namespace Gatam.Application.Extensions.Encryption
{
    public class KraEncryptionService : IEncryptionService
    {

        private readonly IEncryptionKeyProvider _encryptionKeyProvider;

        public KraEncryptionService(IEncryptionKeyProvider encryptionKeyProvider)
        {
            this._encryptionKeyProvider = encryptionKeyProvider;
        }

        public string Encrypt(string plainText)
        {
            var encryptionKey = _encryptionKeyProvider.GetCurrentEncryptionKey();

            var encryptionService = new AuthEncryptionService(encryptionKey.KeyData);
            var base64EncryptedText = encryptionService.Encrypt(plainText);
            return new VersionedAesGcmCipherText(encryptionKey.KeyId, base64EncryptedText).ToString();
        }

        public string Decrypt(string cipherText)
        {
            var versionedCiphertext = VersionedAesGcmCipherText.FromString(cipherText);
            var encryptionKey = _encryptionKeyProvider.GetEncryptionKeyById(versionedCiphertext.KeyId);
            var encryptionService = new AuthEncryptionService(encryptionKey.KeyData);
            return encryptionService.Decrypt(versionedCiphertext.Ciphertext);
        }

        public string UpgradeCiphertextWithCurrentEncryptionKey(string cipherText)
        {
            return Encrypt(Decrypt(cipherText));
        }
    }
}
