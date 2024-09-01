using Orion.Client.App.Users;
using Orion.Client.Attributes;
using Orion.Models.MessageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Messages.Controllers
{
    [Controller]
    public class MessageController : IMessageController
    {
        private readonly App _app;

        public MessageController(App app)
        {
            _app = app;
        }

        [Handler("NewDirectMessage")]
        public void NewDirectMessage(DirectMessageDTO directMessage)
        {
            _app.DirectCommunications.Where(directCommunication => directCommunication.DirectCommunicationProfile.DirectCommunicationId == directMessage.DirectCommunicationId)
                .Single()
                .Messages.Add(new MessageViewModel()
                {
                    MessageId = directMessage.Message.MessageId,
                    Content = directMessage.Message.Content,
                    SenderProfile = new UserProfileViewModel()
                    {
                        UserId = directMessage.Message.SenderProfile.UserId,
                        Username = directMessage.Message.SenderProfile.Username
                    }
                });
        }
    }
}
