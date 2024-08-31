using Orion.Client.Attributes;
using Orion.Models;
using Orion.Models.DirectCommunicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.DirectCommunications.Controllers
{
    [Controller]
    public class DirectCommunicationController : IDirectCommunicationController
    {
        [Handler("NewDirectCommunication")]
        public void NewDirectCommunication(DirectCommunicationDTO directCommunication)
        {
            throw new NotImplementedException();
        }
    }
}
