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

        public async Task<dynamic> AddMessage(Guid senderId, string content)
        {
            var message = new Message() { 
                MessageId = Guid.NewGuid(),
                Content = content,
                SenderId = senderId
            };

            await _dataLayer.AddMessage(message);

            return message;
        }

        public async Task<dynamic> GetMessage(Guid messageId)
        {
            return await _dataLayer.GetMessage(messageId);
        }
    }
}