using Orion.Client.Subscriptions;
using Orion.Client.Subscriptions.Services;
using Orion.Client.TopicHandlers;
using Orion.Client.Transmissions;
using Orion.JsonParser;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Orion.Client.Connections
{
    public class ConnectionService : IConnectionService
    {
        private readonly IPEndPoint _routerIpEndpoint;

        private Socket _router;

        public ConnectionService(IPEndPoint routerIpEndpoint)
        {
            _router = new Socket(routerIpEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _routerIpEndpoint = routerIpEndpoint;
            _router.ConnectAsync(_routerIpEndpoint);
        }

        public async Task<bool> SendMessage(byte[] data)
        {
            int sentBytes = await _router.SendAsync(data, SocketFlags.None);

            return sentBytes == data.Length;
        }

        public async Task<MessageBytes> ReceiveMessage()
        {
            byte[] buffer = new byte[1024];
            int receivedBytes = await _router.ReceiveAsync(buffer, SocketFlags.None);

            return new MessageBytes(buffer, receivedBytes);
        }
    }
}
