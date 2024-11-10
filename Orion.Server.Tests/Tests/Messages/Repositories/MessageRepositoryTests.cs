using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Server.DirectCommuncations;
using Orion.Server.Messages;
using Orion.Server.Messages.Repositories;
using Orion.Server.Tests.Mocks.DataLayer;
using Orion.Server.Users;

namespace Orion.Server.Tests.Messages.Repositories
{
    public class MessageRepositoryTests
    {
        private IMessageRepository _messageRepository;

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
            _messageRepository = new MessageRepository(context);
        }

        private User CreateUser(string username = "")
        {
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Username = username,
                PasswordHash = [1, 2, 3]
            };

            using (var context = new MockOrionDbContext(_options))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return user;
        }

        private Message CreateMessage(string username = "", string content = "") =>
            new Message()
            {
                MessageId = Guid.NewGuid(),
                Sender = CreateUser(username),
                Content = content
            };
        private Message CreateMessage(Guid senderId, string content = "") =>
            new Message()
            {
                MessageId = Guid.NewGuid(),
                SenderId = senderId,
                Content = content
            };
        private Message CreateMessage(User sender, string content = "") =>
            new Message()
            {
                MessageId = Guid.NewGuid(),
                Sender = sender,
                Content = content
            };

        private DirectCommunication CreateDirectCommunication(string senderName = "", string receiverName = "")
        {
            var sender = CreateUser(senderName);

            var directCommunication = new DirectCommunication()
            {
                DirectCommunicationId = Guid.NewGuid(),
                Members = new List<User> { sender, CreateUser(receiverName) },
            };

            using (var context = new MockOrionDbContext(_options))
            {
                context.Attach(sender);
                context.Attach(directCommunication.Members[1]);
                context.DirectCommunications.Add(directCommunication);
                context.SaveChanges();
            }

            return directCommunication;
        }

        [Test]

        [TestCase("Message1")]
        public async Task AddDirectMessage_UpdatesDirectCommunication(string content)
        {
            var newDirectCommunication = CreateDirectCommunication("Sender");
            ServerTransmission? transmission = await _messageRepository.AddDirectMessage(new NewDirectMessage() { DirectCommunicationId = newDirectCommunication.DirectCommunicationId, Content = content, SenderId = newDirectCommunication.Members[0].UserId });

            DirectMessageDTO directMessage = (DirectMessageDTO)transmission.Publish.ServerResult.Data;

            Assert.That(directMessage, Is.Not.Null, "Returned message was null");

            DirectCommunication? directCommunication;

            using (var context = new MockOrionDbContext(_options))
            {
                directCommunication = await context.DirectCommunications
                    .Where(directCommunication => directCommunication.DirectCommunicationId == newDirectCommunication.DirectCommunicationId)
                    .Select(directCommunication => new DirectCommunication()
                    {
                        DirectCommunicationId = directCommunication.DirectCommunicationId,
                        Messages = directCommunication.Messages
                    })
                    .SingleOrDefaultAsync();
            }

            Assert.That(directCommunication, Is.Not.Null, "direct communication was null");
            Assert.That(directCommunication.Messages.Count == 1, $"No message added to direct communication");
        }

        [Test]
        [TestCase("Message1")]
        public async Task AddDirectMessage_AddsMessageToDB(string content)
        {
            var newDirectCommunication = CreateDirectCommunication("Sender");
            ServerTransmission? transmission = await _messageRepository.AddDirectMessage(new NewDirectMessage() { DirectCommunicationId = newDirectCommunication.DirectCommunicationId, Content = content, SenderId = newDirectCommunication.Members[0].UserId });

            DirectMessageDTO directMessage = (DirectMessageDTO)transmission.Publish.ServerResult.Data;

            Assert.That(directMessage, Is.Not.Null, "Returned message was null");

            MessageDTO? message;

            using (var context = new MockOrionDbContext(_options))
            {
                message = await context.Messages
                    .Where(message => message.MessageId == directMessage.Message.MessageId)
                    .Select(message => new MessageDTO()
                    {
                        MessageId = message.MessageId,
                        Content = message.Content,
                        SenderProfile = message.Sender
                    })
                    .SingleOrDefaultAsync();
            }

            Assert.That(message, Is.Not.Null, "Message was null");
            Assert.That(message.Content, Is.EqualTo(content), $"Content was: {message.Content}, when added content was: {content}");
        }
    }
}