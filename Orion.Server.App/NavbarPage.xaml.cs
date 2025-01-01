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
using Orion.Server.App.Logging.Views;
using Orion.Server.App.Users.Views;
using Orion.Server.App.Groups.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Server.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavbarPage : Page
    {
        private Frame contentFrame;

        public NavbarPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter.GetType() != typeof(Frame))
                return;

            contentFrame = (Frame)e.Parameter;
        }

        private void LoadLogsPage(object sender , RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(LogsPage));
        }

        private void LoadUsersPage(object sender , RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(UsersListPage));
        }

        private void LoadGroupsPage(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(GroupsListPage));
        }
    }
}
