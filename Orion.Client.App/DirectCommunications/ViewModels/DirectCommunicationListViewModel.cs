using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Orion.Client.App.DirectCommunications.Models;
using Orion.Client.App.Messages.Models;
using Orion.Client.App.Messages.ViewModels;
using Orion.Client.App.Users.Models;
using Orion.Client.DirectCommunications.Services;
using Orion.Client.Messages.Services;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Orion.Client.App.DirectCommunications.ViewModels
{
    public partial class DirectCommunicationListViewModel : MasterDetailViewModel<DirectCommunication>
    {
        [ObservableProperty]
        private MessageListViewModel _messageListViewModel;

        private readonly IDirectCommunicationService _directCommunicationService;
        private readonly IMessageService _messageService;


        public DirectCommunicationListViewModel()
        {
            _directCommunicationService = App.Current.ConfigurationService.GetSingletonOfType<IDirectCommunicationService>();
            _messageService = App.Current.ConfigurationService.GetSingletonOfType<IMessageService>();

            App.Current.CurrentUser.DirectCommunications.ToList().ForEach(directCommunication => AddItem(directCommunication));
            MessageListViewModel = new();

            MessageListViewModel.OnSendMessage += SendMessage;

            OnSelectedChanged += StartToPopulateMessages;
            
            if (Items.Count > 0)
                Selected = Items[0];
        }

        private async void StartToPopulateMessages(DirectCommunication directCommunication)
        {
            if (directCommunication == null)
                return;

            var subscriptable = await _directCommunicationService.GetMessagesByDirectCommunicationId(directCommunication.DirectCommunicationId);
            subscriptable.Subscribe(MessageListViewModel.PopulateMessages);
        }

        public async void SendMessage(string content, Action onSuccess = null, Action onFailure = null)
        {
            var subscriptable = await _messageService.SendDirectMessage(new NewDirectMessage()
            {
                Content = content,
                DirectCommunicationId = Selected.DirectCommunicationId,
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
