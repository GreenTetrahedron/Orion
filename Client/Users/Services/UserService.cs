using Orion.Client.Subscriptables;
using Orion.Client.Transmissions;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;

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
    }
}