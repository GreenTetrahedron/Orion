using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.ServerResults
{
    public static class ServerResultService
    {
        public static ServerResult NewServerResult(OperationMessages operationMessage, object? data = null, Guid[]? affectedUsers = null)
        {
            return new ServerResult(new OperationResult(operationMessage), data, affectedUsers);
        }
        public static ServerResult NewServerResult(OperationMessages operationMessage, Guid affectedUser, object? data = null)
        {
            return new ServerResult(new OperationResult(operationMessage), data, new Guid[1] { affectedUser });
        }
    }
}
