using Orion.Server.Messages;

namespace Orion.Server.Messages.Repositories
{
    public interface IMessageRepository
    {
        public dynamic AddMessage(Guid senderId, string content);
        public dynamic GetMessage(Guid messageId);
    }
}