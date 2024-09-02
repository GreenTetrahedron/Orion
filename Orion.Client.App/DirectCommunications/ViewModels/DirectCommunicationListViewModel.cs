using Orion.Client.App.DirectCommunications.Models;
using Orion.Client.App.Messages.Models;
using Orion.Client.App.Users.Models;
using Orion.Client.DirectCommunications.Services;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Orion.Client.App.DirectCommunications.ViewModels
{
    public partial class DirectCommunicationListViewModel : MasterDetailViewModel<DirectCommunication>
    {
		private ObservableCollection<Message> messages;

		public ObservableCollection<Message> Messages
		{
			get { return messages; }
			private set { SetProperty(ref messages, value); }
		}

		private IDirectCommunicationService _directCommunicationService;

		public DirectCommunicationListViewModel()
		{
			_directCommunicationService = App.Current.ConfigurationService.GetInstanceOfType<IDirectCommunicationService>();

			App.Current.CurrentUser.DirectCommunications.ToList().ForEach(directCommunication => AddItem(directCommunication));
			Messages = new ObservableCollection<Message>();

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

            Messages.Clear();
            retrievedMessages.ForEach(AddMessageToMessages);
		}

		private void AddMessageToMessages(MessageDTO message)
		{
			Messages.Add(new Message()
			{
				MessageId = message.MessageId,
				Content = message.Content,
				SenderProfile = new UserProfile()
				{
					UserId = message.SenderProfile.UserId,
					Username = message.SenderProfile.Username
				}
			});

        }
	}
}
