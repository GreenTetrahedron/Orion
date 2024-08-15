using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Orion.JsonParser;
using Orion.Client.TopicHandlers;
using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ClientTransmissions;

namespace Orion.Client
{
    public class ClientService
    {
        private readonly IJsonService _jsonService;
        private readonly ITopicHandlerService _topicHandlerService;

        private readonly IPEndPoint _routerIpEndpoint;

        private Socket _router;

        private const string IDENTIFIER = "CLIENT";


        public ClientService(IPEndPoint routerIpEndpoint, IJsonService jsonService, ITopicHandlerService topicHandlerService)
        {
            _topicHandlerService = topicHandlerService;

            _router = new Socket(routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _routerIpEndpoint = routerIpEndpoint;
            _jsonService = jsonService;
        }

        public async Task Run()
        {
            _router.ConnectAsync(_routerIpEndpoint);

            await TransmitData(IDENTIFIER);

            Console.WriteLine("Enter your username: ");
            string username = Console.ReadLine();

            await TransmitData(new ClientTransmission("AuthenticateUser", username));

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

            Console.ReadLine();
        }

        private async Task<int> TransmitData(object data)
        {
            string transmissionJson = _jsonService.SerialiseObject(data);

            var transmissionBytes = Encoding.UTF8.GetBytes(transmissionJson);

            return await _router.SendAsync(transmissionBytes);
        }

        private async Task HandleTransmission(ClientTransmission transmission)
        {
            var handler = _topicHandlerService.GetTopicHandler(transmission.Topic);

            if (handler == null)
                throw new ApplicationException($"No handler found for topic: {transmission.Topic}");

            handler.Invoke(transmission.Data);
        }

        private async Task<ClientTransmission?> ReceiveTransmission()
        {
            byte[] buffer = new byte[1024];

            int transmissionLength = await _router.ReceiveAsync(buffer);

            string transmissionJson = Encoding.UTF8.GetString(buffer, 0, transmissionLength);

            var transmission = _jsonService.DeserialiseJson<ClientTransmission>(transmissionJson);

            return transmission;
        }

    }
}
