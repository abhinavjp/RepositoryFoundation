using System;
using System.Security.Cryptography;
using RepositoryFoundation.Helper.EncodeHelper;
using System.IO;

namespace RepositoryFoundation.Helper.CryptographyHelper
{
    public class SymmetricCryptography
    {
        private readonly ServiceProvider _algorithm;
        private readonly SymmetricAlgorithm _cryptoService;
        public string Key { get; set; }
        public string Salt { get; set; }

        private void SetLegalIV()
        {
            switch (_algorithm)
            {
                case ServiceProvider.Rijndael:
                    _cryptoService.IV = new byte[] {0xf, 0x6f, 0x13, 0x2e, 0x35,
                                0xc2, 0xcd, 0xf9, 0x5, 0x46, 0x9c, 0xea, 0xa8, 0x4b, 0x73,0xcc};
                    break;
                default:
                    _cryptoService.IV = new byte[] {0xf, 0x6f, 0x13, 0x2e,
                                0x35, 0xc2, 0xcd, 0xf9};
                    break;
            }
        }

        public enum ServiceProvider
        {
            Rijndael,
            RC2,
            DES,
            TripleDES
        }

        public SymmetricCryptography(): this(ServiceProvider.Rijndael)
        {
        }

        public SymmetricCryptography(ServiceProvider serviceProvider): this(serviceProvider, string.Empty)
        {
        }

        public SymmetricCryptography(string key) : this(ServiceProvider.Rijndael, key)
        {
        }

        public SymmetricCryptography(ServiceProvider serviceProvider, string key): this(serviceProvider, string.Empty, key)
        {

        }

        public SymmetricCryptography(ServiceProvider serviceProvider, string salt, string key)
        {
            _algorithm = serviceProvider;
            switch (serviceProvider)
            {
                case ServiceProvider.Rijndael:
                    _cryptoService = new RijndaelManaged();
                    break;
                case ServiceProvider.RC2:
                    _cryptoService = new RC2CryptoServiceProvider();
                    break;
                case ServiceProvider.DES:
                    _cryptoService = new DESCryptoServiceProvider();
                    break;
                case ServiceProvider.TripleDES:
                    _cryptoService = new TripleDESCryptoServiceProvider();
                    break;
            }
            _cryptoService.Mode = CipherMode.CBC;
            Salt = salt;
            Key = key;
        }

        public virtual byte[] GetLegalKey()
        {
            // Adjust key if necessary, and return a valid key
            if (_cryptoService.LegalKeySizes.Length > 0)
            {
                // Key sizes in bits
                var keySize = Key.Length * 8;
                var minSize = _cryptoService.LegalKeySizes[0].MinSize;
                var maxSize = _cryptoService.LegalKeySizes[0].MaxSize;
                var skipSize = _cryptoService.LegalKeySizes[0].SkipSize;

                if (keySize > maxSize)
                {
                    // Extract maximum size allowed
                    Key = Key.Substring(0, maxSize / 8);
                }
                else if (keySize < maxSize)
                {
                    // Set valid size
                    var validSize = (keySize <= minSize) ? minSize :
                         (keySize - keySize % skipSize) + skipSize;
                    if (keySize < validSize)
                    {
                        // Pad the key with asterisk to make up the size
                        Key = Key.PadRight(validSize / 8, '*');
                    }
                }
            }
            using (var key = new Rfc2898DeriveBytes(Key, EncodingHelper.GetAsciiBytes(Salt)))
            {
                return key.GetBytes(Key.Length);
            }
        }

        public virtual string Encrypt(string plainText)
        {
            return Encrypt(EncodingHelper.GetAsciiBytes(plainText));
        }

        public virtual string Encrypt(byte[] plainByte)
        {
            var keyByte = GetLegalKey();

            // Set private key
            _cryptoService.Key = keyByte;
            SetLegalIV();

            // Encryptor object
            var cryptoTransform = _cryptoService.CreateEncryptor();

            // Memory stream object
            var ms = new MemoryStream();

            // Crpto stream object
            using (var cs = new CryptoStream(ms, cryptoTransform,
                 CryptoStreamMode.Write))
            {

                // Write encrypted byte to memory stream
                cs.Write(plainByte, 0, plainByte.Length);
                cs.FlushFinalBlock();

                // Get the encrypted byte length
                var cryptoByte = ms.ToArray();

                // Convert into base 64 to enable result to be used in Xml
                return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0));
            }
        }

        public virtual string Decrypt(string cryptoText)
        {
            return Decrypt(Convert.FromBase64String(cryptoText));
        }

        public virtual string Decrypt(byte[] cryptoByte)
        {
            var keyByte = GetLegalKey();

            // Set private key
            _cryptoService.Key = keyByte;
            SetLegalIV();

            // Decryptor object
            var cryptoTransform = _cryptoService.CreateDecryptor();
            try
            {
                // Memory stream object
                var ms = new MemoryStream(cryptoByte, 0, cryptoByte.Length);

                // Crpto stream object
                var cs = new CryptoStream(ms, cryptoTransform,
                    CryptoStreamMode.Read);

                // Get the result from the Crypto stream
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
