using Microsoft.EntityFrameworkCore;
using Orion.Cryptography.HashingServices;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;
using Orion.Server.Tests.Mocks.DataLayer;
using Orion.Server.Users;
using Orion.Server.Users.Repositories;

namespace Orion.Server.Tests.Users.Repositories
{
    public class UserRepositoryTests
    {
        private IUserRepository _userRepository;
        private DbContextOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<MockOrionDbContext>().Options;

            var context = new MockOrionDbContext(_options);
            context.Database.EnsureDeleted();
            _userRepository = new UserRepository(context, new HashingService());
        }
        private User CreateUser(string username = "")
        {
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Username = username
            };

            using (var context = new MockOrionDbContext(_options))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return user;
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task AuthenticateUser_ReturnsAuthenticationsMessagesServerResult(string username)
        {
            var credentials = new Credentials() { Username = username };

            ServerTransmission tranmission = await _userRepository.AuthenticateUser(credentials);

            Assert.That(tranmission.Response, Is.Not.Null, "Response was null");
            Assert.That(tranmission.Response.ServerResult, Is.Not.Null, "Result was null");
            Assert.That(tranmission.Response.ServerResult.OperationInformation, Is.Not.Null, "Operation information was null");

            ServerResult<AuthenticationMessages> result;

            Assert.DoesNotThrow(() => result = tranmission.Response.ServerResult, "Wrong type returned...");
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

            using (var context = new MockOrionDbContext(_options))
            {
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();
            }


            var credentials = new Credentials() { Username = username };

            ServerTransmission transmission = await _userRepository.AuthenticateUser(credentials);

            Assert.That(((ServerResult<AuthenticationMessages>)transmission.Response.ServerResult).OperationInformation.OperationMessageCode, Is.EqualTo(AuthenticationMessages.VALID_CREDENTIALS));
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task AuthenticateUser_ReturnsCorrectServerTransmissionForValidUsername(string username)
        {
            var newUser = new User()
            {
                UserId = Guid.NewGuid(),
                Username = username
            };

            using (var context = new MockOrionDbContext(_options))
            {
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();
            }

            var credentials = new Credentials() { Username = username };

            ServerTransmission transmission = await _userRepository.AuthenticateUser(credentials);

            Assert.That(transmission.Response, Is.Not.Null, "Response was null");
            Assert.That(transmission.Response.ServerResult, Is.Not.Null, "Result was null");
            Assert.That(transmission.Response.AffectedUsers.SequenceEqual([newUser.UserId]), "Wrong affected users");
            Assert.That(((UserDTO)transmission.Response.ServerResult.Data).UserId == newUser.UserId
                && ((UserDTO)transmission.Response.ServerResult.Data).Username == newUser.Username, "Wrong user returned");
            Assert.That(transmission.Publish, Is.Null, "Publish was not null");
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task AuthenticateUser_ReturnsCorrectServerTransmissionForInvalidUsername(string username)
        {
            var credentials = new Credentials() { Username = username };

            ServerTransmission transmission = await _userRepository.AuthenticateUser(credentials);

            Assert.That(transmission.Response, Is.Not.Null, "Response was null");
            Assert.That(transmission.Response.ServerResult, Is.Not.Null, "Result was null");
            Assert.That(transmission.Response.AffectedUsers, Is.Null, "Affected user was returned");
            Assert.That(transmission.Response.ServerResult.Data, Is.Null, "User returned");
            Assert.That(transmission.Publish, Is.Null, "Publish was not null");
        }



        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task AuthenticateUser_ReturnsFalseForInvalidUsername(string username)
        {
            var credentials = new Credentials() { Username = username };

            ServerTransmission transmission = await _userRepository.AuthenticateUser(credentials);

            Assert.That(((ServerResult<AuthenticationMessages>)transmission.Response.ServerResult).OperationInformation.OperationMessageCode, Is.EqualTo(AuthenticationMessages.INVALID_CREDENTIALS), "Invalid credentials message was not returned");
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task AddUser_AddsUserToDB(string username)
        {
            var newUser = await _userRepository.AddUser(new Credentials { Username = username, Password = "Password"});

            User result;

            using (var context = new MockOrionDbContext(_options))
            {
                result = await context.Users
                    .Where(user => user.UserId == newUser.UserId)
                    .SingleOrDefaultAsync();
            }

            Assert.That(result, Is.Not.Null, "No user added");
            Assert.That(result.UserId, Is.EqualTo(newUser.UserId), "Wrong userId returned");
            Assert.That(result.Username, Is.EqualTo(newUser.Username), "Wrong username added");
        }


        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task GetUserById_GetsCorrectUserFromDB(string username)
        {
            var newUser = new User()
            {
                UserId = Guid.NewGuid(),
                Username = username
            };

            using (var context = new MockOrionDbContext(_options))
            {
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();
            }

            UserDTO result = await _userRepository.GetUser(newUser.UserId);

            Assert.That(result, Is.Not.Null, "No user added");
            Assert.That(result.UserId, Is.EqualTo(newUser.UserId), "Wrong userId returned");
            Assert.That(result.Username, Is.EqualTo(newUser.Username), "Wrong username returned");
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task GetUserProfileByUsername_ReturnsGetUserMessagesServerResult(string username)
        {
            ServerTransmission? tranmission = await _userRepository.GetUserProfileByUsername(username);

            Assert.That(tranmission, Is.Not.Null, "Transmission was null");
            Assert.That(tranmission.Response, Is.Not.Null, "Response was null");
            Assert.That(tranmission.Response.ServerResult, Is.Not.Null, "Result was null");
            Assert.That(tranmission.Response.ServerResult.OperationInformation, Is.Not.Null, "Operation information was null");

            ServerResult<GetUserMessages> result;

            Assert.DoesNotThrow(() => result = tranmission.Response.ServerResult, "Wrong type returned...");
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task GetUserProfileByUsername_ReturnsTrueForValidUsername(string username)
        {
            var newUser = CreateUser(username);

            ServerTransmission transmission = await _userRepository.GetUserProfileByUsername(username);

            Assert.That(((ServerResult<GetUserMessages>)transmission.Response.ServerResult).OperationInformation.OperationMessageCode, Is.EqualTo(GetUserMessages.USER_FOUND));
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task GetUserProfileByUsername_ReturnsCorrectServerTransmissionForValidUsername(string username)
        {
            var newUser = CreateUser(username);

            ServerTransmission? transmission = await _userRepository.GetUserProfileByUsername(username);

            Assert.That(transmission, Is.Not.Null, "Transmission was null");
            Assert.That(transmission.Response, Is.Not.Null, "Response was null");
            Assert.That(transmission.Response.ServerResult, Is.Not.Null, "Result was null");
            Assert.That(transmission.Publish, Is.Null, "Publish was not null");

            Assert.That(((UserProfile)transmission.Response.ServerResult.Data).UserId == newUser.UserId
                && ((UserProfile)transmission.Response.ServerResult.Data).Username == newUser.Username, "Wrong user returned");
        }

        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task GetUserProfileByUsername_ReturnsCorrectServerTransmissionForInvalidUsername(string username)
        {
            ServerTransmission? transmission = await _userRepository.GetUserProfileByUsername(username);

            Assert.That(transmission, Is.Not.Null, "Transmission was null");
            Assert.That(transmission.Response, Is.Not.Null, "Response was null");
            Assert.That(transmission.Response.ServerResult, Is.Not.Null, "Result was null");
            Assert.That(transmission.Response.AffectedUsers, Is.Null, "Affected user was returned");
            Assert.That(transmission.Response.ServerResult.Data, Is.Null, "User returned");
            Assert.That(transmission.Publish, Is.Null, "Publish was not null");
        }



        [Test]
        [TestCase("User1")]
        [TestCase("American bald eagle")]
        [TestCase("User212343441")]
        public async Task GetUserProfileByUsername_ReturnsFalseForInvalidUsername(string username)
        {
            ServerTransmission? transmission = await _userRepository.GetUserProfileByUsername(username);

            Assert.That(((ServerResult<GetUserMessages>)transmission.Response.ServerResult).OperationInformation.OperationMessageCode, Is.EqualTo(GetUserMessages.USER_NOT_FOUND), "Invalid username message was not returned");
        }

    }
}