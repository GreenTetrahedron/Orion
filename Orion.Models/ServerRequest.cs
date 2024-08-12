using System;
using System.Collections.Generic;

namespace Orion.Models
{
    public class ServerRequest
    {
        public Guid? RequestId { get; set; }

        public string Topic { get; set; }

        public object Data { get; set; }
    }
}
