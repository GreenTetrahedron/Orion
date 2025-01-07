using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class UsersListPage : Page
    {
        public UsersListPage()
        {
            this.InitializeComponent();
        }


        private void OnAddUser(object sender, RoutedEventArgs e)
        {
            Model.HideAddUserForm = false;
        }

        private void OnEditUser(object sender, RoutedEventArgs e)
        {
            Model.OnEditUser((Guid)((Button)sender).Tag);
        }

        private void OnDeleteUser(object sender, RoutedEventArgs e)
        {
            Model.OnDeleteUser((Guid)((Button)sender).Tag);
        }
    }
}
