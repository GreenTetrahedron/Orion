using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Orion.Router.Connections
{
    public class ConnectionService : IConnectionService
    {
        private ConcurrentDictionary<Guid, Socket> _userIdToHandler;
        private ConcurrentDictionary<Socket, Guid> _handlerToUserId;

        public ConnectionService()
        {
            _userIdToHandler = new();
            _handlerToUserId = new();
        }

        public bool TryAddConnection(Socket handler, Guid userId)
        {
            return _userIdToHandler.TryAdd(userId, handler) && _handlerToUserId.TryAdd(handler, userId);
        }

        public bool TryGetConnectionHandler(Guid userId, out Socket? handler)
        {
            return _userIdToHandler.TryGetValue(userId, out handler);
        }

        public bool TryGetRequesterId(Socket handler, out Guid requesterId)
        {
            return _handlerToUserId.TryGetValue(handler, out requesterId);
        }
    }
}
