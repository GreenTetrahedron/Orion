using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Orion.Client.Users.Services;
using Orion.Models.UserModels;
using Orion.Models.ServerTransmissions.Results;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Orion.Client.App.DirectCommunications.Views;
using Orion.Client.App.Users.Models;
using Orion.Client.App.DirectCommunications.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App.Users.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class LoginPage : Page, INotifyPropertyChanged
    {
        private readonly IUserService _userService = App.Current.ConfigurationService.GetInstanceOfType<IUserService>();

        private Visibility _visible;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Visibility Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnPropertyChanged();
            }
        }


        public LoginPage()
        {
            InitializeComponent();
            Visible = Visibility.Collapsed;
        }

        private async void OnLogin(object sender, RoutedEventArgs e)
        {
            var enteredUsername = usernameTextEntry.Text;
            var credentials = new Credentials() { Username = enteredUsername };

            var subscriptable = await _userService.AuthenticateUser(credentials);
            subscriptable.Subscribe(OnAuthenticationResultReceived);
        }

        private void OnAuthenticationResultReceived(ServerResult<AuthenticationMessages> result)
        {
            if (result.OperationInformation.OperationMessageCode != AuthenticationMessages.VALID_CREDENTIALS)
            {
                Visible = Visibility.Visible;
                return;
            }

            Visible = Visibility.Collapsed;

            var user = (UserDTO)result.Data;

            App.Current.CurrentUser = new CurrentUser()
            {
                UserProfile = new Models.UserProfile()
                {
                    UserId = user.UserId,
                    Username = user.Username
                }
            };

            var directCommunications = user.DirectCommunicationProfiles.Select(directCommunication => new DirectCommunication()
            {
                DirectCommunicationId = directCommunication.DirectCommunicationId,
                ReceiverProfile = directCommunication.MemberProfiles.Where(member => member.UserId != user.UserId).Select(member => new Models.UserProfile()
                {
                    UserId = member.UserId,
                    Username = member.Username
                }).Single()
            }).ToList();

            directCommunications.ForEach(App.Current.CurrentUser.DirectCommunications.Add);


            App.Current.RootFrame.Navigate(typeof(MainPage));
        }
    }
}
