using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Cryptography.EncryptionServices
{
    public class AesEncryptionService : IEncryptionService
    {
        private int _ivSize = 16;

        public int IVSize => _ivSize;

        public byte[] Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            return aes.DecryptCbc(cipherText, iv, PaddingMode.PKCS7);
        }

        public byte[] Encrypt(byte[] plainText, byte[] key, byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            return aes.EncryptCbc(plainText, iv, PaddingMode.PKCS7);
        }

        public byte[] Encrypt(byte[] plainText, byte[] key, out byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            iv = aes.IV;

            return aes.EncryptCbc(plainText, iv, PaddingMode.PKCS7);
        }
    }
}
