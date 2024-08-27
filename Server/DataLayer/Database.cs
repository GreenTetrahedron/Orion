using Orion.Models;
using Orion.Server.DataLayer.Entities;
using Orion.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DataLayer
{
    public class Database
    {
        public IEntity<User> UserEntity { get; set; }
        public IEntity<Message> MessageEntity { get; set; }
        public IEntity<DirectCommunication> DirectCommunicationEntity { get; set; }
        public IEntity<DirectMessage> DirectMessageEntity { get; set; }

        public Database()
        {
            UserEntity = new Entity<User>();

            var user1 = new User() { UserId = Guid.NewGuid(), Username = "User1" };
            var user2 = new User() { UserId = Guid.NewGuid(), Username = "User2" };

            UserEntity.AddRecord(user1.UserId, user1);
            UserEntity.AddRecord(user2.UserId, user2);

            MessageEntity = new Entity<Message>();

            DirectCommunicationEntity = new Entity<DirectCommunication>();

            DirectMessageEntity = new Entity<DirectMessage>();
        }
    }
}
