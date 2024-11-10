using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Server.Attributes;
using Orion.Server.Messages.Repositories;

namespace Orion.Server.Messages.Controllers
{
    [Controller]
    public class MessageController : IMessageController
    {
        private IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [Handler("AddDirectMessage")]
        public async Task<ServerTransmission> AddDirectMessage(NewDirectMessage newDirectMessage)
        {
            return await _messageRepository.AddDirectMessage(newDirectMessage);
        }
    }
}
