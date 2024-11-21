using Orion.Client.Subscriptions;
using Orion.Client.Subscriptions.Services;
using Orion.Client.TopicHandlers;
using Orion.JsonParser;
using Orion.Models.ClientTransmissions;
using Orion.Transport.ConnectionServices;
using System.Text;

namespace Orion.Client.Transmissions
{
    public class TransmissionService : ITransmissionService
    {
        private readonly IConnectionService _connectionService;
        private readonly IJsonService _jsonService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ITopicHandlerService _topicHandlerService;

        public TransmissionService(IConnectionService connectionService, IJsonService jsonService, ISubscriptionService subscriptionService, ITopicHandlerService topicHandlerService)
        {
            _connectionService = connectionService;
            _jsonService = jsonService;
            _subscriptionService = subscriptionService;
            _topicHandlerService = topicHandlerService;
        }

        public async Task<ClientTransmission?> ReceiveData()
        {
            var messageBytes = await _connectionService.ReceiveMessage();

            string transmissionJson = Encoding.UTF8.GetString(messageBytes.Data, 0, messageBytes.DataByteLength);
            var transmission = _jsonService.DeserialiseJson<ClientTransmission?>(transmissionJson);

            if (transmission == null)
                return transmission;

            _subscriptionService.TryPublishDataForTopic(transmission.Topic, transmission.Data);
            var handler = _topicHandlerService.GetTopicHandler(transmission.Topic);

            if (handler != null)
                handler.Invoke(transmission.Data);

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
