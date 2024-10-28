using Orion.Models.ServerTransmissions;
using Orion.Models.UserModels;
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

        public async Task<UserDTO?> AddUser(Credentials credentials)
        {
            methodCallStack.Push($"AddUser: {credentials.Username} {credentials.Password}");
            return null;
        }

        public async Task<ServerTransmission?> AuthenticateUser(Credentials credentials)
        {
            methodCallStack.Push($"AuthenticateUser: {credentials.Username}");
            return null;
        }

        public async Task<UserDTO?> GetUser(Guid userId)
        {
            methodCallStack.Push($"GetUser: {userId}");
            return null;
        }

        public async Task<ServerTransmission?> GetUserProfileByUsername(string username)
        {
            methodCallStack.Push($"{nameof(GetUserProfileByUsername)}: {username}");
            return null;
        }
    }
}
