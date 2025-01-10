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
    public partial class MessageListViewModel : ObservableObject
    {
        private ObservableCollection<Message> _messages;

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            private set
            {
                SetProperty(ref _messages, value);
                _messages.CollectionChanged += (_, _) => OnMessageListChanged.Invoke();
            }
        }

        [ObservableProperty]
        private int _maxMessageLength;

        [ObservableProperty]
        private int _messageLength;

        [ObservableProperty]
        private bool _sendingMessageContentLengthValid;

        private string _sendingMessageContent;

        public string SendingMessageContent
        {
            get => _sendingMessageContent;
            set
            {
                SendingMessageContentLengthValid = !(value.Length > MaxMessageLength);
                MessageLength = value.Length;

                if (!SendingMessageContentLengthValid)
                {
                    OnMessageContentTooBig.Invoke();
                    value = value[..MaxMessageLength];
                }


                SetProperty(ref _sendingMessageContent, value);
            }
        }

        public event Action OnMessageListChanged = delegate { };
        public event Action OnMessageContentTooBig = delegate { };

        public MessageListViewModel()
        {
            MaxMessageLength = 1000;
            Messages = new();
            SendingMessageContentLengthValid = true;
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
