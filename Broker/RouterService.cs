using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    public class RouterService
    {
        private IPEndPoint _iPEndPoint;
        private ConcurrentDictionary<Guid, Socket> _userIdToSocket;


        public RouterService(IPEndPoint iPEndPoint)
        {
            _userIdToSocket = new ConcurrentDictionary<Guid, Socket>();

            _iPEndPoint = iPEndPoint;
        }

        public async Task Run()
        {
            var listener = new Socket(_iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(_iPEndPoint);

            listener.Listen(100);

            Task.Run( () =>
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

            var message = await ReceiveTransmission(handler);
        }

        public async Task<byte[]> ReceiveTransmission(Socket handler)
        {
            var buffer = new byte[1024];

            handler.ReceiveAsync(buffer, SocketFlags.None);

            return buffer;
        }
    }
}
