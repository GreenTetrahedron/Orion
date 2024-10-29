using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Orion.Client.DirectCommunications.Services;
using Orion.Client.Users.Services;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;
using Orion.Models.DirectCommunicationModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App.DirectCommunications.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddDirectCommunicationPage : Page
    {
        private readonly IDirectCommunicationService _directCommunicationService;
        private readonly IUserService _userService;

        public AddDirectCommunicationPage()
        {
            InitializeComponent();
            _directCommunicationService = App.Current.ConfigurationService.GetSingletonOfType<IDirectCommunicationService>();
            _userService = App.Current.ConfigurationService.GetSingletonOfType<IUserService>();
        }

        private void AddDirectCommunication(object sender, RoutedEventArgs e)
        {
            var otherUserName = otherUserNameTextEntry.Text;

            FindUserByName(otherUserName, async id =>
            {
                var subscriptable = await _directCommunicationService.NewDirectCommunication(new NewDirectCommunication()
                {
                    ReceiverId = id,
                    SenderId = App.Current.CurrentUser.UserProfile.UserId
                });

                subscriptable.Subscribe(result =>
                {
                    // Do something
                });
            });
        }

        private async void FindUserByName(string username, Action<Guid> onCompletedSuccessfully)
        {
            var subscriptable = await _userService.GetUserByUsername(username);
            subscriptable.Subscribe(result =>
            {
                if (result.OperationInformation.OperationMessageCode != GetUserMessages.USER_FOUND)
                    return;

                var user = (UserProfile)result.Data;

                onCompletedSuccessfully(user.UserId);
            });
        }
    }
}
