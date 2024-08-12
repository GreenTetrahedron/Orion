using Orion.Server.DataLayer;
using Orion.Server.Messages;
using Orion.Server.Messages.Repositories;

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
        public void AddMessageAddsMessageToDB()
        {
            Message message = _messageRepository.AddMessage(new Guid(), "Message1");

            Message result = _dataLayer.GetMessage(message.MessageId);
            Assert.That(result?.Content == "Message1", $"Content was: {result?.Content}");
        }


        [Test]
        public void GetMessageGetsMessageFromDB()
        {
            var message = new Message()
            {
                MessageId = new Guid(),
                SenderId = new Guid(),
                Content = "Message1"
            };

            _dataLayer.AddMessage(message);

            Message result = _messageRepository.GetMessage(message.MessageId);
            Assert.That(result?.Content == "Message1", $"Messagename was: {result?.Content}");
        }
    }
}