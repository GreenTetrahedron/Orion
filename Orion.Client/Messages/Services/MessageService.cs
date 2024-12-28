using Orion.Client.Subscriptions;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results.Messages;

namespace Orion.Client.Messages.Services
{
    public class MessageService : IMessageService
    {
        private readonly IClientService _clientService;

        public MessageService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Subscriptable<NewMessageMessages>> SendDirectMessage(NewDirectMessage newDirectMessage)
        {
            return await _clientService.TransmitDataOfTopic<NewMessageMessages>(newDirectMessage, "AddDirectMessage");
        }
    }
}
