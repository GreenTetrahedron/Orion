using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.UserModels;
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
        public async Task<ServerTransmission> AuthenticateUser(Credentials credentials)
        {
            return await _userRepository.AuthenticateUser(credentials);
        }

        [Handler("GetUserByUsername")]
        public async Task<ServerTransmission> GetUserByUsername(string username)
        {
            return await _userRepository.GetUserProfileByUsername(username);
        }
    }
}
