using Orion.JsonParser;
using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.UserModels;
using Orion.Server.TopicHandlers;
using Orion.Server.Users.Repositories;
using Orion.Transport.ConnectionServices;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Orion.Server
{
    public class ServerService
    {
        private readonly IUserRepository _userRepository;

        private readonly IJsonService _jsonService;
        private readonly ITopicHandlerService _topicHandlerService;
        private readonly IConnectionService _connectionService;

        private const string IDENTIFIER = "SERVER";

        public ServerService(IJsonService jsonService, IUserRepository userRepository, ITopicHandlerService topicHandlerService, IConnectionService connectionService)
        {
            _topicHandlerService = topicHandlerService;

            _connectionService = connectionService;
            _jsonService = jsonService;

            _userRepository = userRepository;
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

            var roles = _topicHandlerService.GetAuthorisedRoleByTopic(request.Topic);
            
            if (roles > Roles.USER && request.RequesterId != null)
            {
                var role = await _userRepository.GetRoleByUserId(request.RequesterId.Value);

                if (role < roles)
                    return new ServerTransmission(new ServerResponse(request.Topic + "Result", new ServerResult(new OperationInformation(Statuses.FAILED, "UNAUTHORISED"))));
            }

            var authorisedRole = _topicHandlerService.GetAuthorisedRoleByTopic(request.Topic);

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
