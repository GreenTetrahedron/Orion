using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Router.Requests
{
    public interface IRequestService
    {
        public bool TryAddRequest(Socket requester, Guid requestId);

        public Guid AddRequest(Socket requester);

        public bool TryGetRequester(Guid requestId, out Socket? requester);
    }
}
