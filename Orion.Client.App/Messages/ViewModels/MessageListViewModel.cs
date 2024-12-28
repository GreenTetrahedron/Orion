using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Client.App.Messages.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void ClearMessages()
        {
            Messages.Clear();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }
    }
}
