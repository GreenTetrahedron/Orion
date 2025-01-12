using Microsoft.EntityFrameworkCore;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;
using Orion.Server.DataLayer;
using Orion.Server.DirectCommuncations;
using Orion.Server.Exceptions;
using Orion.Server.ServerTransmissionServices;
using Orion.Server.Users;

namespace Orion.Server.DirectCommunications.Repositories
{
    public class DirectCommunicationRepository : IDirectCommunicationRepository
    {
        private readonly OrionDbContext _database;

        public DirectCommunicationRepository(OrionDbContext database)
        {
            _database = database;
        }

        public async Task<ServerTransmission> AddDirectCommunication(NewDirectCommunication newDirectCommunication)
        {
            var sender = await _database.Users.FindAsync(newDirectCommunication.SenderId);
            var receiver = await _database.Users.FindAsync(newDirectCommunication.ReceiverId);

            if (sender == null)
                return ServerTransmissionService
                    .NewSuccessfulExceptionResponseServerTransmission("NewDirectCommunicationResult",
                    DirectCommunicationMessages.DIRECT_COMMUNICATION_CREATION_FAILED,
                    "Sender was not found...");

            if (receiver == null)
                return ServerTransmissionService
                    .NewSuccessfulExceptionResponseServerTransmission("NewDirectCommunicationResult",
                    DirectCommunicationMessages.DIRECT_COMMUNICATION_CREATION_FAILED,
                    "Receiver was not found...");

            var directCommunication = new DirectCommunication()
            {
                DirectCommunicationId = Guid.NewGuid(),
                Members = new List<User>() { sender, receiver }
            };

            await _database.DirectCommunications.AddAsync(directCommunication);

            int appliedChanges = await _database.SaveChangesAsync();

            bool wasSuccessful = appliedChanges > 0;

            if (!wasSuccessful)
                return ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("NewDirectCommunicationResult", DirectCommunicationMessages.DIRECT_COMMUNICATION_CREATION_FAILED)
                    .AddResponseOperationMessage("Creation of new direct communication failed...")
                    .AddResponseAffectedUser(newDirectCommunication.SenderId);


            return ServerTransmissionService
                .NewSuccessfulResponseServerTransmission("NewDirectCommunicationResult", DirectCommunicationMessages.DIRECT_COMMUNICATION_CREATION_SUCCEEDED)
                .AddResponseOperationMessage("New direct communication created...")
                .AddResponseAffectedUser(newDirectCommunication.SenderId)
                .AddPublish("NewDirectCommunication", (DirectCommunicationDTO)directCommunication, [newDirectCommunication.ReceiverId, newDirectCommunication.SenderId]);
        }

        public async Task<DirectCommunicationDTO?> GetDirectCommunicationById(Guid id)
        {
            var directCommunication = await _database.DirectCommunications
                .Where(directCommunication => directCommunication.DirectCommunicationId == id)
                .Select(directCommunication => new DirectCommunicationDTO()
                {
                    DirectCommunicationId = directCommunication.DirectCommunicationId,
                    Messages = directCommunication.Messages.Select(message => new MessageDTO()
                    {
                        MessageId = message.MessageId,
                        SenderProfile = message.Sender,
                        LastUpdated = message.LastUpdated
                    }).ToList(),
                    MemberProfiles = directCommunication.Members.Select(member => (UserProfile)member).ToList()
                })
                .SingleOrDefaultAsync();


            if (directCommunication == null)
                return null;

            return directCommunication;
        }

        public async Task<ServerTransmission?> GetDirectMessagesByDirectCommunicationId(DirectCommunicationId id)
        {
            var messages = await _database.DirectCommunications
                .Where(directCommunication => directCommunication.DirectCommunicationId == id.Id)
                .Select(directCommunication =>
                    directCommunication.Messages
                        .OrderBy(message => message.LastUpdated)
                        .Select(message => new MessageDTO
                        {
                            MessageId = message.MessageId,
                            SenderProfile = message.Sender,
                            Content = message.Content,
                            LastUpdated = message.LastUpdated
                        })
                        .ToList()
                ).SingleOrDefaultAsync();

            return messages == null
                ? ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("GetDirectMessagesByDirectCommunicationIdResult", GetMessageMessages.NO_MESSAGES_FOUND)
                    .AddResponseOperationMessage("No messages were found in the direct communication")
                : ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("GetDirectMessagesByDirectCommunicationIdResult", GetMessageMessages.SUCCESSFULLY_RETRIEVED_MESSAGE, messages)
                    .AddResponseOperationMessage("Messages were successfully retrieved");
        }
    }
}
