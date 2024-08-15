namespace Orion.Models.ServerTransmissions.Results
{
    public class OperationResult
    {
        public OperationMessages OperationMessage { get; set; }

        public OperationResult(OperationMessages operationMessage)
        {
            OperationMessage = operationMessage;
        }
    }
}
