using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.ServerTransmissions.Results
{
    public class ServerResult<T> where T : class
    {
        public OperationResult OperationResult { get; set; }

        public T? Data { get; set; }

        public ServerResult(OperationResult operationResult, T? data)
        {
            OperationResult = operationResult;
            Data = data;
        }
    }
}
