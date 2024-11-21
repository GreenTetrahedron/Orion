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
using Orion.Server.DataLayer;
using Orion.Server.DirectCommunications.Repositories;
using Orion.Server.Messages.Repositories;
using Orion.Server.TopicHandlers;
using Orion.Server.Users.Repositories;
using Orion.Transport.ConnectionServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

            configurationService.AddSingleton<IJsonService, JsonService>();
            configurationService.AddSingleton<IHashingService, HashingService>();


            configurationService.AddScoped<OrionDbContext, OrionDbContext>();
            configurationService.AddScoped<IUserRepository, UserRepository>();
            configurationService.AddScoped<IDirectCommunicationRepository, DirectCommunicationRepository>();
            configurationService.AddScoped<IMessageRepository, MessageRepository>();

            configurationService.AddSingleton<ITopicHandlerService>(new TopicHandlerService(configurationService));

            //Console.WriteLine("SERVER");

            //Console.WriteLine("Router IP address: ");
            IPAddress routerIPAddress = IPAddress.Parse("192.168.0.26");

            //Console.WriteLine("Router port: ");
            int routerPort = Convert.ToInt32(50000);

            //Console.WriteLine($"On IP address: {routerIPAddress} and port: {routerPort}");

            var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);
            configurationService.AddSingleton<IConnectionService>(new ConnectionService(routerIPEndPoint));

            configurationService.AddSingleton<ServerService, ServerService>();

            var serverService = configurationService.GetInstanceOfType<ServerService>();

            serverService.Run();

            Console.ReadLine();


            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
    }
}
