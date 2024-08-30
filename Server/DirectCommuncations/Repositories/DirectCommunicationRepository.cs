using Microsoft.EntityFrameworkCore;
using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.UserModels;
using Orion.Server.DataLayer;
using Orion.Server.DirectCommuncations;
using Orion.Server.Exceptions;
using Orion.Server.Messages;
using Orion.Server.ServerTransmissionServices;
using Orion.Server.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
                throw new UserNotFoundException("Sender was null");

            if (receiver == null)
                throw new UserNotFoundException("Receiver was null");

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
                .AddPublish("NewDirectCommunication", (DirectCommunicationDTO)directCommunication, newDirectCommunication.ReceiverId);
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
                        SenderProfile = message.Sender
                    }).ToList(),
                    MemberProfiles = directCommunication.Members.Select(member => (UserProfile)member).ToList()
                })
                .SingleOrDefaultAsync();


            if (directCommunication == null)
                return null;

            return directCommunication;
        }

        public async Task<List<MessageDTO>?> GetDirectMessagesByDirectCommunicationId(Guid id)
        {
            return await _database.DirectCommunications
                .Where(directCommunication => directCommunication.DirectCommunicationId == id)
                .Select(directCommunication => 
                    directCommunication.Messages
                        .Select(x => new MessageDTO
                        {
                            MessageId = x.MessageId,
                            SenderProfile = x.Sender,
                            Content = x.Content
                        }).ToList()
                ).SingleOrDefaultAsync();
        }
    }
}
