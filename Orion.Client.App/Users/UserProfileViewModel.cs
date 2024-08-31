using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Users
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        private Guid _userId;

        public Guid UserId
        {
            get { return _userId; }
            private set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            private set
            {
                _username = value;
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
