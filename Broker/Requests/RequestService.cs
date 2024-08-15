using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
