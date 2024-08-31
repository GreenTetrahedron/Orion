using Orion.Client.Subscriptions;
using Orion.Models.ClientTransmissions;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.ServerTransmissions.Results;

namespace Orion.Client.DirectCommunications.Services
{
    public interface IDirectCommunicationService
    {
        public Task<Subscriptable<DirectCommunicationMessages>> NewDirectCommunication(NewDirectCommunication newDirectCommunication);
    }
}
