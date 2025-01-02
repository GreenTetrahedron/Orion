using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Client.App.DirectCommunications.Models;
using Orion.Client.App.DirectCommunications.ViewModels;
using Orion.Client.App.Groups.Models;
using Orion.Client.App.Groups.ViewModels;
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
        private ObservableCollection<GroupProfile> groups;

        [ObservableProperty]
        private DirectCommunicationListViewModel directCommunicationListViewModel;

        [ObservableProperty]
        private GroupsListViewModel groupListViewModel;

        public CurrentUser()
        {
            directCommunications = new ObservableCollection<DirectCommunication>();
            groups = new();
        }
    }
}
