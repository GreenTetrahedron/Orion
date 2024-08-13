using Orion.Server.DataLayer;
using Orion.Server.Messages;
using Orion.Server.Messages.Repositories;
using Orion.Server.Tests.Mocks.DataLayer;

namespace Orion.Server.Tests.Messages.Repositories
{
    public class MessageRepositoryTests
    {
        private IDataLayer _dataLayer;
        private IMessageRepository _messageRepository;

        [SetUp]
        public void Setup()
        {
            _dataLayer = new MockDB();
            _messageRepository = new MessageRepository(_dataLayer);
        }

        [Test]
        public async Task AddMessageAddsMessageToDB()
        {
            Message message = await _messageRepository.AddMessage(new Guid(), "Message1");

            Message result = await _dataLayer.GetMessage(message.MessageId);
            Assert.That(result?.Content == "Message1", $"Content was: {result?.Content}");
        }


        [Test]
        public async Task GetMessageGetsMessageFromDB()
        {
            var message = new Message()
            {
                MessageId = new Guid(),
                SenderId = new Guid(),
                Content = "Message1"
            };

            await _dataLayer.AddMessage(message);

            Message result = await _messageRepository.GetMessage(message.MessageId);
            Assert.That(result?.Content == "Message1", $"Messagename was: {result?.Content}");
        }
    }
}