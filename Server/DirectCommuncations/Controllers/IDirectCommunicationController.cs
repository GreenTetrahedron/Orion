using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DirectCommuncations.Controllers
{
    public interface IDirectCommunicationController
    {
        public Task<ServerTransmission> AddDirectCommunication(NewDirectCommunicationDTO newDirectCommunication);
    }
}
