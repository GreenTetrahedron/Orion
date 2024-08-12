using System;
using System.Security.AccessControl;

namespace Broker
{
    public class ClientTransmission
    {
        public string Topic { get; set; }

        public object Data { get; set; }
    }
}
