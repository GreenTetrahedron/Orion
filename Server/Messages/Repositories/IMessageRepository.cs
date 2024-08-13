using Orion.Server.Messages;

namespace Orion.Server.Messages.Repositories
{
    public interface IMessageRepository
    {
        public Task<dynamic> AddMessage(Guid senderId, string content);
        public Task<dynamic> GetMessage(Guid messageId);
    }
}