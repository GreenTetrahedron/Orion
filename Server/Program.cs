using Orion.JsonParser;
using Orion.Server;
using Orion.Configuration;
using Orion.Server.DataLayer;
using Orion.Server.Messages.Repositories;
using Orion.Server.TopicHandlers;
using Orion.Server.Users.Repositories;
using System.Net;



var configurationService = new ConfigurationService();

configurationService.AddInstanceOfType<IJsonService>(new JsonService());

configurationService.AddInstanceOfType<IDataLayer>(new DataLayer());
configurationService.AddInstanceOfType<IUserRepository>(new UserRepository(configurationService.GetInstanceOfType<IDataLayer>()));
configurationService.AddInstanceOfType<IMessageRepository>(new MessageRepository(configurationService.GetInstanceOfType<IDataLayer>()));

configurationService.AddInstanceOfType<ITopicHandlerService>(new TopicHandlerService(configurationService));

Console.ReadLine();
Console.WriteLine("SERVER");

var hostName = Dns.GetHostName();
Console.WriteLine($"On host: {hostName}");

//Console.WriteLine("Router IP address: ");
IPAddress routerIPAddress = IPAddress.Parse("192.168.0.26");

//Console.WriteLine("Router port: ");
int routerPort = Convert.ToInt32(50000);

Console.WriteLine($"On IP address: {routerIPAddress} and port: {routerPort}");

var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);

var serverService = new ServerService(routerIPEndPoint,
    configurationService.GetInstanceOfType<IJsonService>(),
    configurationService.GetInstanceOfType<ITopicHandlerService>());

serverService.Run();

Console.ReadLine();