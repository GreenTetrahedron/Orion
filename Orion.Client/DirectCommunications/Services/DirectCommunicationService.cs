using Orion.Client.Subscriptions;
using Orion.Client.Transmissions;
using Orion.Models.ClientTransmissions;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;

namespace Orion.Client.DirectCommunications.Services
{
    public class DirectCommunicationService : IDirectCommunicationService
    {
        private readonly ITransmissionService _transmissionService;

        public DirectCommunicationService(ITransmissionService transmissionService)
        {
            _transmissionService = transmissionService;
        }

        public async Task<Subscriptable<GetMessageMessages>> GetMessagesByDirectCommunicationId(Guid id)
        {
            return await _transmissionService.TransmitDataOfTopic<GetMessageMessages>(new DirectCommunicationId() { Id = id}, "GetMessagesByDirectCommunicationId");
        }

        public async Task<Subscriptable<DirectCommunicationMessages>> NewDirectCommunication(NewDirectCommunication newDirectCommunication)
        {
            return await _transmissionService.TransmitDataOfTopic<DirectCommunicationMessages>(newDirectCommunication, "NewDirectCommunication");
        }
    }
}
