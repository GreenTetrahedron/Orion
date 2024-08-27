using Orion.Models;
using Orion.Server.DataLayer;
using Orion.Server.DataLayer.Entities;
using Orion.Server.Messages;
using System.Diagnostics;

namespace Orion.Server.Tests.Mocks.DataLayer
{
    class MockDB : Database
    {
        public MockDB()
        {
            UserEntity = new MockEntity<User>();

            MessageEntity = new MockEntity<Message>();

            DirectCommunicationEntity = new MockEntity<DirectCommunication>();

            DirectMessageEntity = new MockEntity<DirectMessage>();
        }
    }
}
