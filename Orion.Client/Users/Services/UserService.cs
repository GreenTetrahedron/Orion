using Orion.Client.Subscriptions;
using Orion.Client.Transmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;

namespace Orion.Client.Users.Services
{
    public class UserService : IUserService
    {
        private readonly ITransmissionService _transmissionService;

        public UserService(ITransmissionService transmissionService)
        {
            _transmissionService = transmissionService;
        }

        public async Task<Subscriptable<AuthenticationMessages>> AuthenticateUser(Credentials credentials)
        {
            return await _transmissionService.TransmitDataOfTopic<AuthenticationMessages>(credentials, "AuthenticateUser");
        }
        public async Task<Subscriptable<GetUserMessages>> GetUserByUsername(string username)
        {
            return await _transmissionService.TransmitDataOfTopic<GetUserMessages>(username, "GetUserByUsername");
        }
    }
}