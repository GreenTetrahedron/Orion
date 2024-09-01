using Orion.Client.App.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.DirectCommunications
{
    public class DirectCommunicationProfileViewModel : INotifyPropertyChanged
    {
        private Guid _directCommunicationId;

        public Guid DirectCommunicationId
        {
            get { return _directCommunicationId; }
            set
            {
                _directCommunicationId = value;
                OnPropertyChanged();
            }
        }

        private UserProfileViewModel _receiverProfile;

        public UserProfileViewModel ReceiverProfile
        {
            get { return _receiverProfile; }
            set { _receiverProfile = value; }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
