using Orion.Models.UserModels;

namespace Orion.Models.MessageModels
{
    public class MessageDTO
    {
        public Guid MessageId { get; set; }

        public UserProfile SenderProfile { get; set; }

        public string Content { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
