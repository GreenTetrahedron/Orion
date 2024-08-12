using Orion.Models;
using System;
using System.Net;
using System.Net.Sockets;

namespace Orion.Server
{
    public class ServerService
    {
        private Socket _router;

        private IPEndPoint _routerIpEndpoint;

        public ServerService(IPEndPoint routerIpEndpoint)
        {
            _router = new Socket(routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _routerIpEndpoint = routerIpEndpoint;
        }

        public void Run()
        {
            Console.WriteLine("Server running...");

            _router.ConnectAsync(_routerIpEndpoint);
        }

        private async Task<ServerRequest> ReceiveTransmission()
        {
            byte[] buffer = new byte[1024];

            int receivedBytesCount = await _router.ReceiveAsync(buffer);

            ServerRequest data = buffer;
        }
    }
}
