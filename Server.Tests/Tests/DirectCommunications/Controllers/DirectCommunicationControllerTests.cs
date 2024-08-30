using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.DirectCommunicationModels;
using Orion.Server.DirectCommuncations.Controllers;
using Orion.Server.Tests.Mocks.DataLayer;
using Orion.Server.Tests.Mocks.Users.Repositories;
using Orion.Server.Users;
using Orion.Server.Users.Controllers;

namespace Orion.Server.Tests.DirectCommunications.Controllers
{
    public class DirectCommunicationControllerTests
    {
        private IDirectCommunicationController _directCommunicationController;
        private MockDirectCommunicationRepository _mockDirectCommunicationRepository;

        [SetUp]
        public void Setup()
        {
            _mockDirectCommunicationRepository = new MockDirectCommunicationRepository();
            _directCommunicationController = new DirectCommunicationController(_mockDirectCommunicationRepository);
        }

        [Test]
        public async Task AddDirectCommunication_CallsNewDirectCommunicationWithCorrectArguments()
        {
            var sender = new User() { UserId = Guid.NewGuid() };
            var receiver = new User() { UserId = Guid.NewGuid(), Username = "User" };

            var newDirectCommunication = new NewDirectCommunication() { SenderId = sender.UserId, ReceiverId = receiver.UserId };

            await _directCommunicationController.AddDirectCommunication(newDirectCommunication);

            Assert.That(_mockDirectCommunicationRepository.callStack.GetNameOfLastMethodCalled() == nameof(_mockDirectCommunicationRepository.AddDirectCommunication), "Wrong method called");
            Assert.That(_mockDirectCommunicationRepository.callStack.GetLastMethodCallArguments().SequenceEqual([newDirectCommunication]), "Wrong arguments passed...");
        }
    }
}
