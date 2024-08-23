using Orion.Client.Subscriptables;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;

namespace Orion.Client.Users.Services
{
    public interface IUserService
    {
        public Task<Subscriptable<AuthenticationMessages>> AuthenticateUser(Credentials credentials);
    }
}