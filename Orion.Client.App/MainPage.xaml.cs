using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Orion.Client.App.Groups.Models;
using Orion.Client.App.Groups.Views;
using Orion.Client.App.Users.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private CurrentUser _currentUser;

        public CurrentUser CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }


        public MainPage()
        {
            InitializeComponent();

            App.Current.CurrentUser.GroupListViewModel = GroupListViewModel;

            CurrentUser = App.Current.CurrentUser;

            GroupListViewModel.OnSelectedChanged += (groupProfile) =>
            {
                NavigateToGroupPage();
            };
        }

        private void NavigateToGroupPage()
        {
            if (GroupListViewModel.HasSelected)
                return;

            contentFrame.Navigate(typeof(GroupPage), GroupListViewModel);
        }
    }
}
