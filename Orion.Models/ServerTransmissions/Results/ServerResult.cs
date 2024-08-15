namespace Orion.Models.ServerTransmissions.Results
{
    public class ServerResult
    {
        public OperationResult OperationResult { get; set; }

        public object? Data { get; set; }

        public Guid[]? AffectedUsers { get; set; }

        public ServerResult(OperationResult operationResult, object? data = null, Guid[]? affectedUsers = null)
        {
            OperationResult = operationResult;
            Data = data;
            AffectedUsers = affectedUsers;
        }
    }
}
