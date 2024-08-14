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
        public static ServerResult NewServerResult(OperationMessages operationMessage, object? data = null)
        {
            return new ServerResult(new OperationResult(operationMessage), data);
        }
    }
}
