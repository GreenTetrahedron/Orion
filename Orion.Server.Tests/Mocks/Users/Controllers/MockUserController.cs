using Orion.Models.ServerTransmissions;
using Orion.Models.UserModels;
using Orion.Server.Attributes;
using Orion.Server.Users.Controllers;

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
        public async Task<ServerTransmission?> AuthenticateUser(Credentials credentials)
        {
            callStack.NewMethodCall(nameof(this.AuthenticateUser), [credentials]);
            return default;
        }

        [Handler("GetUserByUsername")]
        public Task<ServerTransmission?> GetUserByUsername(string username)
        {
            callStack.NewMethodCall(nameof(this.AuthenticateUser), [username]);
            return default;
        }
    }
}
