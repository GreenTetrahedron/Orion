using Orion.Client.Subscriptions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;

namespace Orion.Client.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IClientService _clientService;

        public UserService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Subscriptable<AuthenticationMessages>> AuthenticateUser(Credentials credentials)
        {
            return await _clientService.TransmitDataOfTopic<AuthenticationMessages>(credentials, "AuthenticateUser");
        }
        public async Task<Subscriptable<GetUserMessages>> GetUserByUsername(string username)
        {
            return await _clientService.TransmitDataOfTopic<GetUserMessages>(username, "GetUserByUsername");
        }
    }
}