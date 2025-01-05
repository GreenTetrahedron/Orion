using Orion.Router.Models;
using Orion.Transport.TransmissionServices;
using System.Net.Sockets;

namespace Orion.Router.Clients
{
    public interface IClientService
    {
        public Task<LifeSupport> InstantiateConnection(Socket socket);

        public bool TryGetClientConnection(Guid userId, out LifeSupport connection);

        public bool TerminateConnection(ref LifeSupport connection);
    }
}
