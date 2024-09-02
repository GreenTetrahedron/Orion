using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Client.App.Users.Models;
using System;

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
