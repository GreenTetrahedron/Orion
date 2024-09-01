using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
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

        public async Task<ServerTransmission?> AddDirectCommunication(NewDirectCommunication newDirectCommunication)
        {
            callStack.NewMethodCall(nameof(this.AddDirectCommunication), [newDirectCommunication]);

            return default;
        }

        public async Task<DirectCommunicationDTO?> GetDirectCommunicationById(Guid id)
        {
            callStack.NewMethodCall(nameof(this.GetDirectCommunicationById), [id]);

            return default;
        }

        public async Task<ServerTransmission?> GetDirectMessagesByDirectCommunicationId(DirectCommunicationId id)
        {
            callStack.NewMethodCall(nameof(this.GetDirectMessagesByDirectCommunicationId), [id]);

            return default;
        }
    }
}
