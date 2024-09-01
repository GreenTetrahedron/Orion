using Microsoft.EntityFrameworkCore;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Server.DataLayer;
using Orion.Server.DirectCommuncations;
using Orion.Server.Exceptions;
using Orion.Server.ServerTransmissionServices;
using System.Reflection;

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
            var sender = await _database.Users
                .Where(user => user.UserId == newMessage.SenderId)
                .SingleAsync();

            var directCommunication = await _database.DirectCommunications
                .Where(directCommunication => directCommunication.DirectCommunicationId == newMessage.DirectCommunicationId)
                .Select(directCommuncication => new DirectCommunication()
                {
                    DirectCommunicationId = directCommuncication.DirectCommunicationId,
                    Messages = directCommuncication.Messages,
                    Members = directCommuncication.Members
                })
                .SingleAsync();


            var receiver = directCommunication.Members
                .Where(member => member.UserId != sender.UserId)
                .Single();

            var message = new Message()
            {
                MessageId = Guid.NewGuid(),
                Content = newMessage.Content,
                Sender = sender
            };
            

            _database.Attach(sender);
            _database.Attach(receiver);

            await _database.Messages.AddAsync(message);
            directCommunication.Members.Clear();
            directCommunication.Messages.Add(message);

            _database.Update(directCommunication);

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
                    }, [sender.UserId, receiver.UserId]);

        }

        public async Task<ServerTransmission?> GetMessage(Guid messageId)
        {
            var message = await _database.Messages
                .Where(message => message.MessageId == messageId)
                .Select(message => new MessageDTO
                {
                    MessageId = message.MessageId,
                    SenderProfile = message.Sender,
                    Content = message.Content
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