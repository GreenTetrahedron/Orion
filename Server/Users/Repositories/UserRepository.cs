using Server.DataLayer;
using Server.Users;

namespace Server.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataLayer _dataLayer;

        public UserRepository(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        public dynamic AddUser(string username)
        {
            var user = new User() { 
                UserId = new Guid(),
                Username = username
            };

            _dataLayer.AddUser(user);

            return user;
        }

        public dynamic GetUser(Guid userId)
        {
            return _dataLayer.GetUser(userId);
        }
    }
}