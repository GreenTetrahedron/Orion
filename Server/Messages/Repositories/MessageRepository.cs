using Orion.Server.DataLayer;
using Orion.Server.Messages.Repositories;

namespace Orion.Server.Messages.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDataLayer _dataLayer;

        public MessageRepository(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        public dynamic AddMessage(Guid senderId, string content)
        {
            var message = new Message() { 
                MessageId = Guid.NewGuid(),
                Content = content,
                SenderId = senderId
            };

            _dataLayer.AddMessage(message);

            return message;
        }

        public dynamic GetMessage(Guid messageId)
        {
            return _dataLayer.GetMessage(messageId);
        }
    }
}