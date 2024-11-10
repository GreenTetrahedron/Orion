using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;

namespace Orion.Server.DirectCommuncations.Controllers
{
    public interface IDirectCommunicationController
    {
        public Task<ServerTransmission> AddDirectCommunication(NewDirectCommunication newDirectCommunication);
        public Task<ServerTransmission> GetMessagesByDirectCommunicationId(DirectCommunicationId id);
    }
}
