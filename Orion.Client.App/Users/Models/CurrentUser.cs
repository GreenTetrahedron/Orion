using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Client.App.DirectCommunications.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Users.Models
{
    public partial class CurrentUser : ObservableObject
    {
        [ObservableProperty]
        private UserProfile userProfile;

        [ObservableProperty]
        private ObservableCollection<DirectCommunication> directCommunications;

        public CurrentUser()
        {
            directCommunications = new ObservableCollection<DirectCommunication>();
        }
    }
}
