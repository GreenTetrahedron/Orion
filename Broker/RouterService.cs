using Orion.JsonParser;
using Orion.Models.ClientTransmissions;
using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Router.Connections;
using Orion.Router.Requests;
using Orion.Router.TopicInterceptors;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Orion.Router
{
    public class RouterService
    {
        private IPEndPoint _iPEndPoint;
        private Socket _server;

        private IRequestService _requestService;
        private IConnectionService _connectionService;
        private ITopicInterceptorService _topicInterceptorService;

        private readonly IJsonService _jsonService;

        public RouterService(IPEndPoint iPEndPoint, ITopicInterceptorService topicInterceptorService, IConnectionService connectionService, IRequestService requestService, IJsonService jsonService)
        {
            _iPEndPoint = iPEndPoint;

            _topicInterceptorService = topicInterceptorService;
            _connectionService = connectionService;
            _requestService = requestService;

            _jsonService = jsonService;
        }

        public async Task Run()
        {
            Console.WriteLine("Broker running...");
            var listener = new Socket(_iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(_iPEndPoint);

            listener.Listen(100);

            Task.Run(() =>
            {
                while (true)
                {
                    var handler = listener.Accept();
                    
                    if (!handler.Connected)
                    {
                        Console.WriteLine("Connection attempt failed...");
                        continue;
                    }

                    Console.WriteLine("Connection attempt successful, new client connected...");

                    NewConnection(handler);
                }
            });

            Console.ReadLine();
        }

        public async Task NewConnection(Socket handler)
        {
            Console.WriteLine("Handling new connection...");

            string message = _jsonService.DeserialiseJson<string>(await ReceiveTransmission(handler));

            switch (message)
            {
                case "SERVER":
                    ServerConnection(handler);
                    break;
                default:
                    NewClientConnection(handler);
                    break;
            }
            Console.WriteLine("Connection handled...");
        }

        public async Task NewClientConnection(Socket handler)
        {
            if (_server == null)
            {
                Console.WriteLine("No server... Cannot connect client...");
                return;
            }

            while (true)
            {
                var clientTransmission = await ReceiveClientTransmission(handler);

                var requestId = _requestService.AddRequest(handler);

                SendServerRequest(_server, clientTransmission.Topic, clientTransmission.Data, requestId);
            }
        }

        public async Task ServerConnection(Socket serverHandler)
        {
            _server = serverHandler;
            Console.WriteLine("Server connected...");

            while (true)
            {
                ServerTransmission serverTransmission = await ReceiveServerTransmission();

                bool topicHasInterceptor = _topicInterceptorService.TryGetTopicInterceptor(serverTransmission.Response.Topic, out var topicInterceptor);

                if (topicHasInterceptor)
                {
                    topicInterceptor.Invoke(serverTransmission.Response);
                }

                var broadcastList = serverTransmission.Response.AffectedUsers;

                if (broadcastList == null || broadcastList.Length == 0)
                {
                    if (serverTransmission.Response.RequestId == null)
                        continue;

                    bool wasRequested = _requestService.TryGetRequester(serverTransmission.Response.RequestId.Value, out var requester);

                    if (wasRequested)
                    {
                        await ForwardServerResponse(serverTransmission.Response, requester);
                    }

                    continue;
                }

                bool isConnected = _connectionService.TryGetConnectionHandler(broadcastList[0], out var client);

                if (isConnected)
                {
                    await ForwardServerResponse(serverTransmission.Response, client);
                }

                if (serverTransmission.Publish == null || serverTransmission.Publish.AffectedUsers == null || serverTransmission.Publish.AffectedUsers.Length == 0)
                    continue;

                foreach(var userId in serverTransmission.Response.AffectedUsers)
                {
                    isConnected = _connectionService.TryGetConnectionHandler(userId, out client);

                    if (isConnected)
                    {
                        await ForwardServerResponse(serverTransmission.Publish, client);
                    }
                }
            }
        }

        public async Task<string> ReceiveTransmission(Socket handler)
        {
            var buffer = new byte[2048];

            int transmissionBytesCount = await handler.ReceiveAsync(buffer, SocketFlags.None);

            string transmission = Encoding.UTF8.GetString(buffer, 0, transmissionBytesCount);

            return transmission;
        }

        public async Task<ServerTransmission?> ReceiveServerTransmission()
        {
            var buffer = new byte[2048];

            int transmissionBytesCount = await _server.ReceiveAsync(buffer, SocketFlags.None);

            string transmissionJson = Encoding.UTF8.GetString(buffer, 0, transmissionBytesCount);
            ServerTransmission? serverTransmission = _jsonService.DeserialiseJson<ServerTransmission?>(transmissionJson);

            return serverTransmission;
        }

        public async Task<ClientTransmission?> ReceiveClientTransmission(Socket client)
        {
            var buffer = new byte[2048];

            int transmissionBytesCount = await client.ReceiveAsync(buffer, SocketFlags.None);

            string transmissionJson = Encoding.UTF8.GetString(buffer, 0, transmissionBytesCount);
            var clientTransmission = _jsonService.DeserialiseJson<ClientTransmission>(transmissionJson);

            Console.WriteLine($"New client transmission of topic: {clientTransmission.Topic}");

            return clientTransmission;
        }

        public async Task<int> ForwardServerResponse(ServerResponse serverResponse, Socket client)
        {
            string responseJson = _jsonService.SerialiseObject(new ClientTransmission(serverResponse.Topic, serverResponse.ServerResult));

            byte[] transmissionBytes = Encoding.UTF8.GetBytes(responseJson);

            return await client.SendAsync(transmissionBytes);
        }

        public async Task<int> SendServerRequest(ServerRequest serverRequest)
        {
            byte[] transmissionBytes = Encoding.UTF8.GetBytes(_jsonService.SerialiseObject(serverRequest));

            return await _server.SendAsync(transmissionBytes);
        }

        public async Task<int> SendServerRequest(Socket handler, string topic, object data, Guid? requestId = null)
        {
            var serverRequest = new ServerRequest(topic, data, requestId);

            byte[] transmissionBytes = Encoding.UTF8.GetBytes(_jsonService.SerialiseObject(serverRequest));

            return await handler.SendAsync(transmissionBytes);
        }
    }
}
