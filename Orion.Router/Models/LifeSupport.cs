using Orion.Cryptography.EncryptionServices;
using Orion.Cryptography.KeyExchangers;
using Orion.JsonParser;
using Orion.Transport.ConnectionServices;
using Orion.Transport.TransmissionServices;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Router.Models
{
    public class LifeSupport
    {
        private Guid _userId;

        private readonly ITransmissionService _transmissionService;

        private bool _isLoggedIn;

        public bool IsLoggedIn => _isLoggedIn;

        public bool Initialised => _transmissionService.Initialised;

        public event Action<Guid> OnLogIn = delegate { };

        public Guid UserId => _userId;

        private LifeSupport()
        {
            _isLoggedIn = false;
        }

        public LifeSupport(Socket socket, IJsonService jsonService, IEncryptionService encryptionService, IKeyExchanger keyExchanger, int bufferLength = 16192) : this()
        {
            _transmissionService = new TransmissionService(jsonService, encryptionService, keyExchanger, new ConnectionService(socket, bufferLength));
        }

        public LifeSupport(ITransmissionService transmissionService) : this()
        {
            _transmissionService = transmissionService;
        }

        public void LogIn(Guid userId)
        {
            if (_isLoggedIn)
                throw new InvalidOperationException("Already logged in...");

            _isLoggedIn = true;
            _userId = userId;
            OnLogIn.Invoke(_userId);
        }

        public async Task<bool> InitialiseConnection()
        {
            return await _transmissionService.InitialiseConnection();
        }

        public async Task<T> ReceiveTransmission<T>() where T : class
        {
            return await _transmissionService.ReceiveTransmission<T>();
        }

        public async Task<int> SendTransmission(object data)
        {
            return await _transmissionService.SendTransmission(data);
        }
    }
}
