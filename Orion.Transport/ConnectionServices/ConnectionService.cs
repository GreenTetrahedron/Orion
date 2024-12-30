using System.Net;
using System.Net.Sockets;

namespace Orion.Transport.ConnectionServices
{
    public class ConnectionService : IConnectionService, IDisposable
    {
        private readonly IPEndPoint? _routerIpEndpoint;

        private Socket? _router;

        private int _bufferLength;

        private bool _disposed;

        public ConnectionService(IPEndPoint? routerIpEndpoint = null, int bufferLength = 16192)
        {
            _routerIpEndpoint = routerIpEndpoint;
            _bufferLength = bufferLength;
            _disposed = false;
            _router = null;
            
            if (_routerIpEndpoint == null)
                return;

            _router = new Socket(_routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _router.ConnectAsync(_routerIpEndpoint);
            _router.SendBufferSize = Math.Max(_bufferLength, _router.SendBufferSize);
            _router.ReceiveBufferSize = Math.Max(_bufferLength, _router.ReceiveBufferSize);
        }

        public async Task<int> SendMessage(byte[] data)
        {
            if (_router == null)
                throw new Exception("router was null...");

            return await SendMessage(data, _router);
        }

        public async Task<int> SendMessage(byte[] data, Socket socket)
        {
            byte[] finalTransmission = new byte[4 + data.Length];

            Buffer.BlockCopy(BitConverter.GetBytes(data.Length), 0, finalTransmission, 0, 4);
            Buffer.BlockCopy(data, 0, finalTransmission, 4, data.Length);

            int sentBytes = 0;

            byte[] sendBuffer = new byte[_bufferLength];

            do
            {
                if (finalTransmission.Length - sentBytes < _bufferLength)
                    sendBuffer = new byte[finalTransmission.Length - sentBytes];

                Buffer.BlockCopy(finalTransmission, sentBytes, sendBuffer, 0, sendBuffer.Length);
             
                sentBytes += await socket.SendAsync(sendBuffer, SocketFlags.None);
            }
            while (sentBytes < finalTransmission.Length);

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
            byte[] receiveBuffer = new byte[_bufferLength];

            int previousReceivedBytes = 0;
            int receivedBytes = 0;
            int transmissionLength = 4;
            byte[] lengthBuffer = new byte[4];
            int someRandomNumber = 0;

            while (receivedBytes < transmissionLength)
            {
                previousReceivedBytes = await socket.ReceiveAsync(receiveBuffer, SocketFlags.None);
                someRandomNumber = Math.Min(Math.Max(4 - receivedBytes, 0), receiveBuffer.Length);
                Buffer.BlockCopy(receiveBuffer, 0, lengthBuffer, receivedBytes, someRandomNumber);
                receivedBytes += previousReceivedBytes;
            }

            transmissionLength = BitConverter.ToInt32(lengthBuffer);
            byte[] finalBuffer = new byte[transmissionLength];
            receivedBytes -= 4;

            Buffer.BlockCopy(receiveBuffer, someRandomNumber, finalBuffer, 0, receivedBytes);

            while (receivedBytes < transmissionLength)
            {
                previousReceivedBytes = await socket.ReceiveAsync(receiveBuffer, SocketFlags.None);
                Buffer.BlockCopy(receiveBuffer, 0, finalBuffer, receivedBytes, Math.Min(receiveBuffer.Length, transmissionLength - receivedBytes));
                receivedBytes += previousReceivedBytes;
            }

            return new MessageBytes(finalBuffer, finalBuffer.Length);
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
