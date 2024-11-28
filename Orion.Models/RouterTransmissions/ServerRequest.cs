namespace Orion.Models.RouterTransmissions
{
    public class ServerRequest
    {
        public string Topic { get; set; }

        public object Data { get; set; }

        public Guid? RequestId { get; set; }

        public Guid? RequesterId { get; set; }

        public ServerRequest(string topic, object data, Guid? requestId = null, Guid? requesterId = null)
        {
            Topic = topic;
            Data = data;
            RequestId = requestId;
            RequesterId = requesterId;
        }
    }
}
