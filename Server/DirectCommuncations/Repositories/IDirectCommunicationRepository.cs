using Orion.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DirectCommunications.Repositories
{
    public interface IDirectCommunicationRepository
    {
        public Task<DirectCommunication?> GetDirectCommunicationById(Guid id);

        public Task<List<Message>?> GetDirectMessagesByDirectCommunicationId(Guid id);

        public Task<List<DirectCommunication>?> GetDirectCommunicationsByUserId(Guid id);
    }
}
