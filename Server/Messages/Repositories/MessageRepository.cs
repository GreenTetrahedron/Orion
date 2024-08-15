using Orion.Server.DataLayer;

namespace Orion.Server.Messages.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDataLayer _dataLayer;

        public MessageRepository(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        public async Task<Message?> AddMessage(Guid senderId, string content)
        {
            var message = new Message()
            {
                MessageId = Guid.NewGuid(),
                Content = content,
                SenderId = senderId
            };

            await _dataLayer.AddMessage(message);

            return message;
        }

        public async Task<Message?> GetMessage(Guid messageId)
        {
            return await _dataLayer.GetMessage(messageId);
        }
    }
}