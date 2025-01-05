using Orion.Router.Models;
using System.Net.Sockets;

namespace Orion.Router.Requests
{
    public interface IRequestService
    {
        public bool NewRequest(ref LifeSupport connection, out Guid requestId);
        
        public bool TryGetRequestConnection(Guid requestId, out LifeSupport connection);

        public bool TryEndRequest(Guid requestId, out LifeSupport connection);
    }
}
