using Orion.JsonParser;
using Orion.Models.ClientTransmissions;
using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Router.Clients;
using Orion.Router.Requests;
using Orion.Router.TopicInterceptors;
using Orion.Transport.ConnectionServices;
using Orion.Transport.TransmissionServices;
using System.ComponentModel.DataAnnotations;
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
        private IClientService _clientService;
        private ITopicInterceptorService _topicInterceptorService;

        private readonly ConnectionService _connectionService;
        private readonly ITransmissionService _transmissionService;

        public RouterService(IPEndPoint iPEndPoint, ITopicInterceptorService topicInterceptorService, IClientService clientService, IRequestService requestService, ITransmissionService transmissionService)
        {
            _iPEndPoint = iPEndPoint;

            _topicInterceptorService = topicInterceptorService;
            _clientService = clientService;
            _requestService = requestService;

            _connectionService = new();
            _transmissionService = transmissionService;
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

            string message = await _transmissionService.ReceiveTransmission<string>(async () => await _connectionService.ReceiveMessage(handler));

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

            ClientTransmission? clientTransmission;

            while (handler.Connected)
            {
                clientTransmission = await ReceiveClientTransmission(handler);

                if (clientTransmission == null)
                    break;

                var requestId = _requestService.AddRequest(handler);

                SendServerRequest(_server, clientTransmission.Topic, clientTransmission.Data, handler, requestId);
            }

            Console.WriteLine("Client disconnecting...");

            if (_clientService.TerminateConnection(handler))
                Console.WriteLine("Client disconnected");
            else
                Console.WriteLine("No handler found for client....");
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

                if (serverTransmission.Response != null)
                    await ForwardServerTransmission(serverTransmission.Response);

                if (serverTransmission.Publish != null)
                    await ForwardServerTransmission(serverTransmission.Publish);
            }
        }

        public async Task<ServerTransmission?> ReceiveServerTransmission()
        {
            ServerTransmission? serverTransmission = await _transmissionService.ReceiveTransmission<ServerTransmission>(async () => await _connectionService.ReceiveMessage(_server));

            return serverTransmission;
        }

        public async Task<ClientTransmission?> ReceiveClientTransmission(Socket client)
        {
            var clientTransmission = await _transmissionService.ReceiveTransmission<ClientTransmission>(async () => await _connectionService.ReceiveMessage(client));

            if (clientTransmission == null)
                return clientTransmission;

            Console.WriteLine($"New client transmission of topic: {clientTransmission.Topic}");

            return clientTransmission;
        }

        public async Task<int> ForwardServerTransmission(ServerResponse serverResponse)
        {
            int result = 0;
            Socket? client;

            if (serverResponse.RequestId != null && _requestService.TryGetRequester(serverResponse.RequestId.Value, out client) && client != null)
            {
                result &= await ForwardServerResponse(serverResponse, client);
                _requestService.RemoveRequest(serverResponse.RequestId.Value);
            }

            if (serverResponse.AffectedUsers == null)
                return result;


            foreach(var user in serverResponse.AffectedUsers)
            {
                if (_clientService.TryGetConnectionHandler(user, out client) && client != null)
                {
                    result &= await ForwardServerResponse(serverResponse, client);
                }
            }

            return result;
        }

        public async Task<int> ForwardServerResponse(ServerResponse serverResponse, Socket client)
        {
            var transmission = new ClientTransmission(serverResponse.Topic, serverResponse.ServerResult);

            Console.WriteLine($"Transmitting new response of topic: {serverResponse.Topic}");

            return await _transmissionService.SendTransmission(transmission, async (data) => await _connectionService.SendMessage(data, client));
        }

        public async Task<int> SendServerRequest(ServerRequest serverRequest)
        {
            return await _transmissionService.SendTransmission(serverRequest, async (data) => await _connectionService.SendMessage(data, _server));
        }

        public async Task<int> SendServerRequest(Socket serverHandler, string topic, object data, Socket clientHandler, Guid? requestId = null)
        {
            Guid requesterId;

            bool hasConnection = _clientService.TryGetRequesterId(clientHandler, out requesterId);

            var serverRequest = hasConnection
                ? new ServerRequest(topic, data, requestId, requesterId)
                : new ServerRequest(topic, data, requestId);

            return await SendServerRequest(serverRequest);
        }
    }
}
