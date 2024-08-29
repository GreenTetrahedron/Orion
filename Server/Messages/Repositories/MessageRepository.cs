using Microsoft.EntityFrameworkCore;
using Orion.Models.MessageModels;
using Orion.Server.DataLayer;

namespace Orion.Server.Messages.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly OrionDbContext _database;

        public MessageRepository(OrionDbContext database)
        {
            _database = database;
        }

        public async Task<MessageDTO?> AddMessage(NewMessage newMessage)
        {
            var sender = await _database.Users.FindAsync(newMessage.SenderId);

            var message = new Message()
            {
                MessageId = Guid.NewGuid(),
                Content = newMessage.Content,
                Sender = sender
            };

            await _database.Messages.AddAsync(message);

            bool wasSuccessful = await _database.SaveChangesAsync() > 0;

            return wasSuccessful ? (MessageDTO)message : null;
        }

        public async Task<MessageDTO?> GetMessage(Guid messageId)
        {
            return await _database.Messages
                .Where(message => message.MessageId == messageId)
                .Select(message => new MessageDTO
                {
                    MessageId = message.MessageId,
                    SenderProfile = message.Sender,
                    Content = message.Content
                })
                .SingleOrDefaultAsync();
        }
    }
}