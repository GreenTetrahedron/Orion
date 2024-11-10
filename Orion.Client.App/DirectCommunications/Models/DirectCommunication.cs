using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Client.App.Users.Models;
using System;

namespace Orion.Client.App.DirectCommunications.Models
{
    public partial class DirectCommunication : ObservableObject
    {
        [ObservableProperty]
        private Guid directCommunicationId;

        [ObservableProperty]
        private UserProfile receiverProfile;
    }
}
