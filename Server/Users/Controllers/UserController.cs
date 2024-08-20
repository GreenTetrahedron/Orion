using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Attributes;
using Orion.Server.Users.Repositories;

namespace Orion.Server.Users.Controllers
{
    [Controller]
    public class UserController : IUserController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Handler("AuthenticateUser")]
        public async Task<ServerResult?> AuthenticateUser(string username)
        {
            return await _userRepository.AuthenticateUser(username);
        }

        [Handler("NewDirectMessage")]
        public async Task<ServerResult?> AddDirectCommunication(Guid senderId, Guid recipientId)
        {
            return await _userRepository.AddDirectCommunication(senderId, recipientId);
        }
    }
}
