using Orion.Client;
using Orion.Client.Connections;
using Orion.Client.TopicHandlers;
using Orion.Configuration;
using Orion.JsonParser;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello World!");

//Console.ReadLine();

//Console.WriteLine("CLIENT");

//var hostName = Dns.GetHostName();
//Console.WriteLine($"On host: {hostName}");

////Console.WriteLine("Router IP address: ");
//IPAddress routerIPAddress = IPAddress.Parse("192.168.0.26");

////Console.WriteLine("Router port: ");
//int routerPort = Convert.ToInt32(50000);

//Console.WriteLine($"On IP address: {routerIPAddress} and port: {routerPort}");

//var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);


//var configurationService = new ConfigurationService();

//configurationService.AddInstanceOfType<IJsonService>(new JsonService());

//configurationService.AddInstanceOfType<ISubscriptableService>(new SubscriptableService());
//configurationService.AddInstanceOfType<ITopicHandlerService>(new TopicHandlerService(configurationService));

//var connectionService = new ConnectionService
//    (
//        routerIPEndPoint,
//        configurationService.GetInstanceOfType<IJsonService>(),
//        configurationService.GetInstanceOfType<ITopicHandlerService>()
//    );

//configurationService.AddInstanceOfType<IConnectionService>(connectionService);

//configurationService.AddInstanceOfType<ITransmissionService>(new TransmissionService(
//        configurationService.GetInstanceOfType<IConnectionService>(),
//        configurationService.GetInstanceOfType<IJsonService>(),
//        configurationService.GetInstanceOfType<ISubscriptableService>()
//    ));

//configurationService.AddInstanceOfType<IUserService>(new UserService(
//        configurationService.GetInstanceOfType<ISubscriptableService>(),
//        configurationService.GetInstanceOfType<ITransmissionService>()
//    ));


//connectionService.Run();

//var application = new Application(configurationService.GetInstanceOfType<IUserService>());
//application.Run();
//Console.WriteLine("HELLO");

//Console.ReadLine();