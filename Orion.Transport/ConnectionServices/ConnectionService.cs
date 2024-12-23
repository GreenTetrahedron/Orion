using System.Net;
using System.Net.Sockets;

namespace Orion.Transport.ConnectionServices
{
    public class ConnectionService : IConnectionService, IDisposable
    {
        private readonly IPEndPoint _routerIpEndpoint;

        private Socket _router;

        private int _bufferSpace;

        private bool _disposed;

        public ConnectionService(IPEndPoint routerIpEndpoint, int bufferSpace = 16192)
        {
            _router = new Socket(routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _routerIpEndpoint = routerIpEndpoint;
            _router.ConnectAsync(_routerIpEndpoint);

            _bufferSpace = bufferSpace;


            _disposed = false;
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

        ~ConnectionService()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _router.Shutdown(SocketShutdown.Both);
            _router.Disconnect(false);
            _router.Close();
            _router.Dispose();

            _disposed = true;
        }
    }
}
