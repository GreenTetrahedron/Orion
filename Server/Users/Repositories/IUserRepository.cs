using Orion.Server.Users;

namespace Orion.Server.Users.Repositories
{
    public interface IUserRepository
    {
        public dynamic AddUser(string username);
        public dynamic GetUser(Guid userId);
    }
}