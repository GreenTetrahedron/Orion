using Orion.Server.DataLayer;
using Orion.Server.Users;
using Orion.Server.Users.Repositories;

namespace Orion.Server.Tests.Users.Repositories
{
    public class UserRepositoryTests
    {
        private IDataLayer _dataLayer;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _dataLayer = new MockDB();
            _userRepository = new UserRepository(_dataLayer);
        }

        [Test]
        public void AddUserAddsUserToDB()
        {
            User user = _userRepository.AddUser("User1");

            User result = _dataLayer.GetUser(user.UserId);
            Assert.That(result?.Username == "User1", $"Username was: {result?.Username}");
        }


        [Test]
        public void GetUserGetsUserFromDB()
        {
            var user = new User()
            {
                UserId = new Guid(),
                Username = "User1"
            };

            _dataLayer.AddUser(user);

            User result = _userRepository.GetUser(user.UserId);
            Assert.That(result?.Username == "User1", $"Username was: {result?.Username}");
        }
    }
}