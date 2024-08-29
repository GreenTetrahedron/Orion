using Orion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.DirectCommunications.Controllers
{
    public interface IDirectCommunicationController
    {
        public void NewDirectCommunication(DirectCommunication directCommunication);
    }
}
