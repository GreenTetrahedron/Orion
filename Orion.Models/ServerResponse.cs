using System;
using System.Collections.Generic;

namespace Orion.Models
{
    public class ServerResponse
    {
        public Guid? RequestId { get; set; }

        public string Topic { get; set; }

        public object Data { get; set; }

        public List<Guid>? BroadcastList { get; set; }
    }
}
