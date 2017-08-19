using System;
using System.Security.Cryptography;
using RepositoryFoundation.Helper.EncodeHelper;

namespace RepositoryFoundation.Helper.CryptographyHelper
{
    public class AsymmetricCryptography
    {
        private bool _isOptimalAsymmetricEncryptionPadding;
        public AsymmetricCryptography(bool isOptimalAsymmetricEncryptionPadding)
        {
            _isOptimalAsymmetricEncryptionPadding = isOptimalAsymmetricEncryptionPadding;
        }
        public AsymmetricCryptography():this(false)
        {

        }
        public static void GenerateKeys(int keySize, out string publicKey, out string publicAndPrivateKey)
        {
            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                publicKey = provider.ToXmlString(false);
                publicAndPrivateKey = provider.ToXmlString(true);
            }
        }

        public string EncryptText(string text, int keySize, string publicKeyXml)
        {
            var encrypted = Encrypt(EncodingHelper.GetUtf8Bytes(text), keySize, publicKeyXml);
            return Convert.ToBase64String(encrypted);
        }

        public byte[] Encrypt(byte[] data, int keySize, string publicKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", nameof(data));
            var maxLength = GetMaxDataLength(keySize);
            if (data.Length > maxLength) throw new ArgumentException($"Maximum data length is {maxLength}", nameof(data));
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", nameof(keySize));
            if (String.IsNullOrEmpty(publicKeyXml)) throw new ArgumentException("Key is null or empty", nameof(publicKeyXml));

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicKeyXml);
                return provider.Encrypt(data, _isOptimalAsymmetricEncryptionPadding);
            }
        }

        public string DecryptText(string text, int keySize, string publicAndPrivateKeyXml)
        {
            var decrypted = Decrypt(Convert.FromBase64String(text), keySize, publicAndPrivateKeyXml);
            return EncodingHelper.GetUtf8String(decrypted);
        }

        public byte[] Decrypt(byte[] data, int keySize, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", nameof(data));
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", nameof(keySize));
            if (String.IsNullOrEmpty(publicAndPrivateKeyXml)) throw new ArgumentException("Key is null or empty", nameof(publicAndPrivateKeyXml));

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicAndPrivateKeyXml);
                return provider.Decrypt(data, _isOptimalAsymmetricEncryptionPadding);
            }
        }

        public int GetMaxDataLength(int keySize)
        {
            if (_isOptimalAsymmetricEncryptionPadding)
            {
                return ((keySize - 384) / 8) + 7;
            }
            return ((keySize - 384) / 8) + 37;
        }

        public static bool IsKeySizeValid(int keySize)
        {
            return keySize >= 384 &&
                    keySize <= 16384 &&
                    keySize % 8 == 0;
        }
    }
}
