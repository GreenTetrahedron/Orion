using Orion.Models.DirectCommunicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.DirectCommunications.Controllers
{
    public interface IDirectCommunicationController
    {
        public void NewDirectCommunication(DirectCommunicationDTO directCommunicationDTO);
    }
}
