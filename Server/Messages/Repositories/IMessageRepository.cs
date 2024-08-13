using Orion.Server.Messages;

namespace Orion.Server.Messages.Repositories
{
    public interface IMessageRepository
    {
        public Task<Message?> AddMessage(Guid senderId, string content);
        public Task<Message?> GetMessage(Guid messageId);
    }
}