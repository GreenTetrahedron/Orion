using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Orion.Configuration;
using Orion.Cryptography.HashingServices;
using Orion.JsonParser;
using Orion.Logging.DataLayer;
using Orion.Logging.LoggingServices;
using Orion.Server.App.Users.Models;
using Orion.Server.App.Users.Views;
using Orion.Server.DataLayer;
using Orion.Server.DirectCommunications.Repositories;
using Orion.Server.Groups.Repositories;
using Orion.Server.Messages.Repositories;
using Orion.Server.TopicHandlers;
using Orion.Server.Users.Repositories;
using Orion.Transport.ConnectionServices;
using Orion.Transport.TransmissionServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Server.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public new static App Current = (App)Application.Current;

        public CurrentUser CurrentUser { get; set; }

        public IConfigurationService ConfigurationService { get; set; }

        public Frame RootFrame { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var configurationService = new ConfigurationService();

            configurationService.AddSingleton<IConfigurationService>(configurationService);

            configurationService.AddScoped<LoggingDbContext, LoggingDbContext>();
            configurationService.AddScoped<LoggingService, DbLoggingService>();

            configurationService.AddSingleton<IJsonService, JsonService>();
            configurationService.AddSingleton<IHashingService, HashingService>();

            configurationService.AddScoped<OrionDbContext, OrionDbContext>();
            configurationService.AddScoped<IUserRepository, UserRepository>();
            configurationService.AddScoped<IDirectCommunicationRepository, DirectCommunicationRepository>();
            configurationService.AddScoped<IMessageRepository, MessageRepository>();
            configurationService.AddScoped<IGroupRepository, GroupRepository>();

            configurationService.AddSingleton<ITopicHandlerService>(new TopicHandlerService(configurationService));

            ConfigurationService = configurationService;

            //Console.WriteLine("SERVER");

            //Console.WriteLine("Router IP address: ");
            IPAddress routerIPAddress = IPAddress.Parse("192.168.0.26");

            //Console.WriteLine("Router port: ");
            int routerPort = Convert.ToInt32(50000);

            //Console.WriteLine($"On IP address: {routerIPAddress} and port: {routerPort}");

            var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);
            configurationService.AddSingleton<IConnectionService>(new ConnectionService(routerIPEndPoint));
            configurationService.AddSingleton<ITransmissionService>(new TransmissionService(configurationService.GetSingletonOfType<IJsonService>(), configurationService.GetSingletonOfType<IConnectionService>()));

            configurationService.AddScoped<ServerService, ServerService>();

            var serverService = configurationService.GetInstanceOfType<ServerService>();

            serverService.Run();

            window = new MainWindow();

            RootFrame = window.RootFrame;

            RootFrame.NavigationFailed += OnNavigationFailed;
            RootFrame.Navigate(typeof(LoginPage), args.Arguments);

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
