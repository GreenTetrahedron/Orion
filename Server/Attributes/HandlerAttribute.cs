using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Attributes
{
    public class HandlerAttribute : Attribute
    {
        public string Topic { get; set; }

        public HandlerAttribute(string topic)
        {
            Topic = topic;
        }
    }
}
