using Orion.Server.DataLayer;
using Orion.Server.DataLayer.Entities;

namespace Orion.Server.Messages.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly Database _database;

        public MessageRepository(Database database)
        {
            _database = database;
        }

        public async Task<Message?> AddMessage(Guid senderId, string content)
        {
            var message = new Message()
            {
                MessageId = Guid.NewGuid(),
                Content = content,
                SenderId = senderId
            };

            bool wasSuccessful = await _database.MessageEntity.AddRecord(message.MessageId, message);

            return wasSuccessful ? message : null;
        }

        public async Task<Message?> GetMessage(Guid messageId)
        {
            return await _database.MessageEntity.GetRecordById(messageId);
        }
    }
}