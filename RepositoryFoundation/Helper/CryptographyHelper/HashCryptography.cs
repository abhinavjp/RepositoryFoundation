using System;
using System.Security.Cryptography;
using RepositoryFoundation.Helper.EncodeHelper;

namespace RepositoryFoundation.Helper.CryptographyHelper
{
    /// <summary>
    /// HashCryptography is a password protection
    /// one way encryption algorithm.
    /// </summary>
    public class HashCryptography
    {
        private readonly HashAlgorithm _cryptoService;
        public string Salt { get; set; }

        public enum ServiceProvider
        {
            // Supported algorithms
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            MD5
        }

        public HashCryptography(): this(ServiceProvider.SHA1)
        {
        }

        public HashCryptography(ServiceProvider serviceProvider) : this(serviceProvider, string.Empty)
        {
        }

        public HashCryptography(ServiceProvider serviceProvider, string salt)
        {
            // Select hash algorithm
            switch (serviceProvider)
            {
                case ServiceProvider.MD5:
                    _cryptoService = new MD5CryptoServiceProvider();
                    break;
                case ServiceProvider.SHA1:
                    _cryptoService = new SHA1Managed();
                    break;
                case ServiceProvider.SHA256:
                    _cryptoService = new SHA256Managed();
                    break;
                case ServiceProvider.SHA384:
                    _cryptoService = new SHA384Managed();
                    break;
                case ServiceProvider.SHA512:
                    _cryptoService = new SHA512Managed();
                    break;
            }
            Salt = salt;
        }

        public virtual string Encrypt(string plainText)
        {
            var cryptoByte = _cryptoService.ComputeHash(EncodingHelper.GetAsciiBytes(plainText + Salt));
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.Length);
        }
    }
}
