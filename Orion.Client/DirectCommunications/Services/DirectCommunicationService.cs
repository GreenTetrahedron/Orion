using Orion.Client.Subscriptions;
using Orion.Client.Transmissions;
using Orion.Models.ClientTransmissions;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.ServerTransmissions.Results;

namespace Orion.Client.DirectCommunications.Services
{
    public class DirectCommunicationService : IDirectCommunicationService
    {
        private readonly ITransmissionService _transmissionService;

        public DirectCommunicationService(ITransmissionService transmissionService)
        {
            _transmissionService = transmissionService;
        }

        public async Task<Subscriptable<DirectCommunicationMessages>> NewDirectCommunication(NewDirectCommunication newDirectCommunication)
        {
            return await _transmissionService.TransmitDataOfTopic<DirectCommunicationMessages>(newDirectCommunication, "NewDirectCommunication");
        }
    }
}
