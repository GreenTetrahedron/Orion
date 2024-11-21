using Orion.JsonParser;
using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Server.TopicHandlers;
using Orion.Transport.ConnectionServices;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Orion.Server
{
    public class ServerService
    {
        private readonly IJsonService _jsonService;
        private readonly ITopicHandlerService _topicHandlerService;
        private readonly IConnectionService _connectionService;

        private const string IDENTIFIER = "SERVER";

        public ServerService(IJsonService jsonService, ITopicHandlerService topicHandlerService, IConnectionService connectionService)
        {
            _topicHandlerService = topicHandlerService;

            _connectionService = connectionService;
            _jsonService = jsonService;
        }

        public async Task Run()
        {
            //Console.WriteLine("Server running...");

            await TransmitData(IDENTIFIER);

            Task.Run(async () =>
            {
                while (true)
                {
                    var request = await ReceiveRequest();
                    //Console.WriteLine($"New request of topic: {request.Topic}");

                    if (request == null)
                        continue;

                    var response = await HandleRequest(request);

                    await TransmitData(response);

                    //Console.WriteLine($"Transmitted response to request of topic: {request.Topic}");
                }
            });

            //Console.ReadLine();
        }

        private async Task<int> TransmitData(object data)
        {
            string transmissionJson = _jsonService.SerialiseObject(data);

            var transmissionBytes = Encoding.UTF8.GetBytes(transmissionJson);

            return await _connectionService.SendMessage(transmissionBytes) ? 1 : 0;
        }

        private async Task<ServerTransmission?> HandleRequest(ServerRequest request)
        {
            var handler = _topicHandlerService.GetTopicHandler(request.Topic);

            if (handler == null)
                throw new ApplicationException($"No handler found for topic: {request.Topic}");

            ServerTransmission result = await handler.Invoke(request.Data);
            result.Response.RequestId = request.RequestId;
            return result;
        }

        private async Task<ServerRequest?> ReceiveRequest()
        {
            var message = await _connectionService.ReceiveMessage();

            string transmissionJson = Encoding.UTF8.GetString(message.Data, 0, message.DataByteLength);

            ServerRequest? request = _jsonService.DeserialiseJson<ServerRequest>(transmissionJson);

            return request;
        }
    }
}
