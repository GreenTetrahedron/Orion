using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Orion.Router.Requests
{
    public class RequestService : IRequestService
    {
        private ConcurrentDictionary<Guid, Socket> _requestIdToRequester;

        public RequestService()
        {
            _requestIdToRequester = new ConcurrentDictionary<Guid, Socket>();
        }
        public Guid AddRequest(Socket requester)
        {
            var requestId = Guid.NewGuid();
            _requestIdToRequester[requestId] = requester;

            return requestId;
        }

        public bool RemoveRequest(Guid requestId)
        {
            return _requestIdToRequester.TryRemove(requestId, out _);
        }

        public bool TryAddRequest(Socket requester, Guid requestId)
        {
            return _requestIdToRequester.TryAdd(requestId, requester);
        }

        public bool TryGetRequester(Guid requestId, out Socket? requester)
        {
            return _requestIdToRequester.TryGetValue(requestId, out requester);
        }
    }
}
