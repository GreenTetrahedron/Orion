using Orion.JsonParser;
using Orion.Models.ClientTransmissions;
using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Router.Clients;
using Orion.Router.Models;
using Orion.Router.Requests;
using Orion.Router.TopicInterceptors;
using Orion.Transport.ConnectionServices;
using Orion.Transport.TransmissionServices;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Orion.Router
{
    public class RouterService
    {
        private IPEndPoint _iPEndPoint;
        private LifeSupport? _serverConnection;

        private IRequestService _requestService;
        private IClientService _clientService;
        private ITopicInterceptorService _topicInterceptorService;

        private readonly ConnectionService _connectionService;

        public RouterService(IPEndPoint iPEndPoint, ITopicInterceptorService topicInterceptorService, IClientService clientService, IRequestService requestService)
        {
            _iPEndPoint = iPEndPoint;

            _topicInterceptorService = topicInterceptorService;
            _clientService = clientService;
            _requestService = requestService;

            _connectionService = new();
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

            var connection = await _clientService.InstantiateConnection(handler);

            string message = await connection.ReceiveTransmission<string>();

            switch (message)
            {
                case "SERVER":
                    ServerConnection(connection);
                    break;
                default:
                    NewClientConnection(connection);
                    break;
            }
            Console.WriteLine("Connection handled...");
        }

        public async Task NewClientConnection(LifeSupport connection)
        {
            if (_serverConnection == null)
            {
                Console.WriteLine("No server... Cannot connect client...");
                return;
            }

            ClientTransmission? clientTransmission = null;

            while (true)
            {
                try
                {
                    clientTransmission = await ReceiveClientTransmission(connection);
                }
                catch (Exception e)
                {
                    connection.SendTransmission("Invalid request format...");
                }

                if (clientTransmission == null)
                    break;

                bool successful = _requestService.NewRequest(ref connection, out Guid requestId);

                if (!successful)
                    throw new Exception("Could not add request...");

                SendServerRequest(clientTransmission.Topic, clientTransmission.Data, connection, requestId);
            }

            Console.WriteLine("Client disconnecting...");

            if (_clientService.TerminateConnection(ref connection))
                Console.WriteLine("Client disconnected");
            else
                Console.WriteLine("No handler found for client....");
        }

        public async Task ServerConnection(LifeSupport connection)
        {
            if (_serverConnection != null)
            {
                connection.SendTransmission("Server already connected you dolt.");
                return;
            }

            _serverConnection = connection;
            Console.WriteLine("Server connected...");

            while (true)
            {
                ServerTransmission? serverTransmission = await ReceiveServerTransmission();

                if (serverTransmission == null)
                    break;

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
            ServerTransmission? serverTransmission = await _serverConnection.ReceiveTransmission<ServerTransmission>();

            return serverTransmission;
        }

        public async Task<ClientTransmission?> ReceiveClientTransmission(LifeSupport connection)
        {
            var clientTransmission = await connection.ReceiveTransmission<ClientTransmission>();

            if (clientTransmission == null)
                return clientTransmission;

            Console.WriteLine($"New client transmission of topic: {clientTransmission.Topic}");

            return clientTransmission;
        }

        public async Task<int> ForwardServerTransmission(ServerResponse serverResponse)
        {
            LifeSupport? requesterConnection = null;
            int result = 0;


            if (serverResponse.RequestId != null && _requestService.TryGetRequestConnection(serverResponse.RequestId.Value, out requesterConnection) && requesterConnection != null)
            {
                result += await ForwardServerResponse(serverResponse, requesterConnection);
                _requestService.TryEndRequest(serverResponse.RequestId.Value, out _);
            }

            if (serverResponse.AffectedUsers == null)
                return result;


            foreach(var userId in serverResponse.AffectedUsers)
            {
                if (requesterConnection != null && requesterConnection.IsLoggedIn && requesterConnection.UserId == userId)
                    continue;

                if (_clientService.TryGetClientConnection(userId, out LifeSupport? connection) && connection != null)
                {
                    result += await ForwardServerResponse(serverResponse, connection);
                }
            }

            return result;
        }

        public async Task<int> ForwardServerResponse(ServerResponse serverResponse, LifeSupport clientConnection)
        {
            var transmission = new ClientTransmission(serverResponse.Topic, serverResponse.ServerResult);

            Console.WriteLine($"Transmitting new response of topic: {serverResponse.Topic}");

            return await clientConnection.SendTransmission(transmission);
        }

        public async Task<int> SendServerRequest(ServerRequest serverRequest)
        {
            return await _serverConnection.SendTransmission(serverRequest);
        }

        public async Task<int> SendServerRequest(string topic, object data, LifeSupport clientConnection, Guid? requestId = null)
        {
            bool hasConnection = clientConnection.IsLoggedIn;
         
            Guid requesterId = clientConnection.UserId;

            var serverRequest = hasConnection
                ? new ServerRequest(topic, data, requestId, requesterId)
                : new ServerRequest(topic, data, requestId);

            return await SendServerRequest(serverRequest);
        }
    }
}
