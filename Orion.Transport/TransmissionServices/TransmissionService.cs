using Orion.JsonParser;
using Orion.Transport.ConnectionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Transport.TransmissionServices
{
    public class TransmissionService : ITransmissionService
    {
        private readonly IConnectionService? _connectionService;
        private readonly IJsonService _jsonService;

        public TransmissionService(IJsonService jsonService, IConnectionService? connectionService = null)
        {
            _connectionService = connectionService;
            _jsonService = jsonService;
        }

        public async Task<T> ReceiveTransmission<T>() where T : class
        {
            if (_connectionService == null)
                throw new Exception("connectionService was null...");

            return await ReceiveTransmission<T>(_connectionService.ReceiveMessage);
        }

        public async Task<T> ReceiveTransmission<T>(Func<Task<MessageBytes>> receiveFunction) where T : class
        {
            var messageBytes = await receiveFunction.Invoke();

            string json = Encoding.UTF8.GetString(messageBytes.Data, 0, messageBytes.DataByteLength);

            return _jsonService.DeserialiseJson<T>(json);
        }

        public async Task<int> SendTransmission(object transmission)
        {
            if (_connectionService == null)
                throw new Exception("connectionService was null...");

            return await SendTransmission(transmission, _connectionService.SendMessage);
        }

        public async Task<int> SendTransmission(object transmission, Func<byte[], Task<int>> sendFunction)
        {
            string transmissionJson = _jsonService.SerialiseObject(transmission);
            byte[] transmissionJsonBytes = Encoding.UTF8.GetBytes(transmissionJson);

            int sentBytes = await sendFunction.Invoke(transmissionJsonBytes);

            byte[] currentTransmissionJsonBytes = transmissionJsonBytes;

            while (sentBytes < transmissionJsonBytes.Length)
            {
                Array.Copy(currentTransmissionJsonBytes, sentBytes - 1, currentTransmissionJsonBytes, 0, currentTransmissionJsonBytes.Length - sentBytes);

                sentBytes += await sendFunction.Invoke(currentTransmissionJsonBytes);
            }

            return sentBytes;
        }
    }
}
