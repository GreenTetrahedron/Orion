using Orion.Server.DataLayer;
using Orion.Server.Messages;
using Orion.Server.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Tests.Mocks.DataLayer
{
    public class MockDB : IDataLayer
    {
        private Dictionary<Guid, Message> messages;
        private Dictionary<Guid, User> users;

        public MockDB()
        {
            messages = new Dictionary<Guid, Message>();
            users = new Dictionary<Guid, User>();
        }

        public async Task<dynamic> AddMessage(Message message)
        {
            messages.Add(message.MessageId, message);
            return null;
        }

        public async Task<dynamic> AddUser(User user)
        {
            users.Add(user.UserId, user);
            return null;
        }

        public async Task<dynamic> GetAllUsers()
        {
            return users.Values;
        }

        public async Task<dynamic> GetMessage(Guid messageId)
        {
            messages.TryGetValue(messageId, out var message);

            return message;
        }
        public async Task<dynamic> GetUser(Guid userId)
        {
            users.TryGetValue(userId, out var user);

            return user;
        }
    }
}
