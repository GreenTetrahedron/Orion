using Orion.Router.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Orion.Router.Requests
{
    public class RequestService : IRequestService
    {
        private readonly ConcurrentDictionary<Guid, LifeSupport> _requestIdToRequester;

        public RequestService()
        {
            _requestIdToRequester = new ConcurrentDictionary<Guid, LifeSupport>();
        }

        public bool TryEndRequest(Guid requestId, out LifeSupport connection)
        {
            return _requestIdToRequester.Remove(requestId, out connection);
        }

        public bool TryGetRequestConnection(Guid requestId, out LifeSupport connection)
        {
            return _requestIdToRequester.TryGetValue(requestId, out connection);
        }

        public bool NewRequest(ref LifeSupport connection, out Guid requestId)
        {
            requestId = Guid.NewGuid();

            return _requestIdToRequester.TryAdd(requestId, connection);
        }
    }
}
