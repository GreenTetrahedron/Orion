using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Orion.Client.App.Users.Models
{
    public partial class UserProfile : ObservableObject
    {
        [ObservableProperty]
        private Guid userId;

        [ObservableProperty]
        private string username;
    }
}
