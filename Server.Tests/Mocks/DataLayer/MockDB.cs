using Orion.Server.DataLayer;
using Orion.Server.DataLayer.Entities;
using Orion.Server.Messages;
using Orion.Server.Users;
using System.Diagnostics;

namespace Orion.Server.Tests.Mocks.DataLayer
{
    class MockDB : Database
    {
        public IEntity<User> UserEntity { get; set; }
        public IEntity<Message> MessageEntity { get; set; }

        public MockDB()
        {
            UserEntity = new MockEntity<User>();

            MessageEntity = new MockEntity<Message>();
        }
    }
}
