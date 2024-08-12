using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Router
{
    public class RouterService
    {
        private IPEndPoint _iPEndPoint;
        private ConcurrentDictionary<Guid, Socket> _userIdToSocket;
        private Socket _server;

        private ConcurrentDictionary<Guid, Action<ServerResponse>> _requestIdToHandler;


        public RouterService(IPEndPoint iPEndPoint)
        {
            _userIdToSocket = new ConcurrentDictionary<Guid, Socket>();
            _requestIdToHandler = new ConcurrentDictionary<Guid, Action<string>>();

            _iPEndPoint = iPEndPoint;
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

            string message = await ReceiveTransmission(handler);

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

            string username = await ReceiveTransmission(handler);

            Console.WriteLine($"Username received: {username}");

            TransmitData(_server, $"NEW CLIENT: {username}");
            
            _requestIdToHandler[Guid.NewGuid()] = (userId) => {
                Console.WriteLine($"New user id: {userId}");
                _userIdToSocket[Guid.Parse(userId)] = handler;
                };
        }

        public async Task ServerConnection(Socket serverHandler)
        {
            _server = serverHandler;
            Console.WriteLine("Server connected...");

            while (true)
            {
                string message = await ReceiveTransmission(_server);
                string[] processResult = message.Split(" ");

                _handlerToProcess[processResult[0]].Invoke(processResult[1]);
            }
        }

        public async Task<string> ReceiveTransmission(Socket handler)
        {
            var buffer = new byte[1024];

            int transmissionBytesCount = await handler.ReceiveAsync(buffer, SocketFlags.None);

            return Encoding.UTF8.GetString(buffer, 0, transmissionBytesCount);
        }

        public async Task<int> TransmitData(Socket handler, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            return await handler.SendAsync(messageBytes);
        }
    }
}
