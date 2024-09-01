using Orion.Client.Subscriptions;
using Orion.Models.ClientTransmissions;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;

namespace Orion.Client.DirectCommunications.Services
{
    public interface IDirectCommunicationService
    {
        public Task<Subscriptable<DirectCommunicationMessages>> NewDirectCommunication(NewDirectCommunication newDirectCommunication);
        public Task<Subscriptable<GetMessageMessages>> GetMessagesByDirectCommunicationId(Guid id);
    }
}
