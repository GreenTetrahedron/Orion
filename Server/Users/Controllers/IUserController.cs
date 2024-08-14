using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Users.Controllers
{
    public interface IUserController
    {
        public Task<ServerResult?> AuthenticateUser(string username);
    }
}
