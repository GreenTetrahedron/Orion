using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Cryptography.HashingServices
{
    public interface IHashingService
    {
        public byte[] Hash(byte[] bytes);
    }
}
