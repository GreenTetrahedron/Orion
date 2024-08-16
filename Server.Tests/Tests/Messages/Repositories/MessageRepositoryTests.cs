using Orion.Server.DataLayer;
using Orion.Server.Messages;
using Orion.Server.Messages.Repositories;
using Orion.Server.Tests.Mocks.DataLayer;

namespace Orion.Server.Tests.Messages.Repositories
{
    public class MessageRepositoryTests
    {
        private Database _mockDB;
        private IMessageRepository _messageRepository;

        [SetUp]
        public void Setup()
        {
            _mockDB = new MockDB();
            _messageRepository = new MessageRepository(_mockDB);
        }

        [Test]
        public async Task AddMessageAddsMessageToDB()
        {
            Message message = await _messageRepository.AddMessage(Guid.NewGuid(), "Message1");

            Message result = await _mockDB.MessageEntity.GetRecordById(message.MessageId);
            Assert.That(result?.Content == "Message1", $"Content was: {result?.Content}");
        }


        [Test]
        public async Task GetMessageGetsMessageFromDB()
        {
            var message = new Message()
            {
                MessageId = Guid.NewGuid(),
                SenderId = Guid.NewGuid(),
                Content = "Message1"
            };

            await _mockDB.MessageEntity.AddRecord(message.MessageId, message);

            Message result = await _messageRepository.GetMessage(message.MessageId);
            Assert.That(result?.Content == "Message1", $"Messagename was: {result?.Content}");
        }
    }
}