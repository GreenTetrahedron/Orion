using System.Net.Sockets;

namespace Orion.Router.Connections
{
    public interface IConnectionService
    {
        public bool TryAddConnection(Socket handler, Guid userId);

        public bool TryGetConnectionHandler(Guid userId, out Socket? handler);

        public bool TryGetRequesterId(Socket handler, out Guid requesterId);
    }
}
