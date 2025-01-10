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

        public void SendMessage(string content)
        {
            var subscriptable = _messageService.SendDirectMessage(new NewDirectMessage()
            {
                Content = content,
                DirectCommunicationId = Selected.DirectCommunicationId,
                SenderId = App.Current.CurrentUser.UserProfile.UserId,
                LastUpdated = DateTime.Now
            });
        }
    }
}
