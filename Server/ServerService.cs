using Orion.JsonParser;
using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.TopicHandlers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Orion.Server
{
    public class ServerService
    {
        private readonly IJsonService _jsonService;
        private readonly ITopicHandlerService _topicHandlerService;

        private readonly IPEndPoint _routerIpEndpoint;

        private Socket _router;


        public ServerService(IPEndPoint routerIpEndpoint, IJsonService jsonService, ITopicHandlerService topicHandlerService)
        {
            _topicHandlerService = topicHandlerService;

            _router = new Socket(routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _routerIpEndpoint = routerIpEndpoint;
            _jsonService = jsonService;
        }

        public void Run()
        {
            Console.WriteLine("Server running...");

            _router.ConnectAsync(_routerIpEndpoint);

            Task.Run(() =>
            {
                while (true)
                {
                    var request = ReceiveRequest();
                    request.RunSynchronously();

                    if (request.Result == null)
                        continue;

                    HandleRequest(request.Result);
                }
            });

            Console.ReadLine();
        }

        private async Task<ServerResponse?> HandleRequest(ServerRequest request)
        {
            var handler = _topicHandlerService.GetTopicHandler(request.Topic);

            if (handler == null)
                throw new ApplicationException($"No handler found for topic: {request.Topic}");

            ServerResult result = await handler.Invoke(request.Data);

            return new ServerResponse(request.Topic, result, request.RequestId);
        }

        private async Task<ServerRequest?> ReceiveRequest()
        {
            byte[] buffer = new byte[1024];

            int transmissionLength = await _router.ReceiveAsync(buffer);

            string transmissionJson = Encoding.UTF8.GetString(buffer, 0, transmissionLength);

            ServerRequest? request = _jsonService.DeserialiseJson<ServerRequest>(transmissionJson);

            return request;
        }
    }
}
