using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
