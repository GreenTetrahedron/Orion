using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Client.App.DirectCommunications.Models;
using Orion.Client.App.DirectCommunications.ViewModels;
using System.Collections.ObjectModel;

namespace Orion.Client.App.Users.Models
{
    public partial class CurrentUser : ObservableObject
    {
        [ObservableProperty]
        private UserProfile userProfile;

        [ObservableProperty]
        private ObservableCollection<DirectCommunication> directCommunications;

        [ObservableProperty]
        private DirectCommunicationListViewModel directCommunicationListViewModel;

        public CurrentUser()
        {
            directCommunications = new ObservableCollection<DirectCommunication>();
        }
    }
}
