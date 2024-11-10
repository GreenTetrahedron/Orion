namespace Orion.Models.ServerTransmissions.Results
{
    public class ServerResult<T> where T : Enum
    {
        public OperationInformation<T> OperationInformation { get; set; }

        public object? Data { get; set; }

        public ServerResult(OperationInformation<T> operationInformation, object? data = null)
        {
            OperationInformation = operationInformation;
            Data = data;
        }

        public static implicit operator ServerResult(ServerResult<T> value) =>
            new ServerResult(value.OperationInformation, value.Data);

        public static implicit operator ServerResult<T>(ServerResult value) =>
            new ServerResult<T>(value.OperationInformation, value.Data);
    }

    public class ServerResult
    {
        public OperationInformation? OperationInformation { get; set; }

        public object? Data { get; set; }

        public ServerResult(OperationInformation? operationInformation = null, object? data = null)
        {
            OperationInformation = operationInformation;
            Data = data;
        }
    }
}
