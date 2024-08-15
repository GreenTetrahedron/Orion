namespace Orion.Server.Messages
{
    public class Message
    {
        public Guid MessageId { get; set; }

        public string Content { get; set; }

        public Guid SenderId { get; set; }
    }
}
