using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.ServerResults
{
    public static class ServerResultService
    {
        public static ServerResult<T> NewServerResult<T>(OperationMessages operationMessage, T? data = null) where T : class
        {
            return new ServerResult<T>(new OperationResult(operationMessage), data);
        }
    }
}
