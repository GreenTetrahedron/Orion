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
        Task<int> SendTransmission(object transmission);
        Task<int> SendTransmission(object transmission, Func<byte[], Task<int>> sendFunction);

        Task<T> ReceiveTransmission<T>() where T : class;
        Task<T> ReceiveTransmission<T>(Func<Task<MessageBytes>> receiveFunction) where T : class;
    }
}
