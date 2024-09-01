using Orion.Client.App.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Messages
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        private Guid _messageId;

        public Guid MessageId
        {
            get { return _messageId; }
            set
            {
                _messageId = value;
                OnPropertyChanged();
            }
        }

        private UserProfileViewModel _senderProfile;

        public UserProfileViewModel SenderProfile
        {
            get { return _senderProfile; }
            set
            {
                _senderProfile = value;
                OnPropertyChanged();
            }
        }

        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
