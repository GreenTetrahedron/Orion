using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.DataLayer;
using Orion.Server.Messages;
using Orion.Server.ServerResults;
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
        private readonly Database _database;

        public DirectCommunicationRepository(Database database)
        {
            _database = database;
        }

        public async Task<ServerTransmission> AddDirectCommunication(NewDirectCommunicationDTO newDirectCommunication)
        {
            var receiverId = (await _database.UserEntity.GetAllRecords())
                .Where(x => x.Username == newDirectCommunication.ReceiverName)
                .Single()
                .UserId;

            bool wasSuccessful = receiverId != null;

            var directCommunication = new DirectCommunication()
            {
                DirectCommunicationId = Guid.NewGuid(),
                UserIds = new Tuple<Guid, Guid>(newDirectCommunication.SenderId, receiverId)
            };

            wasSuccessful &= await _database.DirectCommunicationEntity.AddRecord(directCommunication.DirectCommunicationId, directCommunication);

            var sender = await _database.UserEntity.GetRecordById(newDirectCommunication.SenderId);

            wasSuccessful &= sender != null;

            sender.DirectCommunicationIds = sender.DirectCommunicationIds == null
                ? new List<Guid> { directCommunication.DirectCommunicationId }
                : sender.DirectCommunicationIds.Append(directCommunication.DirectCommunicationId).ToList();

            wasSuccessful &= await _database.UserEntity.UpdateRecord(sender.UserId, sender);

            var receiver = await _database.UserEntity.GetRecordById(receiverId);

            wasSuccessful &= receiver != null;

            receiver.DirectCommunicationIds = receiver.DirectCommunicationIds == null
                ? new List<Guid> { directCommunication.DirectCommunicationId }
                : receiver.DirectCommunicationIds.Append(directCommunication.DirectCommunicationId).ToList();

            wasSuccessful &= await _database.UserEntity.UpdateRecord(receiver.UserId, receiver);

            return wasSuccessful
                ? ServerResultService.NewSuccessfulResponseServerTransmission("NewDirectCommunicationResult", DirectCommunicationMessages.DIRECT_COMMUNICATION_CREATION_FAILED)
                    .AddResponseOperationMessage("Creation of new direct communication failed...")
                    .AddResponseAffectedUser(newDirectCommunication.SenderId)
                : ServerResultService.NewSuccessfulResponseServerTransmission("NewDirectCommunicationResult", DirectCommunicationMessages.DIRECT_COMMUNICATION_CREATION_SUCCEEDED)
                    .AddResponseOperationMessage("New direct communication created...")
                    .AddResponseAffectedUser(newDirectCommunication.SenderId)
                    .AddPublish("NewDirectCommunication", directCommunication, receiverId);
        }

        public async Task<DirectCommunication?> GetDirectCommunicationById(Guid id)
        {
            return await _database.DirectCommunicationEntity.GetRecordById(id);
        }

        public async Task<List<DirectCommunication>?> GetDirectCommunicationsByUserId(Guid id)
        {
            return (await _database.DirectCommunicationEntity.GetAllRecords())
                .Where(x => x.UserIds.Item1 == id || x.UserIds.Item2 == id)
                .ToList();
        }

        public async Task<List<Message>?> GetDirectMessagesByDirectCommunicationId(Guid id)
        {
            return (await _database.DirectMessageEntity.GetAllRecords())
                .Where(x => x.DirectCommunicationId == id)
                .Select(x => _database.MessageEntity.GetRecordById(x.MessageId).Result)
                .ToList();
        }
    }
}
