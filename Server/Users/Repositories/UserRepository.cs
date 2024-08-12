using Orion.Server.DataLayer;
using Orion.Server.Users;

namespace Orion.Server.Users.Repositories
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
                UserId = Guid.NewGuid(),
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