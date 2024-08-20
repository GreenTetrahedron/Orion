using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DirectCommunications
{
    public class DirectCommunication
    {
        public Guid DirectCommunicationId { get; set; }

        public Tuple<Guid, Guid> UserIds { get; set; }

        public List<Guid>? DirectMessageIds { get; set; }
    }
}
