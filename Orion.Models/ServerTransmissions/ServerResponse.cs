using Orion.Models.ServerTransmissions.Results;

namespace Orion.Models.ServerTransmissions
{
    public class ServerResponse
    {
        public Guid? RequestId { get; set; }

        public ServerResult? ServerResult { get; set; }

        public Guid[]? AffectedUsers { get; set; }

        public string Topic { get; set; }

        public ServerResponse(string topic, ServerResult? serverResult = null, Guid? requestId = null, Guid[]? affectedUsers = null)
        {
            Topic = topic;
            ServerResult = serverResult;
            RequestId = requestId;
            AffectedUsers = affectedUsers;
        }
    }
}
