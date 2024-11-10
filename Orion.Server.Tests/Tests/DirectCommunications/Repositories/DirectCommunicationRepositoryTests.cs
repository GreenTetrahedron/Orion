using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.UserModels;
using Orion.Server.DirectCommuncations;
using Orion.Server.DirectCommunications.Repositories;
using Orion.Server.Messages;
using Orion.Server.Tests.Mocks.DataLayer;
using Orion.Server.Users;

namespace Orion.Server.Tests.DirectCommunications.Repositories
{
    public class DirectCommunicationRepositoryTests
    {
        private IDirectCommunicationRepository _directCommunicationRepository;

        private static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();
        private DbContextOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<MockOrionDbContext>()
                .UseInMemoryDatabase("OrionDatabase", InMemoryDatabaseRoot)
                .Options;

            var context = new MockOrionDbContext(_options);
            context.Database.EnsureDeleted();
            _directCommunicationRepository = new DirectCommunicationRepository(context);
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

        private Message CreateMessage(string content = "") =>
            new Message()
            {
                MessageId = Guid.NewGuid(),
                SenderId = Guid.NewGuid(),
                Content = content
            };
        private Message CreateMessage(Guid senderId, string content = "") =>
            new Message()
            {
                MessageId = Guid.NewGuid(),
                SenderId = senderId,
                Content = content
            };
        private Message CreateMessage(User sender, string content = "")
        {
            var message = new Message()
            {
                MessageId = Guid.NewGuid(),
                Sender = sender,
                Content = content
            };

            using (var context = new MockOrionDbContext(_options))
            {
                context.Attach(sender);
                context.Add(message);
                context.SaveChanges();
            }

            return message;
        }

        private DirectCommunication CreateDirectCommunication(string senderName = "", string receiverName = "", string messageContent = "")
        {
            var sender = CreateUser(senderName);
            return new DirectCommunication()
            {
                DirectCommunicationId = Guid.NewGuid(),
                Members = new List<User> { sender, CreateUser(receiverName) },
                Messages = new List<Message> { CreateMessage(sender, messageContent) }
            };
        }

        [Test]
        public async Task GetDirectCommunicationById_ReturnsCorrectDirectCommunication()
        {
            var directCommunication = CreateDirectCommunication();

            using (var context = new MockOrionDbContext(_options))
            {
                directCommunication.Members.ForEach(member => context.Attach(member));
                directCommunication.Messages.ForEach(message => context.Attach(message));
                await context.DirectCommunications.AddAsync(directCommunication);
                await context.SaveChangesAsync();
            }

            var result = await _directCommunicationRepository.GetDirectCommunicationById(directCommunication.DirectCommunicationId);

            Assert.That(result, Is.Not.Null, "Result was null...");
            Assert.That(result.DirectCommunicationId, Is.EqualTo(directCommunication.DirectCommunicationId), "Wrong direct communication id");


            Assert.That(result.MemberProfiles.Count == directCommunication.Members.Count
                && result.MemberProfiles.Exists(user => user.UserId == directCommunication.Members[0].UserId)
                && result.MemberProfiles.Exists(user => user.UserId == directCommunication.Members[1].UserId),
                "Wrong members");
        }

        [Test]
        [TestCase("Message1")]
        public async Task GetDirectMessagesByDirectCommunicationId_ReturnsCorrectDirectMessages(string content)
        {
            var directCommunication = CreateDirectCommunication(messageContent: content);

            using (var context = new MockOrionDbContext(_options))
            {
                directCommunication.Members.ForEach(member => context.Attach(member));
                directCommunication.Messages.ForEach(message => context.Attach(message));
                await context.DirectCommunications.AddAsync(directCommunication);
                await context.SaveChangesAsync();
            }

            var thing = await _directCommunicationRepository.GetDirectMessagesByDirectCommunicationId(new DirectCommunicationId { Id = directCommunication.DirectCommunicationId });

            var result = (List<MessageDTO>)thing.Response.ServerResult.Data;

            Assert.That(result, Is.Not.Null, "Result was null...");
            Assert.That(result.Count == directCommunication.Messages.Count
                && result.Exists(message => message.MessageId == directCommunication.Messages[0].MessageId
                                    && message.Content == directCommunication.Messages[0].Content),
                "Wrong messages returned");
        }

        [Test]
        public async Task AddDirectCommunication_ReturnsCorrectDirectCommunication()
        {
            var sender = CreateUser("Sender");
            var receiver = CreateUser("Receiver");

            var newDirectCommunication = new NewDirectCommunication() { SenderId = sender.UserId, ReceiverId = receiver.UserId };

            ServerTransmission? result = await _directCommunicationRepository.AddDirectCommunication(newDirectCommunication);

            Assert.That(result, Is.Not.Null, "Result was null");

            DirectCommunicationDTO? directCommunication = (DirectCommunicationDTO?)result.Publish.ServerResult.Data;

            Assert.That(directCommunication, Is.Not.Null, "DirectCommunication was null");

            Assert.That(directCommunication.MemberProfiles.Count == 2
                && directCommunication.MemberProfiles.Exists(user => user.UserId == sender.UserId)
                && directCommunication.MemberProfiles.Exists(user => user.UserId == receiver.UserId),
                "Wrong members");
        }

        [Test]
        public async Task AddDirectCommunication_AddsCorrectDirectCommunication()
        {
            var sender = CreateUser("Sender");
            var receiver = CreateUser("Receiver");

            var newDirectCommunication = new NewDirectCommunication() { SenderId = sender.UserId, ReceiverId = receiver.UserId };

            ServerTransmission? result = await _directCommunicationRepository.AddDirectCommunication(newDirectCommunication);

            Assert.That(result, Is.Not.Null, "Result was null");

            DirectCommunicationDTO returnedDirectCommunication = (DirectCommunicationDTO)result.Publish.ServerResult.Data;

            DirectCommunicationDTO? directCommunication;

            using (var context = new MockOrionDbContext(_options))
            {
                directCommunication = await context.DirectCommunications
                    .Where(directCommunication => directCommunication.DirectCommunicationId == returnedDirectCommunication.DirectCommunicationId)
                    .Select(directCommunication => new DirectCommunicationDTO()
                    {
                        DirectCommunicationId = directCommunication.DirectCommunicationId,
                        MemberProfiles = directCommunication.Members.Select(user => (UserProfile)user).ToList()
                    })
                    .SingleOrDefaultAsync();
            }
            Assert.That(directCommunication, Is.Not.Null, "DirectCommunication was null");

            Assert.That(directCommunication.MemberProfiles.Count == 2
                && directCommunication.MemberProfiles.Exists(user => user.UserId == sender.UserId)
                && directCommunication.MemberProfiles.Exists(user => user.UserId == receiver.UserId),
                "Wrong members");
        }

        [Test]
        public async Task AddDirectCommunication_UpdatesUserEntities()
        {
            var sender = CreateUser("Sender");
            var receiver = CreateUser("Receiver");

            var newDirectCommunication = new NewDirectCommunication() { SenderId = sender.UserId, ReceiverId = receiver.UserId };

            ServerTransmission? result = await _directCommunicationRepository.AddDirectCommunication(newDirectCommunication);

            using (var context = new MockOrionDbContext(_options))
            {
                sender = await context.Users
                    .Where(user => user.UserId == sender.UserId)
                    .Select(user => new User
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        DirectCommunications = user.DirectCommunications
                    })
                    .SingleAsync();

                receiver = await context.Users
                    .Where(user => user.UserId == receiver.UserId)
                    .Select(user => new User
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        DirectCommunications = user.DirectCommunications
                    })
                    .SingleAsync();
            }

            Assert.That(sender.DirectCommunications.Count > 0, "No direct communication added to sender");
            Assert.That(receiver.DirectCommunications.Count > 0, "No direct communication added to receiver");
        }
    }
}