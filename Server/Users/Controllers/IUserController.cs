using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.Users.Controllers
{
    public interface IUserController
    {
        public Task<ServerResult?> AuthenticateUser(string username);
    }
}
