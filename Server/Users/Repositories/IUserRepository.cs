using Server.Users;

namespace Server.Users.Repositories
{
    public interface IUserRepository
    {
        public User AddUser(string username);
        public User GetUser(Guid userId);
    }
}