using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Client.App.Messages.Models;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orion.Client.App.Users.Models;
using System.Reflection.Metadata.Ecma335;

namespace Orion.Client.App.Messages.ViewModels
{
    public class MessageListViewModel : ObservableObject
    {
        private ObservableCollection<Message> messages;

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set
            {
                SetProperty(ref messages, value);
                messages.CollectionChanged += (_, _) => OnMessageListChanged.Invoke();
            }
        }

        public event Action OnMessageListChanged = delegate { };

        public MessageListViewModel()
        {
            Messages = new();
        }

        public void PopulateMessages(ServerResult<GetMessageMessages> result)
        {
            if (result.OperationInformation.OperationMessageCode != GetMessageMessages.SUCCESSFULLY_RETRIEVED_MESSAGE)
            {
                return;
            }

            PopulateMessages(result.Data as List<MessageDTO>);
        }

        public void PopulateMessages(List<MessageDTO> messages)
        {
            Messages.Clear();
            messages.ForEach(AddMessageToMessages);
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
                },
                LastUpdated = message.LastUpdated
            });
        }
    }
}
