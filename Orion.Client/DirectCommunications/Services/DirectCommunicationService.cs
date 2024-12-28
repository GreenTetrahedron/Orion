using Orion.Client.Subscriptions;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;

namespace Orion.Client.DirectCommunications.Services
{
    public class DirectCommunicationService : IDirectCommunicationService
    {
        private readonly IClientService _clientService;

        public DirectCommunicationService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Subscriptable<GetMessageMessages>> GetMessagesByDirectCommunicationId(Guid id)
        {
            return await _clientService.TransmitDataOfTopic<GetMessageMessages>(new DirectCommunicationId() { Id = id }, "GetDirectMessagesByDirectCommunicationId");
        }

        public async Task<Subscriptable<DirectCommunicationMessages>> NewDirectCommunication(NewDirectCommunication newDirectCommunication)
        {
            return await _clientService.TransmitDataOfTopic<DirectCommunicationMessages>(newDirectCommunication, "NewDirectCommunication");
        }
    }
}
