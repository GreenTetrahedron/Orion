using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Router.Attributes;
using Orion.Router.Clients;
using Orion.Router.Requests;

namespace Orion.Router.User.Controllers
{
    [Controller]
    public class UserController
    {
        private readonly IRequestService _requestService;
        private readonly IClientService _connectionService;

        public UserController(IRequestService requestService, IClientService connectionService)
        {
            _requestService = requestService;
            _connectionService = connectionService;
        }

        [Interceptor("AuthenticateUserResult")]
        public async Task InitialiseClient(ServerResponse authenticationResponse)
        {
            if (authenticationResponse.ServerResult.OperationInformation.OperationMessageCode != AuthenticationMessages.VALID_CREDENTIALS.ToString())
                return;

            if (authenticationResponse.RequestId == null)
                throw new ArgumentNullException(nameof(authenticationResponse.RequestId));

            bool requestIsValid = _requestService.TryGetRequester(authenticationResponse.RequestId.Value, out var client);

            if (!requestIsValid)
                throw new Exception($"Request, {authenticationResponse.RequestId}, not found");

            if (authenticationResponse.AffectedUsers == null || authenticationResponse.AffectedUsers.Length == 0)
                throw new Exception("AffectedUsers was null");

            var userId = authenticationResponse.AffectedUsers[0];

            _connectionService.TryAddConnection(client, userId);
        }
    }
}
