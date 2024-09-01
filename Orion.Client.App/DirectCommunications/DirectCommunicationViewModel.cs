using Orion.Client.App.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.DirectCommunications
{
    public class DirectCommunicationViewModel : INotifyPropertyChanged
    {
        private DirectCommunicationProfileViewModel _directCommunicationProfile;

        public DirectCommunicationProfileViewModel DirectCommunicationProfile
        {
            get { return _directCommunicationProfile; }
            set
            {
                _directCommunicationProfile = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MessageViewModel> Messages { get; private set; }

        public DirectCommunicationViewModel(DirectCommunicationProfileViewModel directCommunicationProfile)
        {
            DirectCommunicationProfile = directCommunicationProfile;
            Messages = new ObservableCollection<MessageViewModel>();
        }


        public DirectCommunicationViewModel(DirectCommunicationProfileViewModel directCommunicationProfile, ObservableCollection<MessageViewModel> messages)
        {
            DirectCommunicationProfile = directCommunicationProfile;
            Messages = messages;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
