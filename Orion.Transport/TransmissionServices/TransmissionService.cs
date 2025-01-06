using Orion.Cryptography.EncryptionServices;
using Orion.Cryptography.KeyExchangers;
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
        private readonly IEncryptionService _encryptionService;
        private readonly IKeyExchanger _keyExchanger;

        private readonly IJsonService _jsonService;
        private readonly IConnectionService _connectionService;

        private bool _initialised;
        private byte[] _encryptionKey;

        public bool Initialised => _initialised;

        public TransmissionService(IJsonService jsonService, IEncryptionService encryptionService, IKeyExchanger keyExchanger, IConnectionService connectionService)
        {
            _encryptionService = encryptionService;
            _keyExchanger = keyExchanger;

            _jsonService = jsonService;
            _connectionService = connectionService;
        }

        public async Task<bool> InitialiseConnection()
        {
            if (_initialised)
                return true;

            try
            {
                _keyExchanger.GeneratePublicPrivateKeyPair(out var privateKey, out var publicKey);
                
                await _connectionService.SendMessage(publicKey);

                var messageBytes = await _connectionService.ReceiveMessage();

                var otherPublicKey = messageBytes.Data;

                _encryptionKey = _keyExchanger.CombineKeys(otherPublicKey, privateKey, publicKey);

                _initialised = true;
            }
            catch
            {

            }

            return _initialised;
        }

        public async Task<T> ReceiveTransmission<T>() where T : class
        {
            if (!Initialised)
                throw new Exception("Not initialised...");

            var transmissionBytes = await _connectionService.ReceiveMessage();

            byte[] iv = new byte[_encryptionService.IVSize];
            byte[] cipherText = new byte[transmissionBytes.DataByteLength - iv.Length];

            Buffer.BlockCopy(transmissionBytes.Data, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(transmissionBytes.Data, iv.Length, cipherText, 0, cipherText.Length);

            byte[] plainText = _encryptionService.Decrypt(cipherText, _encryptionKey, iv);

            string json = Encoding.UTF8.GetString(plainText, 0, plainText.Length);

            return _jsonService.DeserialiseJson<T>(json);
        }

        public async Task<int> SendTransmission(object transmission)
        {
            if (!Initialised)
                throw new Exception("Not initialised...");

            string transmissionJson = _jsonService.SerialiseObject(transmission);
            byte[] transmissionJsonBytes = Encoding.UTF8.GetBytes(transmissionJson);

            byte[] cipherText = _encryptionService.Encrypt(transmissionJsonBytes, _encryptionKey, out byte[] iv);
            byte[] finalTransmission = new byte[cipherText.Length + iv.Length];

            Buffer.BlockCopy(iv, 0, finalTransmission, 0, iv.Length);
            Buffer.BlockCopy(cipherText, 0, finalTransmission, iv.Length, cipherText.Length);

            return await _connectionService.SendMessage(finalTransmission);
        }
    }
}
