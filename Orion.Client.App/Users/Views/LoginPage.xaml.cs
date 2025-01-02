using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Orion.Client.Users.Services;
using Orion.Models.UserModels;
using Orion.Models.ServerTransmissions.Results;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Orion.Client.App.Users.Models;
using Orion.Client.App.DirectCommunications.Models;
using Orion.Client.App.Groups.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App.Users.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class LoginPage : Page, INotifyPropertyChanged
    {
        private readonly IUserService _userService = App.Current.ConfigurationService.GetSingletonOfType<IUserService>();

        private bool _isValid;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
            InitializeComponent();
            _isValid = true;
        }

        private async void OnLogin(object sender, RoutedEventArgs e)
        {
            var enteredUsername = usernameTextEntry.Text;
            var enteredPassword = passwordTextEntry.Password;
            var credentials = new Credentials() { Username = enteredUsername, Password = enteredPassword };

            var subscriptable = await _userService.AuthenticateUser(credentials);
            IsValid = true;
            subscriptable.Subscribe(OnAuthenticationResultReceived);
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

            var groups = user.GroupProfiles.Select(group => new GroupProfile()
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName
            }).ToList();

            groups.ForEach(App.Current.CurrentUser.Groups.Add);


            App.Current.RootFrame.Navigate(typeof(MainPage));
        }
    }
}
