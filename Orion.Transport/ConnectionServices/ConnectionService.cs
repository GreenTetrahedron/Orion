using System.Net;
using System.Net.Sockets;

namespace Orion.Transport.ConnectionServices
{
    public class ConnectionService : IConnectionService
    {
        private readonly IPEndPoint _routerIpEndpoint;

        private Socket _router;

        private int _bufferSpace;

        public ConnectionService(IPEndPoint routerIpEndpoint, int bufferSpace = 16192)
        {
            _router = new Socket(routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _routerIpEndpoint = routerIpEndpoint;
            _router.ConnectAsync(_routerIpEndpoint);

            _bufferSpace = bufferSpace;
        }

        public async Task<bool> SendMessage(byte[] data)
        {
            int sentBytes = await _router.SendAsync(data, SocketFlags.None);

            return sentBytes == data.Length;
        }

        public async Task<MessageBytes> ReceiveMessage()
        {
            byte[] buffer = new byte[_bufferSpace];
            int receivedBytes = await _router.ReceiveAsync(buffer, SocketFlags.None);

            return new MessageBytes(buffer, receivedBytes);
        }
    }
}
