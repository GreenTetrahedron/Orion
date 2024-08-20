using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.Users.Controllers
{
    public interface IUserController
    {
        public Task<ServerResult?> AuthenticateUser(string username);

        public Task<ServerResult?> AddDirectCommunication(Guid senderId, Guid recipientId);
    }
}
