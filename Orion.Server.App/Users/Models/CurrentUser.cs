using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Orion.Server.App.Users.Models
{
    public partial class CurrentUser : ObservableObject
    {
        [ObservableProperty]
        private UserProfile userProfile;
    }
}
