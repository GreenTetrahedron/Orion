using Orion.JsonParser;
using Orion.Models.ClientTransmissions;
using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.TopicHandlers;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Orion.Server
{
    public class ServerService
    {
        private readonly IJsonService _jsonService;
        private readonly ITopicHandlerService _topicHandlerService;

        private readonly IPEndPoint _routerIpEndpoint;

        private Socket _router;

        private const string IDENTIFIER = "SERVER";


        public ServerService(IPEndPoint routerIpEndpoint, IJsonService jsonService, ITopicHandlerService topicHandlerService)
        {
            _topicHandlerService = topicHandlerService;

            _router = new Socket(routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _routerIpEndpoint = routerIpEndpoint;
            _jsonService = jsonService;
        }

        public async Task Run()
        {
            Console.WriteLine("Server running...");

            await _router.ConnectAsync(_routerIpEndpoint);

            await TransmitData(IDENTIFIER);

            Task.Run(async () =>
            {
                while (true)
                {
                    var request = await ReceiveRequest();
                    Console.WriteLine($"New request of topic: {request.Topic}");

                    if (request == null)
                        continue;

                    var response = await HandleRequest(request);

                    await TransmitData(response);

                    Console.WriteLine($"Transmitted response to request of topic: {request.Topic}");
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

        private async Task<ServerTransmission?> HandleRequest(ServerRequest request)
        {
            var handler = _topicHandlerService.GetTopicHandler(request.Topic);

            if (handler == null)
                throw new ApplicationException($"No handler found for topic: {request.Topic}");

            ServerTransmission result = await handler.Invoke(request.Data);
            result.Response.RequestId = request.RequestId;
            return result;
        }

        private async Task<ServerRequest?> ReceiveRequest()
        {
            byte[] buffer = new byte[2048];

            int transmissionLength = await _router.ReceiveAsync(buffer);

            string transmissionJson = Encoding.UTF8.GetString(buffer, 0, transmissionLength);

            ServerRequest? request = _jsonService.DeserialiseJson<ServerRequest>(transmissionJson);

            return request;
        }
    }
}
