using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.ServerResults
{
    public static class ServerResultService
    {
        public static ServerResult<T> NewServerResult<T>(Statuses status, T operationMessageCode, string? operationMessage = null, Guid[]? affectedUsers = null, object? data = null) where T : Enum
        {
            return new ServerResult<T>(new OperationInformation<T>(status, operationMessageCode, operationMessage), data, affectedUsers);
        }

        public static ServerResult<T> NewServerResult<T>(Statuses status, T operationMessageCode, Guid affectedUser, string? operationMessage = null, object? data = null) where T : Enum =>
            NewServerResult(status, operationMessageCode, operationMessage, [ affectedUser ], data);
        

        public static ServerResult NewServerResult(Statuses status, Enum operationMessageCode, string? operationMessage = null, Guid[]? affectedUsers = null, object? data = null)
        {
            return new ServerResult(new OperationInformation(status, operationMessageCode.ToString(), operationMessage), data, affectedUsers);
        }

        public static ServerResult NewServerResult(Statuses status, Enum operationMessageCode, Guid affectedUser, string? operationMessage = null, object? data = null) =>
            NewServerResult(status, operationMessageCode, operationMessage, [ affectedUser ], data);

        public static ServerResult<T> NewSuccessfulServerResult<T>(T operationMessageCode, string? operationMessage = null, Guid[]? affectedUsers = null, object? data = null) where T : Enum =>
            NewServerResult(Statuses.SUCCEEDED, operationMessageCode, operationMessage, affectedUsers, data);
        public static ServerResult<T> NewSuccessfulServerResult<T>(T operationMessageCode, Guid affectedUser, string? operationMessage = null, object? data = null) where T : Enum =>
            NewServerResult(Statuses.SUCCEEDED, operationMessageCode, affectedUser, operationMessage, data);

        public static ServerResult NewSuccessfulServerResult(Enum operationMessageCode, string? operationMessage = null, Guid[]? affectedUsers = null, object? data = null) =>
            NewServerResult(Statuses.SUCCEEDED, operationMessageCode, operationMessage, affectedUsers, data);
        public static ServerResult NewSuccessfulServerResult(Enum operationMessageCode, Guid affectedUser, string? operationMessage = null, object? data = null)=>
            NewServerResult(Statuses.SUCCEEDED, operationMessageCode, affectedUser, operationMessage, data);

    }
}
