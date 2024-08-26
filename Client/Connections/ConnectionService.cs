using Orion.Client.Subscriptions;
using Orion.Client.Subscriptions.Services;
using Orion.Client.TopicHandlers;
using Orion.JsonParser;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Orion.Client.Connections
{
    public class ConnectionService : IConnectionService
    {
        private readonly IJsonService _jsonService;
        private readonly ISubscriptionService _subscriptionService;

        private readonly IPEndPoint _routerIpEndpoint;

        private Socket _router;

        private const string IDENTIFIER = "CLIENT";


        public ConnectionService(IPEndPoint routerIpEndpoint, IJsonService jsonService, ISubscriptionService subscriptionService)
        {
            _router = new Socket(routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _routerIpEndpoint = routerIpEndpoint;
            _jsonService = jsonService;
            _subscriptionService = subscriptionService;
        }

        public async Task Run()
        {
            _router.ConnectAsync(_routerIpEndpoint);

            await TransmitData(IDENTIFIER);

            Task.Run(async () =>
            {
                while (true)
                {
                    var request = await ReceiveTransmission();

                    Console.WriteLine($"New transmission of topic: {request.Topic}");

                    if (request == null)
                        continue;

                    HandleTransmission(request);
                }
            });
        }

        public async Task<int> TransmitData(object data)
        {
            string transmissionJson = _jsonService.SerialiseObject(data);

            var transmissionBytes = Encoding.UTF8.GetBytes(transmissionJson);

            return await _router.SendAsync(transmissionBytes);
        }

        private async Task HandleTransmission(ClientTransmission transmission)
        {
            var topicHasSubscription = _subscriptionService.TryPublishDataForTopic(transmission.Topic, transmission.Data);

            if (!topicHasSubscription)
                throw new ApplicationException($"No handler found for topic: {transmission.Topic}");
        }

        private async Task<ClientTransmission?> ReceiveTransmission()
        {
            byte[] buffer = new byte[1024];

            int transmissionLength = await _router.ReceiveAsync(buffer);

            string transmissionJson = Encoding.UTF8.GetString(buffer, 0, transmissionLength);

            var transmission = _jsonService.DeserialiseJson<ClientTransmission>(transmissionJson);

            return transmission;
        }

        public async Task<bool> SendMessage(byte[] data)
        {
            int sentBytes = await _router.SendAsync(data);

            return sentBytes == data.Length;
        }
    }
}
