using Orion.Server.DataLayer.Entities;
using Orion.Server.Messages;
using Orion.Server.Users;
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

        public Database()
        {
            UserEntity = new Entity<User>();

            MessageEntity = new Entity<Message>();
        }
    }
}
