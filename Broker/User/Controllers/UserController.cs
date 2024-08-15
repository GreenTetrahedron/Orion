using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Router.Attributes;
using Orion.Router.Connections;
using Orion.Router.Requests;

namespace Orion.Router.User.Controllers
{
    [Controller]
    public class UserController
    {
        private readonly IRequestService _requestService;
        private readonly IConnectionService _connectionService;

        public UserController(IRequestService requestService, IConnectionService connectionService)
        {
            _requestService = requestService;
            _connectionService = connectionService;
        }

        [Interceptor("AuthenticationResult")]
        public async Task InitialiseClient(ServerResponse authenticationResponse)
        {
            if (authenticationResponse.ServerResult.OperationResult.OperationMessage != OperationMessages.VALID_CREDENTIALS)
                return;

            if (authenticationResponse.RequestId == null)
                throw new ArgumentNullException(nameof(authenticationResponse.RequestId));

            bool requestIsValid = _requestService.TryGetRequester(authenticationResponse.RequestId.Value, out var client);

            if (!requestIsValid)
                throw new Exception($"Request, {authenticationResponse.RequestId}, not found");

            if (authenticationResponse.ServerResult.AffectedUsers == null || authenticationResponse.ServerResult.AffectedUsers.Length == 0)
                throw new Exception("AffectedUsers was null");

            var userId = authenticationResponse.ServerResult.AffectedUsers[0];

            _connectionService.TryAddConnection(client, userId);
        }
    }
}
