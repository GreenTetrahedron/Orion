using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.DataLayer;
using Orion.Server.DirectCommunications;
using Orion.Server.Tests.Mocks.DataLayer;
using Orion.Server.Users;
using Orion.Server.Users.Repositories;

namespace Orion.Server.Tests.Users.Repositories
{
    public class UserRepositoryTests
    {
        private Database _mockDB;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _mockDB = new MockDB();
            _userRepository = new UserRepository(_mockDB);
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task AuthenticateUser_ReturnsTrueForValidUsername(string username)
        {
            var newUser = new User()
            {
                UserId = Guid.NewGuid(),
                Username = username
            };

            await _mockDB.UserEntity.AddRecord(newUser.UserId, newUser);

            var credentials = new Credentials() { Username = username };

            ServerResult<AuthenticationMessages> result = await _userRepository.AuthenticateUser(credentials);

            Assert.That(result.OperationInformation.OperationMessageCode == AuthenticationMessages.VALID_CREDENTIALS);
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task AuthenticateUser_ReturnsFalseForInvalidUsername(string username)
        {
            Assert.That((await _mockDB.UserEntity.GetAllRecords()).ToList().Count == 0, "Invalid test conditions");
            
            var credentials = new Credentials() { Username = username };

            ServerResult<AuthenticationMessages> result = await _userRepository.AuthenticateUser(credentials);

            Assert.That(result.OperationInformation.OperationMessageCode == AuthenticationMessages.INVALID_CREDENTIALS);
        }
        [Test]
        public async Task AddUserAddsUserToDB()
        {
            User user = await _userRepository.AddUser("User1");

            User result = await _mockDB.UserEntity.GetRecordById(user.UserId);
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

            await _mockDB.UserEntity.AddRecord(user.UserId, user);

            User result = await _userRepository.GetUser(user.UserId);
            Assert.That(result?.Username == "User1", $"Username was: {result?.Username}");
        }

        [Test]
        public async Task AddDirectCommunication_ReturnsValidDirectCommunication()
        {
            var senderId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();

            ServerResult? result = await _userRepository.AddDirectCommunication(senderId, receiverId);

            Assert.IsNotNull(result, "Result was null");

            DirectCommunication? directCommunication = (DirectCommunication?)result.Data;

            Assert.IsNotNull(directCommunication, "DirectCommunication was null");

            DirectCommunication? storedDirectCommunication = await _mockDB.DirectCommunicationEntity.GetRecordById(directCommunication.DirectCommunicationId);

            Assert.That(directCommunication.DirectCommunicationId == storedDirectCommunication.DirectCommunicationId
                        && directCommunication.UserIds == storedDirectCommunication.UserIds,
                        "Wrong DirectCommunication stored...");
        }
    }
}