using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Orion.Router.Clients
{
    public class ClientService : IClientService
    {
        private ConcurrentDictionary<Guid, Socket> _userIdToHandler;
        private ConcurrentDictionary<Socket, Guid> _handlerToUserId;

        public ClientService()
        {
            _userIdToHandler = new();
            _handlerToUserId = new();
        }

        public bool TerminateConnection(Socket handler)
        {
            var result = _handlerToUserId.TryRemove(handler, out var userId)
                && _userIdToHandler.TryRemove(userId, out _);

            handler?.Shutdown(SocketShutdown.Both);
            handler?.Disconnect(false);
            handler?.Close();
            handler?.Dispose();

            return result;
        }

        public bool TerminateConnection(Guid userId)
        {
            var result = _userIdToHandler.TryRemove(userId, out var handler)
                && handler != null
                && _handlerToUserId.TryRemove(handler, out _);

            handler?.Shutdown(SocketShutdown.Both);
            handler?.Disconnect(false);
            handler?.Close();
            handler?.Dispose();

            return result;
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
