using Orion.Client.App.DirectCommunications.Models;
using Orion.Client.App.Messages.Models;
using Orion.Client.App.Messages.ViewModels;
using Orion.Client.App.Users.Models;
using Orion.Client.DirectCommunications.Services;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Orion.Client.App.DirectCommunications.ViewModels
{
    public partial class DirectCommunicationListViewModel : MasterDetailViewModel<DirectCommunication>
    {
        private MessageListViewModel messageListViewModel;

        public MessageListViewModel MessageListViewModel
        {
            get { return messageListViewModel; }
        }


        private IDirectCommunicationService _directCommunicationService;

        public DirectCommunicationListViewModel()
        {
            _directCommunicationService = App.Current.ConfigurationService.GetSingletonOfType<IDirectCommunicationService>();

            App.Current.CurrentUser.DirectCommunications.ToList().ForEach(directCommunication => AddItem(directCommunication));
            messageListViewModel = new();

            OnSelectedChanged += StartToPopulateMessages;
        }

        private async void StartToPopulateMessages(DirectCommunication directCommunication)
        {
            var subscriptable = await _directCommunicationService.GetMessagesByDirectCommunicationId(directCommunication.DirectCommunicationId);
            subscriptable.Subscribe(PopulateMessages);
        }

        private void PopulateMessages(ServerResult<GetMessageMessages> result)
        {
            if (result.OperationInformation.OperationMessageCode != GetMessageMessages.SUCCESSFULLY_RETRIEVED_MESSAGE)
            {
                return;
            }

            var retrievedMessages = (List<MessageDTO>)result.Data;

            MessageListViewModel.ClearMessages();
            retrievedMessages.ForEach(AddMessageToMessages);
        }

        private void AddMessageToMessages(MessageDTO message)
        {
            MessageListViewModel.AddMessage(new Message()
            {
                MessageId = message.MessageId,
                Content = message.Content,
                SenderProfile = new UserProfile()
                {
                    UserId = message.SenderProfile.UserId,
                    Username = message.SenderProfile.Username
                },
                LastUpdated = message.LastUpdated
            });

        }
    }
}
