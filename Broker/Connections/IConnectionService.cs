using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Router.Connections
{
    public interface IConnectionService
    {
        public bool TryAddConnection(Socket handler, Guid userId);

        public bool TryGetConnectionHandler(Guid requestId, out Socket? handler);
    }
}
