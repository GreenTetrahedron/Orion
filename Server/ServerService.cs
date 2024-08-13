using Orion.JsonParser;
using Orion.Models.RouterTransmissions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Orion.Server
{
    public class ServerService
    {
        private readonly Dictionary<string, Action<object>> _topicToHandler;

        private readonly IJsonService _jsonService;
        private readonly IPEndPoint _routerIpEndpoint;


        private Socket _router;


        public ServerService(IPEndPoint routerIpEndpoint, IJsonService jsonService)
        {
            _topicToHandler = new Dictionary<string, Action<object>>();
            _topicToHandler.Add("AuthenticateClient");

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
                while(true)
                {
                    var request = await ReceiveRequest();

                    if (request == null)
                        continue;

                    HandleRequest(request);
                }
            });

            Console.ReadLine();
        }

        private async Task HandleRequest(ServerRequest request)
        {

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
