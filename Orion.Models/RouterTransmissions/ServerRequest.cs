using System;
using System.Collections.Generic;

namespace Orion.Models.RouterTransmissions
{
    public class ServerRequest
    {
        public Guid? RequestId { get; set; }

        public string Topic { get; set; }

        public object Data { get; set; }
    }
}
