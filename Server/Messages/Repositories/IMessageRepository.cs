using Orion.Models.MessageModels;

namespace Orion.Server.Messages.Repositories
{
    public interface IMessageRepository
    {
        public Task<MessageDTO?> AddMessage(NewMessage newMessage);
        public Task<MessageDTO?> GetMessage(Guid messageId);
    }
}