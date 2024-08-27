using Orion.Client.Transmissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.Connections
{
    public interface IConnectionService
    {
        public Task<bool> SendMessage(byte[] data);
        public Task<MessageBytes> ReceiveMessage();
    }
}
