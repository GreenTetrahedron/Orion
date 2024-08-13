using NUnit.Framework;
using Orion.Server.Tests.Mocks.User.Repositories;
using Orion.Server.Tests.Mocks.Users.Repositories;
using Orion.Server.Users.Controllers;
using Orion.Server.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Tests.Users.Controllers
{
    public class UserControllerTests
    {
        private IUserController _userController;
        private MockUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new MockUserRepository();
            _userController = new UserController(_userRepository);
        }

        [Test]
        public void AuthenticateUser_CallsAuthenticateUserWithCorrectUsername()
        {

        }
    }
}
