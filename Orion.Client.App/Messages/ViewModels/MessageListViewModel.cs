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
using Microsoft.UI.Xaml.Controls;
using System.Reflection;

namespace Orion.Client.App.Messages.ViewModels
{
    public partial class MessageListViewModel : ObservableObject
    {
        public event Action<string, Action, Action> OnSendMessage;

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

        private bool _isOnTimeout;
        public bool IsOnTimeOut
        {
            get => _isOnTimeout;
            private set
            {
                SetProperty(ref _isOnTimeout, value);
                
                OnPropertyChanged(nameof(CanSendMessage));
            }
        }

        public bool CanSendMessage => !IsOnTimeOut && ActualMessageContentLengthValid;

        public int ActualMessageLength => ActualMessageContent.Length;
        public bool ActualMessageContentLengthValid => !(ActualMessageLength > MaxMessageLength);

        private string _actualMessageContent;

        public string ActualMessageContent
        {
            get { return _actualMessageContent; }
            set
            {
                SetProperty(ref _actualMessageContent, value);
                SendingMessageContent = _actualMessageContent;

                OnPropertyChanged(nameof(ActualMessageLength));
                OnPropertyChanged(nameof(ActualMessageContentLengthValid));
            }
        }

        private string _sendingMessageContent;

        public string SendingMessageContent
        {
            get => _sendingMessageContent;
            private set
            {
                if (!ActualMessageContentLengthValid)
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
            IsOnTimeOut = false;

            MaxMessageLength = 1000;
            ActualMessageContent = "";
            
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

        public void SendMessage()
        {
            if (!ActualMessageContentLengthValid)
                return;
            var content = SendingMessageContent.Trim();

            if (string.IsNullOrEmpty(content))
                return;

            IsOnTimeOut = true;

            Timer.Wait(0.3f, () =>
            {
                IsOnTimeOut = false;
            });

            OnSendMessage.Invoke(content, () => ActualMessageContent = "", null);
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
