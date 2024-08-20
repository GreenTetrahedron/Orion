using Orion.Server.DataLayer;
using Orion.Server.DirectCommunications;
using Orion.Server.DirectCommunications.Repositories;
using Orion.Server.Messages;
using Orion.Server.Messages.Repositories;
using Orion.Server.Tests.Mocks.DataLayer;

namespace Orion.Server.Tests.DirectCommunications.Repositories
{
    public class DirectCommunicationRepositoryTests
    {
        private Database _mockDB;
        private IDirectCommunicationRepository _directCommunicationRepository;

        [SetUp]
        public void Setup()
        {
            _mockDB = new MockDB();
            _directCommunicationRepository = new DirectCommunicationRepository(_mockDB);
        }

        [Test]
        public async Task GetDirectCommunicationById_ReturnsCorrectDirectCommunication()
        {
            var directCommunication = new DirectCommunication()
            {
                DirectCommunicationId = Guid.NewGuid(),
                UserIds = new Tuple<Guid, Guid>(Guid.NewGuid(), Guid.NewGuid())
            };

            await _mockDB.DirectCommunicationEntity.AddRecord(directCommunication.DirectCommunicationId, directCommunication);

            var result = await _directCommunicationRepository.GetDirectCommunicationById(directCommunication.DirectCommunicationId);

            Assert.That(result, Is.Not.Null, "Result was null...");

            Assert.That(directCommunication.UserIds == result.UserIds
                && directCommunication.DirectMessageIds == result.DirectMessageIds,
                "Wrong directCommuniction returned...");
        }

        [Test]
        [TestCase("Message1")]
        public async Task GetDirectMessagesByDirectCommunicationId_ReturnsCorrectDirectMessages(string content)
        {
            var directCommunicationId = Guid.NewGuid();
            
            var message = new Message()
            {
                MessageId = Guid.NewGuid(),
                Content = content,
                SenderId = Guid.NewGuid()
            };

            var directMessage = new DirectMessage()
            {
                DirectCommunicationId = directCommunicationId,
                DirectMessageId = Guid.NewGuid(),
                MessageId = message.MessageId
            };

            await _mockDB.MessageEntity.AddRecord(message.MessageId, message);

            await _mockDB.DirectMessageEntity.AddRecord(directMessage.DirectMessageId, directMessage);

            var result = await _directCommunicationRepository.GetDirectMessagesByDirectCommunicationId(directCommunicationId);

            Assert.That(result, Is.Not.Null, "Result was null...");

            Assert.That(result.Count == 1
                && result[0].Content == message.Content,
                "Wrong messages returned...");
        }

        [Test]
        public async Task GetDirectCommunicationsByUserId_ReturnsCorrectDirectCommunications()
        {
            var userOneId = Guid.NewGuid();
            var userTwoId = Guid.NewGuid();

            var directCommunication = new DirectCommunication()
            {
                DirectCommunicationId = Guid.NewGuid(),
                UserIds = new Tuple<Guid, Guid>(userOneId, userTwoId)
            };

            await _mockDB.DirectCommunicationEntity.AddRecord(directCommunication.DirectCommunicationId, directCommunication);

            var result = await _directCommunicationRepository.GetDirectCommunicationsByUserId(userOneId);

            Assert.That(result, Is.Not.Null, "Result was null for user 1...");

            Assert.That(result.Count == 1
                && directCommunication.UserIds == result[0].UserIds
                && directCommunication.DirectMessageIds == result[0].DirectMessageIds,
                "Wrong messages returned for user 1...");


            result = await _directCommunicationRepository.GetDirectCommunicationsByUserId(userTwoId);

            Assert.That(result, Is.Not.Null, "Result was null for user 2...");

            Assert.That(result.Count == 1
                && directCommunication.UserIds == result[0].UserIds
                && directCommunication.DirectMessageIds == result[0].DirectMessageIds,
                "Wrong messages returned for user 2...");
        }
    }
}