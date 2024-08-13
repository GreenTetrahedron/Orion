using Orion.Models.ServerTransmissions.Results;
using Orion.Server.DataLayer;
using Orion.Server.Tests.Mocks.DataLayer;
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
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task AuthenticateUser_ReturnsTrueForValidUsername(string username)
        {
            await _dataLayer.AddUser(new User()
            {
                UserId = Guid.NewGuid(),
                Username = username
            });

            ServerResult result = await _userRepository.AuthenticateUser(username);

            Assert.That(result.OperationResult.OperationMessage == OperationMessages.VALID_CREDENTIALS);
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task AuthenticateUser_ReturnsFalseForInvalidUsername(string username)
        {
            Assert.That((await _dataLayer.GetAllUsers()).Count == 0, "Invalid test conditions");

            ServerResult result = await _userRepository.AuthenticateUser(username);

            Assert.That(result.OperationResult.OperationMessage == OperationMessages.INVALID_CREDENTIALS);
        }
        [Test]
        public async Task AddUserAddsUserToDB()
        {
            User user = await _userRepository.AddUser("User1");

            User result = await _dataLayer.GetUser(user.UserId);
            Assert.That(result?.Username == "User1", $"Username was: {result?.Username}");
        }


        [Test]
        public async Task GetUserGetsUserFromDB()
        {
            var user = new User()
            {
                UserId = new Guid(),
                Username = "User1"
            };

            await _dataLayer.AddUser(user);

            User result = await _userRepository.GetUser(user.UserId);
            Assert.That(result?.Username == "User1", $"Username was: {result?.Username}");
        }
    }
}