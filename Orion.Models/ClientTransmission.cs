using System;
using System.Security.AccessControl;

namespace Orion.Models
{
    public class ClientTransmission
    {
        public string Topic { get; set; }

        public object Data { get; set; }
    }
}
