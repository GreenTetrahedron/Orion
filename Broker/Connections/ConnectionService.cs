using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Router.Connections
{
    public class ConnectionService : IConnectionService
    {
        private ConcurrentDictionary<Guid, Socket> _userIdToHandler;

        public ConnectionService()
        {
            _userIdToHandler = new ConcurrentDictionary<Guid, Socket>();
        }

        public bool TryAddConnection(Socket handler, Guid userId)
        {
            return _userIdToHandler.TryAdd(userId, handler);
        }

        public bool TryGetConnectionHandler(Guid requestId, out Socket? handler)
        {
            return _userIdToHandler.TryGetValue(requestId, out handler);
        }
    }
}
