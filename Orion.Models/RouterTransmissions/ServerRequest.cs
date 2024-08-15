using System;
using System.Collections.Generic;

namespace Orion.Models.RouterTransmissions
{
    public class ServerRequest
    {
        public string Topic { get; set; }

        public object Data { get; set; }

        public Guid? RequestId { get; set; }

        public ServerRequest(string topic, object data, Guid? requestId = null)
        {
            Topic = topic;
            Data = data;
            RequestId = requestId;
        }
    }
}
