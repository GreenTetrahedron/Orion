using Orion.Server.Messages;
using Orion.Server.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DataLayer
{
    public class DataLayer : IDataLayer
    {
        private Dictionary<Guid, Message> messages;
        private Dictionary<Guid, User> users;

        public DataLayer()
        {
            messages = new Dictionary<Guid, Message>();
            users = new Dictionary<Guid, User>();
        }

        public async Task<bool?> AddMessage(Message message)
        {
            messages.Add(message.MessageId, message);
            return true;
        }

        public async Task<bool?> AddUser(User user)
        {
            users.Add(user.UserId, user);
            return true;
        }

        public async Task<IList<User>?> GetAllUsers()
        {
            return users.Values.ToList();
        }

        public async Task<Message?> GetMessage(Guid messageId)
        {
            messages.TryGetValue(messageId, out var message);

            return message;
        }
        public async Task<User?> GetUser(Guid userId)
        {
            users.TryGetValue(userId, out var user);

            return user;
        }
    }
}
