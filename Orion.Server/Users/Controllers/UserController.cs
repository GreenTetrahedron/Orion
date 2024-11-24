using Orion.Logging.LoggingServices;
using Orion.Models.ServerTransmissions;
using Orion.Models.UserModels;
using Orion.Server.Attributes;
using Orion.Server.Users.Repositories;

namespace Orion.Server.Users.Controllers
{
    [Controller]
    public class UserController : IUserController
    {
        private readonly IUserRepository _userRepository;
        private readonly LoggingService _loggingService;

        public UserController(IUserRepository userRepository, LoggingService loggingService)
        {
            _userRepository = userRepository;
            _loggingService = loggingService;
        }

        [Handler("AuthenticateUser")]
        public async Task<ServerTransmission> AuthenticateUser(Credentials credentials)
        {
            _loggingService.Log($"User login attempt: {credentials.Username}", DateTime.Now);

            return await _userRepository.AuthenticateUser(credentials);
        }

        [Handler("GetUserByUsername")]
        public async Task<ServerTransmission> GetUserByUsername(string username)
        {
            _loggingService.Log($"Getting user by username: {username}", DateTime.Now);

            return await _userRepository.GetUserProfileByUsername(username);
        }
    }
}
