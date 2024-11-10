using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;

namespace Orion.Server.DirectCommunications.Repositories
{
    public interface IDirectCommunicationRepository
    {
        public Task<ServerTransmission?> AddDirectCommunication(NewDirectCommunication newDirectCommunication);

        public Task<DirectCommunicationDTO?> GetDirectCommunicationById(Guid id);

        public Task<ServerTransmission?> GetDirectMessagesByDirectCommunicationId(DirectCommunicationId id);
    }
}
