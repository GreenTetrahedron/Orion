using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.DirectCommuncations.Controllers;
using Orion.Server.DirectCommunications.Repositories;
using Orion.Server.Messages;
using Orion.Server.Users;
using Orion.Server.Users.Repositories;

namespace Orion.Server.Tests.Mocks.Users.Repositories
{
    public class MockDirectCommunicationRepository : IDirectCommunicationRepository
    {
        public MethodCallStack callStack;

        public MockDirectCommunicationRepository()
        {
            callStack = new MethodCallStack();
        }

        public async Task<ServerResult<DirectCommunicationMessages>?> AddDirectCommunication(NewDirectCommunicationDTO newDirectCommunication)
        {
            callStack.NewMethodCall(nameof(this.AddDirectCommunication), [newDirectCommunication]);

            return default;
        }

        public async Task<DirectCommunication?> GetDirectCommunicationById(Guid id)
        {
            callStack.NewMethodCall(nameof(this.GetDirectCommunicationById), [id]);

            return default;
        }

        public async Task<List<DirectCommunication>?> GetDirectCommunicationsByUserId(Guid id)
        {
            callStack.NewMethodCall(nameof(this.GetDirectCommunicationsByUserId), [id]);

            return default;
        }

        public async Task<List<Message>?> GetDirectMessagesByDirectCommunicationId(Guid id)
        {
            callStack.NewMethodCall(nameof(this.GetDirectMessagesByDirectCommunicationId), [id]);

            return default;
        }
    }
}
