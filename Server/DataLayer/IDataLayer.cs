using Server.Messages;
using Server.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataLayer
{
    public interface IDataLayer
    {
        public void AddUser(User user);
        public void AddMessage(Message message);

        public dynamic GetUser(Guid userId);
        public dynamic GetMessage(Guid messageId);
    }
}
