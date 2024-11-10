using Microsoft.EntityFrameworkCore;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Server.DataLayer;
using Orion.Server.DirectCommuncations;
using Orion.Server.ServerTransmissionServices;

namespace Orion.Server.Messages.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly OrionDbContext _database;

        public MessageRepository(OrionDbContext database)
        {
            _database = database;
        }

        public async Task<ServerTransmission?> AddDirectMessage(NewDirectMessage newMessage)
        {
            var directCommunication = await _database.DirectCommunications
                .Where(directCommunication => directCommunication.DirectCommunicationId == newMessage.DirectCommunicationId)
                .Select(directCommuncication => new DirectCommunication()
                {
                    DirectCommunicationId = directCommuncication.DirectCommunicationId,
                    Messages = directCommuncication.Messages,
                    Members = directCommuncication.Members
                })
                .SingleAsync();

            var message = new Message()
            {
                MessageId = Guid.NewGuid(),
                Content = newMessage.Content,
                SenderId = newMessage.SenderId,
                LastUpdated = newMessage.LastUpdated
            };

            //_database.ChangeTracker.Clear();

            //directCommunication.Members.ForEach(member => _database.Attach(member));
            _database.Attach(directCommunication);
            directCommunication.Messages.Add(message);

            _database.Messages.Add(message);

            bool wasSuccessful = await _database.SaveChangesAsync() > 0;

            return !wasSuccessful
                ? ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("AddDirectMessageResult", NewMessageMessages.MESSAGE_CREATION_FAILED)
                    .AddResponseOperationMessage("Failed to add new message to database")
                : ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("AddDirectMessageResult", NewMessageMessages.MESSAGE_CREATED_SUCCESSFULLY)
                    .AddResponseOperationMessage("Successfully added new message to database")
                    .AddPublish("NewDirectMessage", new DirectMessageDTO()
                    {
                        Message = (MessageDTO)message,
                        DirectCommunicationId = newMessage.DirectCommunicationId
                    }, directCommunication.Members.Select(member => member.UserId).ToArray());

        }

        public async Task<ServerTransmission?> GetMessage(Guid messageId)
        {
            var message = await _database.Messages
                .Where(message => message.MessageId == messageId)
                .Select(message => new MessageDTO
                {
                    MessageId = message.MessageId,
                    SenderProfile = message.Sender,
                    Content = message.Content,
                    LastUpdated = message.LastUpdated
                })
                .SingleOrDefaultAsync();

            bool wasSuccessful = message != null;

            return !wasSuccessful
                ? ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("GetMessageResult", GetMessageMessages.FAILED_TO_RETRIEVE_MESSAGE)
                    .AddResponseOperationMessage("Failed to retrieve message from database")
                : ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("GetMessageResult", GetMessageMessages.SUCCESSFULLY_RETRIEVED_MESSAGE, message)
                    .AddResponseOperationMessage("Successfully retrieved message from database");
        }
    }
}