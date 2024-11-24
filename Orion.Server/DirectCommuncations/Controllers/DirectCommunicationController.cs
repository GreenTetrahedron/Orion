using Orion.Logging.LoggingServices;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Server.Attributes;
using Orion.Server.DirectCommunications.Repositories;

namespace Orion.Server.DirectCommuncations.Controllers
{
    [Controller]
    public class DirectCommunicationController : IDirectCommunicationController
    {
        private readonly IDirectCommunicationRepository _directCommunicationRepository;
        private readonly LoggingService _loggingService;

        public DirectCommunicationController(IDirectCommunicationRepository directCommunicationRepository, LoggingService loggingService)
        {
            _directCommunicationRepository = directCommunicationRepository;
            _loggingService = loggingService;
        }

        [Handler("NewDirectCommunication")]
        public async Task<ServerTransmission> AddDirectCommunication(NewDirectCommunication newDirectCommunication)
        {
            _loggingService.Log($"New direct communication of users: {newDirectCommunication.SenderId} and {newDirectCommunication.ReceiverId}", DateTime.Now);

            return await _directCommunicationRepository.AddDirectCommunication(newDirectCommunication);
        }

        [Handler("GetDirectMessagesByDirectCommunicationId")]
        public async Task<ServerTransmission> GetMessagesByDirectCommunicationId(DirectCommunicationId id)
        {
            _loggingService.Log($"Get messages by direct communication id: {id.Id}", DateTime.Now);

            return await _directCommunicationRepository.GetDirectMessagesByDirectCommunicationId(id);
        }
    }
}
