using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
