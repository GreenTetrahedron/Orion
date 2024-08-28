using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Orion.Client.Users.Services;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class Login : Page, INotifyPropertyChanged
    {
        private readonly IUserService _userService;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private Visibility invalidMessageVisibility;

        public Visibility InvalidMessageVisibility
        {
            get { return invalidMessageVisibility; }
            set
            {
                invalidMessageVisibility = value;
                OnPropertyChanged();
            }
        }



        public Login()
        {
            DataContext = this;
            invalidMessageVisibility = Visibility.Collapsed;
            InitializeComponent();
            
            _userService = App.Current.ConfigurationService.GetInstanceOfType<IUserService>();
        }

        private async void OnLogin(object sender, RoutedEventArgs e)
        {
            var username = usernameTextBox.Text;

            var credentials = new Credentials() { Username = username };

            var subscriptable = await _userService.AuthenticateUser(credentials);
            subscriptable.Subscribe(result =>
            {
                if (result.OperationInformation.OperationMessageCode == AuthenticationMessages.VALID_CREDENTIALS)
                    SetInvisible();
                else
                    SetVisible();
            });
        }

        public void SetInvisible()
        {
            InvalidMessageVisibility = Visibility.Collapsed;
        }

        public void SetVisible()
        {
            InvalidMessageVisibility = Visibility.Visible;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
