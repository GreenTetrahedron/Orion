using Orion.Server.Messages;
using Orion.Server.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DataLayer
{
    public interface IDataLayer
    {
        public Task<IList<User>?> GetAllUsers();

        public Task<bool?> AddUser(User user);
        public Task<bool?> AddMessage(Message message);

        public Task<User?> GetUser(Guid userId);
        public Task<Message?> GetMessage(Guid messageId);
    }
}
