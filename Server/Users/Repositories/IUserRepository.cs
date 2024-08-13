using Orion.Server.Users;

namespace Orion.Server.Users.Repositories
{
    public interface IUserRepository
    {
        public Task<dynamic> AuthenticateUser(string username);
        public Task<dynamic> AddUser(string username);
        public Task<dynamic> GetUser(Guid userId);
    }
}