using Orion.Client.Connections;
using Orion.Client.Subscriptions;
using Orion.Client.Subscriptions.Services;
using Orion.JsonParser;
using Orion.Models.ClientTransmissions;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orion.Client.Transmissions
{
    public class TransmissionService : ITransmissionService
    {
        private readonly IConnectionService _connectionService;
        private readonly IJsonService _jsonService;
        private readonly ISubscriptionService _subscriptionService;

        public TransmissionService(IConnectionService connectionService, IJsonService jsonService, ISubscriptionService subscriptionService)
        {
            _connectionService = connectionService;
            _jsonService = jsonService;
            _subscriptionService = subscriptionService;
        }

        public async Task<ClientTransmission?> ReceiveData()
        {
            var messageBytes = await _connectionService.ReceiveMessage();

            string transmissionJson = Encoding.UTF8.GetString(messageBytes.Data, 0, messageBytes.ReceivedBytes);
            var transmission = _jsonService.DeserialiseJson<ClientTransmission?>(transmissionJson);

            if (transmission == null)
                return transmission;

            _subscriptionService.TryPublishDataForTopic(transmission.Topic, transmission.Data);

            return transmission;
        }

        public async Task<Subscriptable<T>> TransmitDataOfTopic<T>(object? data, string topic) where T : Enum
        {
            var transmission = new ClientTransmission(topic, data);
            var transmissionJson = _jsonService.SerialiseObject(transmission);
            var transmissionBytes = Encoding.UTF8.GetBytes(transmissionJson);

            var subscriptable = _subscriptionService.GetOrCreateSubscriptableForTopic<T>(topic + "Result");

            _connectionService.SendMessage(transmissionBytes);

            return subscriptable;
        }

        public async Task InitialiseRouterConnection()
        {
            var initialiseMessage = _jsonService.SerialiseObject("CLIENT");
            var initialiseMessageBytes = Encoding.UTF8.GetBytes(initialiseMessage);

            await _connectionService.SendMessage(initialiseMessageBytes);
        }
    }
}
