using Orion.Server.DataLayer;
using Orion.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
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
