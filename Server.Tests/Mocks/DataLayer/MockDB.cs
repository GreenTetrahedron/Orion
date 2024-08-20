using Orion.Server.DataLayer;
using Orion.Server.DataLayer.Entities;
using Orion.Server.DirectCommunications;
using Orion.Server.Messages;
using Orion.Server.Users;
using System.Diagnostics;

namespace Orion.Server.Tests.Mocks.DataLayer
{
    class MockDB : Database
    {
        public IEntity<User> UserEntity { get; set; }
        public IEntity<Message> MessageEntity { get; set; }
        public IEntity<DirectCommunication> DirectCommunicationEntity { get; set; }
        public IEntity<DirectMessage> DirectMessageEntity { get; set; }

        public MockDB()
        {
            UserEntity = new MockEntity<User>();

            MessageEntity = new MockEntity<Message>();

            DirectCommunicationEntity = new MockEntity<DirectCommunication>();

            DirectMessageEntity = new MockEntity<DirectMessage>();
        }
    }
}
