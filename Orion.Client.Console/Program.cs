using Orion.Client;
using Orion.Client.Connections;
using Orion.Client.DirectCommunications.Services;
using Orion.Client.Subscriptions.Services;
using Orion.Client.TopicHandlers;
using Orion.Client.Transmissions;
using Orion.Client.Users.Services;
using Orion.Configuration;
using Orion.JsonParser;
using System.Net;

Console.Write("Client ready...");
Console.ReadLine();

Console.WriteLine("CLIENT");

var hostName = Dns.GetHostName();
Console.WriteLine($"On host: {hostName}");

//Console.WriteLine("Router IP address: ");
IPAddress routerIPAddress = IPAddress.Parse("192.168.0.26");

//Console.WriteLine("Router port: ");
int routerPort = Convert.ToInt32(50000);

Console.WriteLine($"On IP address: {routerIPAddress} and port: {routerPort}");

var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);


var configurationService = new ConfigurationService();

configurationService.AddSingleton<IJsonService>(new JsonService());

configurationService.AddSingleton<ISubscriptionService>(new SubscriptionService());
configurationService.AddSingleton<ITopicHandlerService>(new TopicHandlerService(configurationService));

var connectionService = new ConnectionService
    (
        routerIPEndPoint
    );

configurationService.AddSingleton<IConnectionService>(connectionService);

configurationService.AddSingleton<ITransmissionService>(new TransmissionService(
        configurationService.GetSingletonOfType<IConnectionService>(),
        configurationService.GetSingletonOfType<IJsonService>(),
        configurationService.GetSingletonOfType<ISubscriptionService>()
    ));

configurationService.AddSingleton<IUserService>(new UserService(
        configurationService.GetSingletonOfType<ITransmissionService>()
    ));

configurationService.AddSingleton<IDirectCommunicationService>(new DirectCommunicationService(
        configurationService.GetSingletonOfType<ITransmissionService>()
    ));

var application = new TestApplication(
    configurationService.GetSingletonOfType<IUserService>(),
    configurationService.GetSingletonOfType<IDirectCommunicationService>(),
    configurationService.GetSingletonOfType<ITransmissionService>()
    );

application.Run();

Console.ReadLine();