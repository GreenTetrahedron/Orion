using Orion.Client.App.Messages.Models;
using Orion.Client.App.Users.Models;
using Orion.Client.Attributes;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Messages.Controllers
{
    [Controller]
    public class MessageController
    {
        private App _app;

        public MessageController(App app)
        {
            _app = app;
        }

        [Handler("NewDirectMessage")]
        public async Task NewDirectMessageHandler(ServerResult result)
        {
            if (result.Data == null)
                return;

            var directMessageDTO = (DirectMessageDTO)result.Data;

            if (!_app.CurrentUser.DirectCommunicationListViewModel.HasSelected || _app.CurrentUser.DirectCommunicationListViewModel.Selected.DirectCommunicationId != directMessageDTO.DirectCommunicationId)
                return;

            _app.CurrentUser.DirectCommunicationListViewModel.MessageListViewModel.Messages.Add(new Message()
            {
                MessageId = directMessageDTO.Message.MessageId,
                Content = directMessageDTO.Message.Content,
                LastUpdated = directMessageDTO.Message.LastUpdated,
                SenderProfile = new UserProfile()
                {
                    UserId = directMessageDTO.Message.SenderProfile.UserId,
                    Username = directMessageDTO.Message.SenderProfile.Username
                }
            });
        }

        [Handler("NewGroupMessage")]
        public async Task NewGroupMessageHandler(ServerResult result)
        {
            if (result.Data == null)
                return;

            var groupMessageDTO = (GroupMessageDTO)result.Data;

            if (!_app.CurrentUser.GroupListViewModel.HasSelected || _app.CurrentUser.GroupListViewModel.Selected.GroupId != groupMessageDTO.GroupId)
                return;

            _app.CurrentUser.GroupListViewModel.MessageListViewModel.Messages.Add(new Message()
            {
                MessageId = groupMessageDTO.Message.MessageId,
                Content = groupMessageDTO.Message.Content,
                LastUpdated = groupMessageDTO.Message.LastUpdated,
                SenderProfile = new UserProfile()
                {
                    UserId = groupMessageDTO.Message.SenderProfile.UserId,
                    Username = groupMessageDTO.Message.SenderProfile.Username
                }
            });
        }
    }
}
