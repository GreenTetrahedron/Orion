using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DirectCommunications
{
    public class DirectMessage
    {
        public Guid DirectMessageId { get; set; }

        public Guid DirectCommunicationId { get; set; }

        public Guid MessageId { get; set; }
    }
}
