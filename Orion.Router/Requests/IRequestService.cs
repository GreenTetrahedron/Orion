using System.Net.Sockets;

namespace Orion.Router.Requests
{
    public interface IRequestService
    {
        public bool TryAddRequest(Socket requester, Guid requestId);

        public Guid AddRequest(Socket requester);

        public bool TryGetRequester(Guid requestId, out Socket? requester);
    }
}
