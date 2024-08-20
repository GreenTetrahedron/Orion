using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.ServerResults
{
    public static class ServerResultService
    {
        public static ServerResult NewServerResult(OperationMessages operationMessage, Guid[]? affectedUsers = null, object? data = null)
        {
            return new ServerResult(new OperationResult(operationMessage), data, affectedUsers);
        }
        public static ServerResult NewServerResult(OperationMessages operationMessage, Guid affectedUser, object? data = null)
        {
            return new ServerResult(new OperationResult(operationMessage), data, [ affectedUser ]);
        }
    }
}
