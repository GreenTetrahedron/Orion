using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
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
            var sender = new User() { UserId = Guid.NewGuid() };
            var receiver = new User() { UserId = Guid.NewGuid() };

            await _mockDB.UserEntity.AddRecord(sender.UserId, sender);
            await _mockDB.UserEntity.AddRecord(receiver.UserId, receiver);

            var directCommunication = new DirectCommunication()
            {
                DirectCommunicationId = Guid.NewGuid(),
                UserIds = new Tuple<Guid, Guid>(sender.UserId, receiver.UserId)
            };

            await _mockDB.DirectCommunicationEntity.AddRecord(directCommunication.DirectCommunicationId, directCommunication);

            var result = await _directCommunicationRepository.GetDirectCommunicationsByUserId(sender.UserId);

            Assert.That(result, Is.Not.Null, "Result was null for user 1...");

            Assert.That(result.Count == 1
                && directCommunication.UserIds == result[0].UserIds
                && directCommunication.DirectMessageIds == result[0].DirectMessageIds,
                "Wrong messages returned for user 1...");


            result = await _directCommunicationRepository.GetDirectCommunicationsByUserId(receiver.UserId);

            Assert.That(result, Is.Not.Null, "Result was null for user 2...");

            Assert.That(result.Count == 1
                && directCommunication.UserIds == result[0].UserIds
                && directCommunication.DirectMessageIds == result[0].DirectMessageIds,
                "Wrong messages returned for user 2...");
        }

        [Test]
        public async Task AddDirectCommunication_ReturnsValidDirectCommunication()
        {
            var sender = new User() { UserId = Guid.NewGuid() };
            var receiver = new User() { UserId = Guid.NewGuid(), Username = "User" };

            await _mockDB.UserEntity.AddRecord(sender.UserId, sender);
            await _mockDB.UserEntity.AddRecord(receiver.UserId, receiver);

            var newDirectCommunication = new NewDirectCommunicationDTO() {SenderId = sender.UserId, ReceiverName = receiver.Username};

            ServerResult? result = await _directCommunicationRepository.AddDirectCommunication(newDirectCommunication);

            Assert.IsNotNull(result, "Result was null");

            DirectCommunication? directCommunication = (DirectCommunication?)result.Data;

            Assert.IsNotNull(directCommunication, "DirectCommunication was null");

            DirectCommunication? storedDirectCommunication = await _mockDB.DirectCommunicationEntity.GetRecordById(directCommunication.DirectCommunicationId);

            Assert.That(directCommunication.DirectCommunicationId == storedDirectCommunication.DirectCommunicationId
                        && directCommunication.UserIds.Item1 == sender.UserId
                        && directCommunication.UserIds.Item2 == receiver.UserId,
                        "Wrong DirectCommunication returned...");
        }

        [Test]
        public async Task AddDirectCommunication_AddsDirectCommunicationToDBWithCorrectAttributes()
        {
            var sender = new User() { UserId = Guid.NewGuid(), Username = "User1" };
            var receiver = new User() { UserId = Guid.NewGuid(), Username = "User2" };

            await _mockDB.UserEntity.AddRecord(sender.UserId, sender);
            await _mockDB.UserEntity.AddRecord(receiver.UserId, receiver);

            var newDirectCommunication = new NewDirectCommunicationDTO() {SenderId = sender.UserId, ReceiverName = receiver.Username };

            ServerResult? result = await _directCommunicationRepository.AddDirectCommunication(newDirectCommunication);

            DirectCommunication directCommunication = result.Data as DirectCommunication;

            DirectCommunication? storedDirectCommunication = await _mockDB.DirectCommunicationEntity.GetRecordById(directCommunication.DirectCommunicationId);

            Assert.That(storedDirectCommunication.UserIds.Item1 == sender.UserId && storedDirectCommunication.UserIds.Item2 == receiver.UserId,
                        "Wrong DirectCommunication stored...");
        }

        [Test]
        public async Task AddDirectCommunication_UpdatesUserEntities()
        {
            var sender = new User() { UserId = Guid.NewGuid(), Username = "User1" };
            var receiver = new User() { UserId = Guid.NewGuid(), Username = "User2" };

            await _mockDB.UserEntity.AddRecord(sender.UserId, sender);
            await _mockDB.UserEntity.AddRecord(receiver.UserId, receiver);

            var newDirectCommunication = new NewDirectCommunicationDTO() { SenderId = sender.UserId, ReceiverName = receiver.Username };

            ServerResult? result = await _directCommunicationRepository.AddDirectCommunication(newDirectCommunication);

            DirectCommunication directCommunication = result.Data as DirectCommunication;

            User? storedSender = await _mockDB.UserEntity.GetRecordById(sender.UserId);

            Assert.IsNotNull(storedSender, "Stored sender was null");

            Assert.That(storedSender.DirectCommunicationIds.Contains(directCommunication.DirectCommunicationId), "Sender did not contain directCommunication");

            User? storedReceiver = await _mockDB.UserEntity.GetRecordById(receiver.UserId);

            Assert.IsNotNull(storedReceiver, "Stored receiver was null");

            Assert.That(storedReceiver.DirectCommunicationIds.Contains(directCommunication.DirectCommunicationId), "Receiver did not contain directCommunication");
        }
    }
}