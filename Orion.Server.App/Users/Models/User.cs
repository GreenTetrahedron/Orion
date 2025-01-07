using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing.Workflow;

namespace Orion.Server.App.Users.Models
{
    public partial class User : ObservableObject
    {
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private Roles _role;

        [ObservableProperty]
        private Guid _userId;

        [ObservableProperty]
        private string _password;
    }
}
