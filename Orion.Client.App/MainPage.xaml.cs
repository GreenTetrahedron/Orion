using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Orion.Client.App.Groups.Models;
using Orion.Client.App.Groups.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            App.Current.CurrentUser.GroupListViewModel = GroupListViewModel;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationFrame.Navigate(typeof(NavbarPage), contentFrame);

            base.OnNavigatedTo(e);
        }
    }
}
