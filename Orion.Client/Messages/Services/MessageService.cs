using Orion.Client.Subscriptions;
using Orion.Client.Transmissions;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.Messages.Services
{
    public class MessageService : IMessageService
    {
        private readonly ITransmissionService _transmissionService;

        public MessageService(ITransmissionService transmissionService)
        {
            _transmissionService = transmissionService;
        }

        public async Task<Subscriptable<NewMessageMessages>> SendDirectMessage(NewDirectMessage newDirectMessage)
        {
            return await _transmissionService.TransmitDataOfTopic<NewMessageMessages>(newDirectMessage, "AddDirectMessage");
        }
    }
}
