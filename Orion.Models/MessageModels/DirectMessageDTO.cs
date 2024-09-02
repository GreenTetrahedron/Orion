namespace Orion.Models.MessageModels
{
    public class DirectMessageDTO
    {
        public Guid DirectCommunicationId { get; set; }

        public MessageDTO Message { get; set; }
    }
}
