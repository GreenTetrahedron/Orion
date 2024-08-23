using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Users;
using Orion.Server.Users.Repositories;

namespace Orion.Server.Tests.Mocks.Users.Repositories
{
    public class MockUserRepository : IUserRepository
    {
        public Stack<string> methodCallStack;

        public MockUserRepository()
        {
            methodCallStack = new Stack<string>();
        }

        public async Task<ServerResult?> AddDirectCommunication(Guid senderId, Guid recipientId)
        {
            methodCallStack.Push($"AddDirectCommunication: {senderId}, {recipientId}");
            return null;
        }

        public async Task<User?> AddUser(string username)
        {
            methodCallStack.Push($"AddUser: {username}");
            return null;
        }

        public async Task<ServerResult<AuthenticationMessages>?> AuthenticateUser(Credentials credentials)
        {
            methodCallStack.Push($"AuthenticateUser: {credentials.Username}");
            return null;
        }

        public async Task<User?> GetUser(Guid userId)
        {
            methodCallStack.Push($"GetUser: {userId}");
            return null;
        }
    }
}
