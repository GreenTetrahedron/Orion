using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Orion.Client.App.DirectCommunications.Views;
using Orion.Client.App.Users.Models;
using Orion.Client.App.Users.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class NavbarPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private CurrentUser _model;

        public CurrentUser Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

        private Frame _contentFrame;

        public Frame ContentFrame
        {
            get => _contentFrame;
            set
            {
                _contentFrame = value;
                OnPropertyChanged();
            }
        }

        public NavbarPage()
        {
            this.InitializeComponent();
        }

        private void LoadDirectCommunicationListPage(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(DirectCommunicationListPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter.GetType() != typeof(Frame))
                return;

            ContentFrame = (Frame)e.Parameter;
        }

        private void LoadAddDirectCommunicationPage(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(AddDirectCommunicationPage));
        }
        private void LoadUserDetailsPage(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(UserDetailsPage));
        }
    }
}
