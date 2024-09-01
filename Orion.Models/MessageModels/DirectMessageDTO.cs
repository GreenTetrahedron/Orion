using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.MessageModels
{
    public class DirectMessageDTO
    {
        public Guid DirectCommunicationId { get; set; }

        public MessageDTO Message { get; set; }
    }
}
