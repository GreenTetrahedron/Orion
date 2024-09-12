using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Orion.Cryptography.HashingServices
{
    public class HashingService : IHashingService
    {
        private readonly SHA256 _sha256;

        public HashingService()
        {
            _sha256 = SHA256.Create();
        }

        public byte[] Hash(byte[] bytes)
        {
            return _sha256.ComputeHash(bytes);
        }
    }
}
