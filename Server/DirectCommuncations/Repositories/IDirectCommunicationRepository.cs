using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.DirectCommuncations;
using Orion.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DirectCommunications.Repositories
{
    public interface IDirectCommunicationRepository
    {
        public Task<ServerTransmission?> AddDirectCommunication(NewDirectCommunication newDirectCommunication);

        public Task<DirectCommunicationDTO?> GetDirectCommunicationById(Guid id);

        public Task<ServerTransmission?> GetDirectMessagesByDirectCommunicationId(DirectCommunicationId id);
    }
}
