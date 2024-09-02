using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Client.App.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Messages.Models
{
    public partial class Message : ObservableObject
    {
        [ObservableProperty]
        private Guid messageId;

        [ObservableProperty]
        private string content;

        [ObservableProperty]
        private UserProfile senderProfile;
    }
}
