using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.UserModels;
using Orion.Server.App.Users.Models;
using Orion.Server.DirectCommuncations;
using Orion.Server.Users.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Server.App.Users.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class LoginPage : Page, INotifyPropertyChanged
    {
        private readonly IUserRepository _userRepository;


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isValid;

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }

        public LoginPage()
        {
            this.InitializeComponent();

            _userRepository = App.Current.ConfigurationService.GetInstanceOfType<IUserRepository>();
            _isValid = true;
        }

        private async void OnLogin(object sender, RoutedEventArgs e)
        {

            var enteredUsername = usernameTextEntry.Text;
            var enteredPassword = passwordTextEntry.Password;
            var credentials = new Credentials() { Username = enteredUsername, Password = enteredPassword };

            IsValid = true;

            var result = await _userRepository.AuthenticateSuperadmin(credentials);
            
            OnAuthenticationResultReceived(result);
        }
        private void OnAuthenticationResultReceived(ServerResult<AuthenticationMessages> result)
        {
            if (result.OperationInformation.OperationMessageCode != AuthenticationMessages.VALID_CREDENTIALS)
            {
                IsValid = false;
                return;
            }

            IsValid = true;

            var user = (UserDTO)result.Data;

            App.Current.CurrentUser = new CurrentUser()
            {
                UserProfile = new Models.UserProfile()
                {
                    UserId = user.UserId,
                    Username = user.Username
                }
            };

            App.Current.RootFrame.Navigate(typeof(MainPage));
        }
    }
}
