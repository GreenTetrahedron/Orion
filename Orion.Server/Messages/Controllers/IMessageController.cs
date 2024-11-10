using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;

namespace Orion.Server.Messages.Controllers
{
    public interface IMessageController
    {
        public Task<ServerTransmission> AddDirectMessage(NewDirectMessage newDirectMessage);
    }
}
