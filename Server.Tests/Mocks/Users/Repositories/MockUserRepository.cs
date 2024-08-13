using Orion.Server.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Tests.Mocks.Users.Repositories
{
    public class MockUserRepository : IUserRepository
    {
        public Stack<string> methodCallStack;

        public MockUserRepository()
        {
            methodCallStack = new Stack<string>();
        }

        public async Task<dynamic> AddUser(string username)
        {
            methodCallStack.Push($"AddUser: {username}");
            return null;
        }

        public async Task<dynamic> AuthenticateUser(string username)
        {
            methodCallStack.Push($"AuthenticateUser: {username}");
            return null;
        }

        public async Task<dynamic> GetUser(Guid userId)
        {
            methodCallStack.Push($"GetUser: {userId}");
            return null;
        }
    }
}
