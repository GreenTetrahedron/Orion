using Orion.Client.Subscriptions;
using Orion.Client.Subscriptions.Services;
using Orion.Client.TopicHandlers;
using Orion.JsonParser;
using Orion.Models.ClientTransmissions;
using Orion.Transport.ConnectionServices;
using Orion.Transport.TransmissionServices;
using System.Text;

namespace Orion.Client
{
    public class ClientService : IClientService
    {
        private readonly ITransmissionService _transmissionService;
        private readonly IJsonService _jsonService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ITopicHandlerService _topicHandlerService;

        private bool _running;

        public ClientService(ITransmissionService transmissionService, IJsonService jsonService, ISubscriptionService subscriptionService, ITopicHandlerService topicHandlerService)
        {
            _transmissionService = transmissionService;
            _jsonService = jsonService;
            _subscriptionService = subscriptionService;
            _topicHandlerService = topicHandlerService;

            _running = false;
        }

        public async Task RunClient()
        {
            _running = true;
            await InitialiseRouterConnection();


            while (_running)
            {
                await ReceiveData();
            }
        }

        public async Task StopClient()
        {
            _running = false;
        }

        private async Task<ClientTransmission?> ReceiveData()
        {
            var transmission = await _transmissionService.ReceiveTransmission<ClientTransmission>();

            if (transmission == null)
                return transmission;

            _subscriptionService.TryPublishDataForTopic(transmission.Topic, transmission.Data);
            var handler = _topicHandlerService.GetTopicHandler(transmission.Topic);

            handler?.Invoke(transmission.Data);

            return transmission;
        }

        public async Task<Subscriptable<T>> TransmitDataOfTopic<T>(object? data, string topic) where T : Enum
        {
            var transmission = new ClientTransmission(topic, data);

            var subscriptable = _subscriptionService.GetOrCreateSubscriptableForTopic<T>(topic + "Result");

            _transmissionService.SendTransmission(transmission);

            return subscriptable;
        }

        public async Task InitialiseRouterConnection()
        {
            await _transmissionService.SendTransmission("CLIENT");
        }
    }
}
