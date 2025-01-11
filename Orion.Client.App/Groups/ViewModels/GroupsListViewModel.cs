using Orion.Client.App.Groups.Models;
using Orion.Client.App.Messages.Models;
using Orion.Client.App.Messages.ViewModels;
using Orion.Client.App.Users.Models;
using Orion.Client.Groups.Services;
using Orion.Client.Messages.Services;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Groups.ViewModels
{
    public class GroupsListViewModel : MasterDetailViewModel<GroupProfile>
    {
        private readonly MessageListViewModel _messageListViewModel;

        public MessageListViewModel MessageListViewModel => _messageListViewModel;

        private readonly IGroupService _groupService;
        private readonly IMessageService _messageService;

        public GroupsListViewModel()
        {
            _messageListViewModel = new();
            _messageListViewModel.OnSendMessage += SendMessage;


            _groupService = App.Current.ConfigurationService.GetSingletonOfType<IGroupService>();
            _messageService = App.Current.ConfigurationService.GetSingletonOfType<IMessageService>();

            App.Current.CurrentUser.Groups.ToList().ForEach(group => AddItem(group));

            OnSelectedChanged += StartToPopulateMessages;
        }

        private async void StartToPopulateMessages(GroupProfile groupProfile)
        {
            if (groupProfile == null)
                return;

            var subscriptable = await _groupService.GetMessagesByGroupId(groupProfile.GroupId);
            subscriptable.Subscribe(MessageListViewModel.PopulateMessages);
        }

        public async void SendMessage(string content, Action onSuccess, Action onFailure)
        {
            var subscriptable = await _messageService.SendGroupMessage(new NewGroupMessage()
            {
                Content = content,
                GroupId = Selected.GroupId,
                SenderId = App.Current.CurrentUser.UserProfile.UserId,
                LastUpdated = DateTime.Now
            });

            subscriptable.Subscribe(result =>
            {
                if (result.OperationInformation.OperationMessageCode == NewMessageMessages.MESSAGE_CREATED_SUCCESSFULLY)
                {
                    onSuccess?.Invoke();
                    return;
                }

                onFailure?.Invoke();
            });
        }
    }
}
