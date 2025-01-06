using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Cryptography.KeyExchangers
{
    public class DiffieHellmanKeyExchanger : IKeyExchanger
    {
        public byte[] CombineKeys(byte[] otherPublicKey, byte[] privateKey, byte[] publicKey)
        {
            using ECDiffieHellman exchanger = ECDiffieHellman.Create();

            exchanger.ImportSubjectPublicKeyInfo(publicKey, out _);
            exchanger.ImportECPrivateKey(privateKey, out _);

            using ECDiffieHellman exchanger2 = ECDiffieHellman.Create();

            exchanger2.ImportSubjectPublicKeyInfo(otherPublicKey, out _);

            return exchanger.DeriveKeyMaterial(exchanger2.PublicKey);
        }

        public bool GeneratePublicPrivateKeyPair(out byte[] privateKey, out byte[] publicKey)
        {
            using ECDiffieHellman exchanger = ECDiffieHellman.Create();
            
            publicKey = exchanger.ExportSubjectPublicKeyInfo();
            privateKey = exchanger.ExportECPrivateKey();

            return true;
        }
    }
}
