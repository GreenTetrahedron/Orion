using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.ClientTransmissions
{
    public class NewDirectCommunication
    {
        public Guid SenderId { get; set; }
        public string ReceiverName { get; set; }
    }
}
