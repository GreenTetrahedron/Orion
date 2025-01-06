using Orion.Transport.ConnectionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Transport.TransmissionServices
{
    public interface ITransmissionService
    {
        public bool Initialised { get; }

        Task<bool> InitialiseConnection();

        Task<int> SendTransmission(object transmission);

        Task<T> ReceiveTransmission<T>() where T : class;
    }
}
