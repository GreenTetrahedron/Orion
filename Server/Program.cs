using Orion.JsonParser;
using Orion.Server;
using Orion.Server.Configuration;
using Orion.Server.DataLayer;
using Orion.Server.Messages.Repositories;
using Orion.Server.TopicHandlers;
using Orion.Server.Users.Repositories;
using System.Net;



var configurationService = new ConfigurationService();

configurationService.AddInstanceOfType<IJsonService>(new JsonService());

configurationService.AddInstanceOfType<ITopicHandlerService>(new TopicHandlerService(configurationService));

configurationService.AddInstanceOfType<IDataLayer>(new DataLayer());
configurationService.AddInstanceOfType<IUserRepository>(new UserRepository(configurationService.GetInstanceOfType<IDataLayer>()));
configurationService.AddInstanceOfType<IMessageRepository>(new MessageRepository(configurationService.GetInstanceOfType<IDataLayer>()));

var topicHandlerService = new TopicHandlerService(configurationService);

var handler = topicHandlerService.GetTopicHandler("AuthenticateUser");
var response = await handler.Invoke("User1");

Console.WriteLine(response.OperationResult.OperationMessage.ToString());

Console.WriteLine("SERVER");

var hostName = Dns.GetHostName();
Console.WriteLine($"On host: {hostName}");

Console.WriteLine("Router IP address: ");
IPAddress routerIPAddress = IPAddress.Parse(Console.ReadLine());

Console.WriteLine("Router port: ");
int routerPort = Convert.ToInt32(Console.ReadLine());

var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);

var dataLayer = new DataLayer();
var userRepository = new UserRepository(dataLayer);

var serverService = new ServerService(routerIPEndPoint,
    configurationService.GetInstanceOfType<IJsonService>(),
    configurationService.GetInstanceOfType<ITopicHandlerService>());

serverService.Run();