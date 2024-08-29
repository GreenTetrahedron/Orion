using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.DirectCommunicationModels
{
    public class NewDirectCommunication
    {
        public Guid SenderId { get; set; }

        public Guid ReceiverId { get; set; }
    }
}
