using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Orion.Models.ServerTransmissions.Results
{
    public class OperationInformation<T> where T : Enum
    {
        public Statuses Status { get; set; }

        public T OperationMessageCode { get; set; }

        public string? OperationMessage { get; set; }

        public OperationInformation(Statuses status, T operationMessageCode, string? operationMessage = null)
        {
            Status = status;
            OperationMessageCode = operationMessageCode;
            OperationMessage = operationMessage;
        }

        public static implicit operator OperationInformation<T>(OperationInformation value)
        {
            object result;

            if (!Enum.TryParse(typeof(T), value.OperationMessageCode, out result))
                throw new Exception($"{value.OperationMessageCode} is not a value represented by enumeration {nameof(T)}");

            return new OperationInformation<T>(value.Status, (T)result, value.OperationMessage);
        }

        public static implicit operator OperationInformation(OperationInformation<T> value) =>
            new OperationInformation(value.Status, value.OperationMessageCode.ToString(), value.OperationMessage);
    }

    public class OperationInformation
    {
        public Statuses Status { get; set; }

        public string OperationMessageCode { get; set; }

        public string? OperationMessage { get; set; }

        public OperationInformation(Statuses status, string operationMessageCode, string? operationMessage = null)
        {
            Status = status;
            OperationMessageCode = operationMessageCode;
            OperationMessage = operationMessage;
        }
    }
}
