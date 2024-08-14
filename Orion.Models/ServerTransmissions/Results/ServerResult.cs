using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.ServerTransmissions.Results
{
    public class ServerResult
    {
        public OperationResult OperationResult { get; set; }

        public object? Data { get; set; }

        public ServerResult(OperationResult operationResult, object? data)
        {
            OperationResult = operationResult;
            Data = data;
        }
    }
}
