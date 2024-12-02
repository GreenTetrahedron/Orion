using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Orion.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class UserForm : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Roles> Roles;

        public event Action<string, string, Roles> OnSubmit = delegate { };

        public UserForm()
        {
            this.InitializeComponent();

            Roles =
            [
                Orion.Models.UserModels.Roles.USER,
                Orion.Models.UserModels.Roles.ADMIN,
                Orion.Models.UserModels.Roles.SUPERADMIN,
            ];
        }

        private void OnSubmitClicked(object sender, RoutedEventArgs e)
        {
            OnSubmit.Invoke(usernameTextEntry.Text, passwordTextEntry.Text, (Roles)rolesComboBox.SelectedItem);
        }
    }
}
