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
        private readonly IConnectionService _connectionService;
        private readonly IJsonService _jsonService;

        private bool _initialised;

        public bool Initialised => _initialised;

        public TransmissionService(IJsonService jsonService, IConnectionService connectionService)
        {
            _connectionService = connectionService;
            _jsonService = jsonService;
        }

        public async Task<bool> InitialiseConnection()
        {
            _initialised = true;

            return true;
        }

        public async Task<T> ReceiveTransmission<T>() where T : class
        {
            if (!Initialised)
                throw new Exception("Not initialised...");

            var transmissionBytes = await _connectionService.ReceiveMessage();

            string json = Encoding.UTF8.GetString(transmissionBytes.Data, 0, transmissionBytes.DataByteLength);

            return _jsonService.DeserialiseJson<T>(json);
        }

        public async Task<int> SendTransmission(object transmission)
        {
            if (!Initialised)
                throw new Exception("Not initialised...");

            string transmissionJson = _jsonService.SerialiseObject(transmission);
            byte[] transmissionJsonBytes = Encoding.UTF8.GetBytes(transmissionJson);

            return await _connectionService.SendMessage(transmissionJsonBytes);
        }
    }
}
