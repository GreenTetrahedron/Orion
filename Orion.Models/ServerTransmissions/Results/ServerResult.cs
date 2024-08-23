namespace Orion.Models.ServerTransmissions.Results
{
    public class ServerResult<T> where T : Enum
    {
        public OperationInformation<T> OperationInformation { get; set; }

        public object? Data { get; set; }

        public Guid[]? AffectedUsers { get; set; }

        public ServerResult(OperationInformation<T> operationInformation, object? data = null, Guid[]? affectedUsers = null)
        {
            OperationInformation = operationInformation;
            Data = data;
            AffectedUsers = affectedUsers;
        }

        public static implicit operator ServerResult(ServerResult<T> value) =>
            new ServerResult(value.OperationInformation, value.Data, value.AffectedUsers);
    }

    public class ServerResult
    {
        public OperationInformation OperationInformation { get; set; }

        public object? Data { get; set; }

        public Guid[]? AffectedUsers { get; set; }

        public ServerResult(OperationInformation operationInformation, object? data = null, Guid[]? affectedUsers = null)
        {
            OperationInformation = operationInformation;
            Data = data;
            AffectedUsers = affectedUsers;
        }
    }
}
