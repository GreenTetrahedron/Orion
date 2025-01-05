using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Cryptography.EncryptionServices
{
    public interface IEncryptionService
    {
        public int IVSize { get; }

        public byte[] Encrypt(byte[] plainText, byte[] key, byte[] iv);
        public byte[] Encrypt(byte[] plainText, byte[] key, out byte[] iv);

        public byte[] Decrypt(byte[] cipherText, byte[] key, byte[] iv);
    }
}
