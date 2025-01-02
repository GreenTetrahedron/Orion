using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Orion.Server.App.Groups.Models;
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
    public sealed partial class GroupForm : Page
    {
        public event Action<Group> OnSubmit = delegate { };

        public GroupForm()
        {
            this.InitializeComponent();
        }

        private void OnAddMember(object sender, RoutedEventArgs e)
        {
            string username = memberUsernameTextEntry.Text;

            Model.UserExists = true;

            memberUsernameTextEntry.Text = "";
            Model.TryAddMemberByUsername(username);
        }

        private void OnSubmitClicked(object sender, RoutedEventArgs e)
        {
            OnSubmit.Invoke(Model.Group);
        }
    }
}
