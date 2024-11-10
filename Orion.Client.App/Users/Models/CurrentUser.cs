using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Client.App.DirectCommunications.Models;
using System.Collections.ObjectModel;

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
