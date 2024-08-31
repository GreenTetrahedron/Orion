using Orion.Models;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Users
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private Guid _userId;

        public Guid UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DirectCommunicationProfile> _directCommunicationProfiles;

        public ObservableCollection<DirectCommunicationProfile> DirectCommunicationProfiles
        {
            get { return _directCommunicationProfiles; }
            set
            {
                _directCommunicationProfiles = value;
                OnPropertyChanged();
            }
        }


        public UserViewModel(Guid userId, string username)
        {
            UserId = userId;
            Username = username;
            DirectCommunicationProfiles = new ObservableCollection<DirectCommunicationProfile>();
        }

        public UserViewModel(Guid userId, string username, ObservableCollection<DirectCommunicationProfile> directCommunicationProfiles)
        {
            UserId = userId;
            Username = username;
            DirectCommunicationProfiles = directCommunicationProfiles;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
