using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Router.Attributes
{
    public class InterceptorAttribute : Attribute
    {
        public string Topic { get; set; }

        public InterceptorAttribute(string topic)
        {
            Topic = topic;
        }
    }
}
