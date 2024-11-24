using Orion.Logging.LoggingServices;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Server.Attributes;
using Orion.Server.Messages.Repositories;

namespace Orion.Server.Messages.Controllers
{
    [Controller]
    public class MessageController : IMessageController
    {
        private readonly IMessageRepository _messageRepository;

        private readonly LoggingService _loggingService;

        public MessageController(IMessageRepository messageRepository, LoggingService loggingService)
        {
            _messageRepository = messageRepository;
            _loggingService = loggingService;
        }

        [Handler("AddDirectMessage")]
        public async Task<ServerTransmission> AddDirectMessage(NewDirectMessage newDirectMessage)
        {
            _loggingService.Log($"New direct message from user {newDirectMessage.SenderId} to direct communication {newDirectMessage.DirectCommunicationId}", DateTime.Now);

            return await _messageRepository.AddDirectMessage(newDirectMessage);
        }
    }
}
