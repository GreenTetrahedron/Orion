using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Orion.Client.Transmissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Initial : Page
    {
        public Initial()
        {
            this.InitializeComponent();
        }

        private void Work(object sender, RoutedEventArgs e)
        {
            App.Current.ConfigurationService.GetInstanceOfType<ITransmissionService>().InitialiseRouterConnection();

            DispatcherQueueTimer d = DispatcherQueue.GetForCurrentThread().CreateTimer();

            d.Interval = TimeSpan.Zero;

            d.Tick += (s, e) =>
            {
                App.Current.ConfigurationService.GetInstanceOfType<ITransmissionService>().ReceiveData();
            };

            d.Start();

            Frame.Navigate(typeof(Login));
        }
    }
}
