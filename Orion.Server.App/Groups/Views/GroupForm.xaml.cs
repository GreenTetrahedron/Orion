using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Orion.Server.App.Groups.Models;
using Orion.Server.App.Groups.ViewModels;
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

namespace Orion.Server.App.Groups.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class GroupForm : Page, INotifyPropertyChanged
    {
        private Group _group;

        public Group Group
        {
            get { return _group; }
            set
            {
                _group = value;
                Model.Group = _group;
                OnPropertyChanged();
            }
        }

        public event Action<Group> OnSubmit = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GroupForm()
        {
            this.InitializeComponent();
            Group ??= new();
            Model.Group = Group;
        }

        private void OnAddMember(object sender, RoutedEventArgs e)
        {
            string username = memberUsernameTextEntry.Text;

            Model.UserExists = true;

            memberUsernameTextEntry.Text = "";
            Model.TryAddMemberByUsername(username);
        }

        private void OnRemoveMember(object sender, RoutedEventArgs e)
        {
            Model.RemoveUserById((Guid)((Button)sender).Tag);
        }

        private void OnSubmitClicked(object sender, RoutedEventArgs e)
        {
            OnSubmit.Invoke(Model.Group);
        }
    }
}
