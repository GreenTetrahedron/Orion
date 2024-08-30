using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Orion.Models.MessageModels;
using Orion.Server.DataLayer;
using Orion.Server.DirectCommunications.Repositories;
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
                Username = username
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

        [Test]
        [TestCase("Message1")]
        public async Task AddMessageAddsMessageToDB(string content)
        {
            var sender = CreateUser();
            MessageDTO? returnedMessage = await _messageRepository.AddMessage(new NewMessage() { Content = content, SenderId = sender.UserId });
            
            Assert.That(returnedMessage, Is.Not.Null, "Returned message was null");

            MessageDTO? message;

            using (var context = new MockOrionDbContext(_options))
            {
                message = await context.Messages
                    .Where(message => message.MessageId == returnedMessage.MessageId)
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


        [Test]
        [TestCase("Message1")]
        public async Task GetMessageGetsMessageFromDB(string content)
        {
            var message = CreateMessage(content: content);

            using (var context = new MockOrionDbContext(_options))
            {
                context.Attach(message.Sender);
                await context.Messages.AddAsync(message);
                await context.SaveChangesAsync();
            }

            MessageDTO? result = await _messageRepository.GetMessage(message.MessageId);

            Assert.That(result, Is.Not.Null, "Message was null");
            Assert.That(result.Content, Is.EqualTo(content), $"Content was: {message.Content}, when added content was: {content}");
        }
    }
}