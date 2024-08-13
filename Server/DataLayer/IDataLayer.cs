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
        public Task<dynamic> GetAllUsers();

        public Task<dynamic> AddUser(User user);
        public Task<dynamic> AddMessage(Message message);

        public Task<dynamic> GetUser(Guid userId);
        public Task<dynamic> GetMessage(Guid messageId);
    }
}
