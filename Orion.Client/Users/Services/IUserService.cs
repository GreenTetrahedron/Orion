using Orion.Client.Subscriptions;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.UserModels;

namespace Orion.Client.Users.Services
{
    public interface IUserService
    {
        public Task<Subscriptable<AuthenticationMessages>> AuthenticateUser(Credentials credentials);
    }
}