using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Orion.Client.App.Users.Models;
using Orion.Client.App.Users.Views;
using Orion.Client.Connections;
using Orion.Client.DirectCommunications.Services;
using Orion.Client.Messages.Services;
using Orion.Client.Subscriptions.Services;
using Orion.Client.TopicHandlers;
using Orion.Client.Transmissions;
using Orion.Client.Users.Services;
using Orion.Configuration;
using Orion.JsonParser;
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {
        public new static App Current => (App)Application.Current;

        public IConfigurationService ConfigurationService { get; set; }

        public CurrentUser CurrentUser { get; set; }
        public Frame RootFrame { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var hostName = Dns.GetHostName();

            //Console.WriteLine("Router IP address: ");
            IPAddress routerIPAddress = IPAddress.Parse("192.168.0.26");

            //Console.WriteLine("Router port: ");
            int routerPort = Convert.ToInt32(50000);

            var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);


            var configurationService = new ConfigurationService();

            configurationService.AddInstanceOfType<IJsonService>(new JsonService());

            configurationService.AddInstanceOfType<ISubscriptionService>(new SubscriptionService());
            configurationService.AddInstanceOfType<App>(this);
            configurationService.AddInstanceOfType<ITopicHandlerService>(new TopicHandlerService(configurationService));

            var connectionService = new ConnectionService
                (
                    routerIPEndPoint
                );

            configurationService.AddInstanceOfType<IConnectionService>(connectionService);

            configurationService.AddInstanceOfType<ITransmissionService>(new TransmissionService(
                    configurationService.GetInstanceOfType<IConnectionService>(),
                    configurationService.GetInstanceOfType<IJsonService>(),
                    configurationService.GetInstanceOfType<ISubscriptionService>(),
                    configurationService.GetInstanceOfType<ITopicHandlerService>()
                ));

            configurationService.AddInstanceOfType<IUserService>(new UserService(
                    configurationService.GetInstanceOfType<ITransmissionService>()
                ));

            configurationService.AddInstanceOfType<IMessageService>(new MessageService(
                    configurationService.GetInstanceOfType<ITransmissionService>()
                ));

            configurationService.AddInstanceOfType<IDirectCommunicationService>(new DirectCommunicationService(
                    configurationService.GetInstanceOfType<ITransmissionService>()
                ));

            ConfigurationService = configurationService;

            var transmissionService = ConfigurationService.GetInstanceOfType<ITransmissionService>();

            transmissionService.InitialiseRouterConnection();

            DispatcherQueueTimer d = DispatcherQueue.GetForCurrentThread().CreateTimer();

            d.Interval = TimeSpan.Zero;

            d.Tick += (s, e) =>
            {
                transmissionService.ReceiveData();
            };

            d.Start();

            window = new MainWindow();

            RootFrame = window.RootFrame;

            RootFrame.Navigate(typeof(LoginPage), args.Arguments);
            RootFrame.NavigationFailed += OnNavigationFailed;

            window.Activate();
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private MainWindow window;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
