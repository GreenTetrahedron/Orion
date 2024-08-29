using Orion.Models;
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
    public class UserViewModel : User, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private Guid _userId;

        public new Guid UserId
        {
            get { return _userId; }
            private set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        private string _username;

        public new string Username
        {
            get { return _username; }
            private set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DirectCommunication> _directCommunications;

        public ObservableCollection<DirectCommunication> DirectCommunications
        {
            get { return _directCommunications; }
            private set
            {
                _directCommunications = value;
                OnPropertyChanged();
            }
        }



        public UserViewModel(Guid userId, string username)
        {
            UserId = userId;
            username = username;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
