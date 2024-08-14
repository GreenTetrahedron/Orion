using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;

namespace Orion.Models.ServerTransmissions
{
    public class ServerResponse
    {
        public Guid? RequestId { get; set; }

        public ServerResult? ServerResult { get; set; }

        public string Topic { get; set; }

        public ServerResponse(string topic, ServerResult? serverResult = null, Guid? requestId = null)
        {
            Topic = topic;
            ServerResult = serverResult;
            RequestId = requestId;
        }
    }
}
