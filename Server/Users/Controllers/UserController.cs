using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Users.Controllers
{
    public class UserController : IUserController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ServerResult<User>?> AuthenticateUser(string username)
        {
            return await _userRepository.AuthenticateUser(username);
        }
    }
}
