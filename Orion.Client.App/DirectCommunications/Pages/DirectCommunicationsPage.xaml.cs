using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Orion.Client.App.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App.DirectCommunications
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>


    public sealed partial class DirectCommunicationsPage : Page
    {
        public UserViewModel UserViewModel { get; set; }
        
        public DirectCommunicationsPage()
        {
            this.InitializeComponent();
            UserViewModel = App.Current.UserViewModel;
        }

        public void LoadNewDirectCommunicationPage(object sender, RoutedEventArgs e)
        {
            App.Current.ContentFrame.Navigate(typeof(NewDirectCommunciation));
        }
    }
}
