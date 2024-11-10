namespace Orion.Models.MessageModels
{
    public class NewDirectMessage
    {
        public Guid DirectCommunicationId { get; set; }

        public Guid SenderId { get; set; }

        public string Content { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
