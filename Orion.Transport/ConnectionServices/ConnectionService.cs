using System.Net;
using System.Net.Sockets;

namespace Orion.Transport.ConnectionServices
{
    public class ConnectionService : IConnectionService, IDisposable
    {
        private readonly IPEndPoint? _routerIpEndpoint;

        private Socket? _router;

        private int _bufferSpace;

        private bool _disposed;

        public ConnectionService(IPEndPoint? routerIpEndpoint = null, int bufferSpace = 16192)
        {
            _routerIpEndpoint = routerIpEndpoint;
            _bufferSpace = bufferSpace;
            _disposed = false;
            _router = null;
            
            if (_routerIpEndpoint == null)
                return;

            _router = new Socket(_routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _router.ConnectAsync(_routerIpEndpoint);
        }

        public async Task<int> SendMessage(byte[] data)
        {
            if (_router == null)
                throw new Exception("router was null...");

            return await SendMessage(data, _router);
        }

        public async Task<int> SendMessage(byte[] data, Socket socket)
        {
            int sentBytes = await socket.SendAsync(data, SocketFlags.None);

            return sentBytes;
        }

        public async Task<MessageBytes> ReceiveMessage()
        {
            if (_router == null)
                throw new Exception("router was null...");

            return await ReceiveMessage(_router);
        }

        public async Task<MessageBytes> ReceiveMessage(Socket socket)
        {
            byte[] buffer = new byte[_bufferSpace];
            int receivedBytes = await socket.ReceiveAsync(buffer, SocketFlags.None);

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

            if (_router != null)
            {
                _router.Shutdown(SocketShutdown.Both);
                _router.Disconnect(false);
                _router.Close();
                _router.Dispose();
            }

            _disposed = true;
        }
    }
}
