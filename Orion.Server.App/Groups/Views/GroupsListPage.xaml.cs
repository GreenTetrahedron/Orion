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

namespace Orion.Server.App.Groups.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupsListPage : Page
    {
        public GroupsListPage()
        {
            this.InitializeComponent();
        }


        private void OnAddGroup(object sender, RoutedEventArgs e)
        {
            Model.HideAddGroupForm = false;
        }
     
        private void OnEditGroup(object sender, RoutedEventArgs e)
        {
            Model.OnEditGroup((Guid)((Button)sender).Tag);
            Model.HideEditGroupForm = false;

        }

        private void OnDeleteGroup(object sender, RoutedEventArgs e)
        {
            // Do something
        }
    }
}
