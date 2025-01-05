using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Cryptography.KeyExchangers
{
    public interface IKeyExchanger
    {
        public bool GeneratePublicPrivateKeyPair(out byte[] privateKey, out byte[] publicKey);

        public byte[] CombineKeys(byte[] otherPublicKey, byte[] privateKey, byte[] publicKey);
    }
}
