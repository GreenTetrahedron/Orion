using System.Net;
using System.Net.Sockets;

namespace Orion.Transport.ConnectionServices
{
    public class ConnectionService : IConnectionService, IDisposable
    {
        private readonly IPEndPoint? _routerIpEndpoint;

        private readonly Socket? _socket;

        private readonly int _bufferLength;

        private bool _disposed;

        public ConnectionService(IPEndPoint? routerIpEndpoint = null, int bufferLength = 16192)
        {
            _routerIpEndpoint = routerIpEndpoint;
            _bufferLength = bufferLength;
            _disposed = false;
            _socket = null;
            
            if (_routerIpEndpoint == null)
                return;

            _socket = new Socket(_routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            SetupSocket();

            _socket.ConnectAsync(_routerIpEndpoint);
        }

        public ConnectionService(Socket socket, int bufferLength = 16192)
        {
            _socket = socket;
            _bufferLength = bufferLength;

            SetupSocket();
        }

        private void SetupSocket()
        {
            _socket.SendBufferSize = Math.Max(_bufferLength, _socket.SendBufferSize);
            _socket.ReceiveBufferSize = Math.Max(_bufferLength, _socket.ReceiveBufferSize);
        }

        public async Task<int> SendMessage(byte[] data)
        {
            if (_socket == null)
                throw new Exception("socket was null...");

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
             
                sentBytes += await _socket.SendAsync(sendBuffer, SocketFlags.None);
            }
            while (sentBytes < finalTransmission.Length);

            return sentBytes;
        }

        public async Task<MessageBytes> ReceiveMessage()
        {
            if (_socket == null)
                throw new Exception("router was null...");

            byte[] receiveBuffer = new byte[_bufferLength];

            int previousReceivedBytes = 0;
            int receivedBytes = 0;
            int transmissionLength = 4;
            byte[] lengthBuffer = new byte[4];
            int someRandomNumber = 0;

            while (receivedBytes < transmissionLength)
            {
                previousReceivedBytes = await _socket.ReceiveAsync(receiveBuffer, SocketFlags.None);

                if (previousReceivedBytes == 0)
                    return new MessageBytes(lengthBuffer, receivedBytes);

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
                previousReceivedBytes = await _socket.ReceiveAsync(receiveBuffer, SocketFlags.None);

                if (previousReceivedBytes == 0)
                    return new MessageBytes(finalBuffer, receivedBytes);

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

            if (_socket != null)
            {
                if (_socket.Connected)
                    _socket.Shutdown(SocketShutdown.Both);
                
                _socket.Disconnect(false);
                _socket.Close();
                _socket.Dispose();
            }

            _disposed = true;
        }
    }
}
