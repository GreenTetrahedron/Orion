using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App.DirectCommunications.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DirectCommunicationListPage : Page
    {
        public DirectCommunicationListPage()
        {
            InitializeComponent();
            App.Current.CurrentUser.DirectCommunicationListViewModel = Model;

            AddDirectCommunicationPopup.KeyDown += (a, b) =>
            {
                if(b.Key == Windows.System.VirtualKey.Escape && AddDirectCommunicationPopup.IsOpen);
                    AddDirectCommunicationPopup.IsOpen = false;
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs navigationEventArgs)
        {
            Model.Selected = null;
            App.Current.CurrentUser.DirectCommunicationListViewModel = Model;
        }

        private void OnAddDirectCommunicationSuccessful()
        {
            if (AddDirectCommunicationPopup.IsOpen) AddDirectCommunicationPopup.IsOpen = false;
        }

        private void OnAddDirectCommunication(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (!AddDirectCommunicationPopup.IsOpen) AddDirectCommunicationPopup.IsOpen = true;
        }
    }
}
