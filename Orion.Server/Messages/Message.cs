using Orion.Models.MessageModels;
using Orion.Server.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.Server.Messages
{
    public class Message
    {
        public Guid MessageId { get; set; }

        public string Content { get; set; }

        [ForeignKey("Sender")]
        public Guid SenderId { get; set; }

        public User Sender { get; set; }

        public DateTime LastUpdated { get; set; }


        public static explicit operator MessageDTO(Message message) =>
            new MessageDTO()
            {
                MessageId = message.MessageId,
                Content = message.Content,
                SenderProfile = message.Sender,
                LastUpdated = message.LastUpdated
            };
    }
}
