using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryFoundation.Helper.CryptographyHelper;

namespace RepositoryFoundationTest
{
    [TestClass]
    public class CryptographyTest
    {
        const string key = "@p812imp#";
        const string salt = "de589quuj89666";
        const string plainText = "Abhinav Pisharody";
        const int keySize = 1024;

        [TestMethod]
        public void SymmetricTest()
        {
            var cryptic = new SymmetricCryptography(key)
            {
                Salt = salt
            };

            var encryptedText = cryptic.Encrypt(plainText);

            var decryptedText = cryptic.Decrypt(encryptedText);

            Assert.AreEqual(plainText, decryptedText);

            cryptic = new SymmetricCryptography(SymmetricCryptography.ServiceProvider.RC2)
            {
                Salt = salt
            };

            encryptedText = cryptic.Encrypt(plainText);

            Assert.AreEqual(plainText, cryptic.Decrypt(encryptedText));


            cryptic = new SymmetricCryptography(SymmetricCryptography.ServiceProvider.DES)
            {
                Salt = salt
            };

            encryptedText = cryptic.Encrypt(plainText);

            Assert.AreEqual(plainText, cryptic.Decrypt(encryptedText));

            cryptic = new SymmetricCryptography(SymmetricCryptography.ServiceProvider.TripleDES)
            {
                Salt = salt
            };

            encryptedText = cryptic.Encrypt(plainText);

            Assert.AreEqual(plainText, cryptic.Decrypt(encryptedText));
        }

        [TestMethod]
        public void AsymmetricTest()
        {
            var cryptoService = new AsymmetricCryptography();

            AsymmetricCryptography.GenerateKeys(keySize, out string publicKey, out string publicAndPrivateKey);
            var encryptedText = cryptoService.EncryptText(plainText, keySize, publicKey);
            var decryptedText = cryptoService.DecryptText(encryptedText, keySize, publicAndPrivateKey);

            Assert.AreEqual(plainText, decryptedText);
        }

        [TestMethod]
        public void HashTest()
        {
            var hash = new HashCryptography(HashCryptography.ServiceProvider.SHA1);

            var testHash = hash.Encrypt(plainText);
        }
    }
}
