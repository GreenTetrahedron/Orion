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
        public MethodCallStack callStack;

        public MockUserController(MethodCallStack methodCallStack)
        {
            callStack = methodCallStack;
        }

        [Handler("AuthenticateUser")]
        public async Task<ServerResult<AuthenticationMessages>?> AuthenticateUser(Credentials credentials)
        {
            callStack.NewMethodCall(nameof(this.AuthenticateUser), [credentials]);
            return default;
        }
    }
}
