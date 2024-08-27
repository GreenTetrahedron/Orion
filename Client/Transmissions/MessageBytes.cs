using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.Transmissions
{
    public struct MessageBytes
    {
        public byte[] Data;
        public int ReceivedBytes;

        public MessageBytes(byte[] data, int receivedBytes)
        {
            Data = data;
            ReceivedBytes = receivedBytes;
        }
    }
}
