using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;

namespace Orion.Server.Messages.Repositories
{
    public interface IMessageRepository
    {
        public Task<ServerTransmission?> AddDirectMessage(NewDirectMessage newMessage);
        public Task<ServerTransmission?> AddGroupMessage(NewGroupMessage newMessage);
        public Task<ServerTransmission?> GetMessage(Guid messageId);
    }
}