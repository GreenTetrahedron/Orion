using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.ServerTransmissionServices
{
    public static class ServerTransmissionService
    {
        public static ServerTransmission NewSuccessfulResponseServerTransmission<T>(string topic, T operationMessageCode, object? data = null) where T : Enum
        {
            return new ServerTransmission(new ServerResponse(topic, new ServerResult<T>(new OperationInformation<T>(Statuses.SUCCEEDED, operationMessageCode), data)));
        }

        public static ServerTransmission AddResponseOperationMessage(this ServerTransmission serverTransmission, string? operationMessage = null)
        {
            serverTransmission.Response.ServerResult.OperationInformation.OperationMessage = operationMessage;

            return serverTransmission;
        }

        public static ServerTransmission AddResponseAffectedUser(this ServerTransmission serverTransmission, Guid affectedUser)
        {
            serverTransmission.Response.AffectedUsers = [affectedUser];

            return serverTransmission;
        }

        public static ServerTransmission AddPublish(this ServerTransmission serverTransmission, string topic, object? data, Guid[]? affectedUsers)
        {
            serverTransmission.Publish = new ServerResponse(topic, new ServerResult(null, data), null, affectedUsers);

            return serverTransmission;
        }
        public static ServerTransmission AddPublish(this ServerTransmission serverTransmission, string topic, object? data, Guid affectedUser) =>
            serverTransmission.AddPublish(topic, data, [affectedUser]);
    }
}
