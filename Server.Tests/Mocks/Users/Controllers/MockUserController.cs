using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Attributes;
using Orion.Server.Users.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Tests.Mocks.Users.Controllers
{
    [Controller]
    public class MockUserController : IUserController
    {
        private readonly MockMethodCallStackService _mockMethodCallStackService;

        public MockUserController(MockMethodCallStackService mockMethodCallStackService)
        {
            _mockMethodCallStackService = mockMethodCallStackService;
        }

        [Handler("AuthenticateUser")]
        public async Task<ServerResult<AuthenticationMessages>?> AuthenticateUser(Credentials credentials)
        {
            _mockMethodCallStackService.MethodCallStack.Push($"AuthenticateUser: {credentials.Username}");
            return null;
        }

        [Handler("NewDirectCommunication")]
        public async Task<ServerResult?> AddDirectCommunication(Guid senderId, Guid recipientId)
        {
            _mockMethodCallStackService.MethodCallStack.Push($"AddDirectCommunication: {senderId}, {recipientId}");
            return null;
        }

    }
}
