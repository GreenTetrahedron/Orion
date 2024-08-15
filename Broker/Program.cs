using Orion.JsonParser;
using Orion.Router;
using Orion.Configuration;
using Orion.Router.Connections;
using Orion.Router.Requests;
using Orion.Router.TopicInterceptors;
using System.Net;

var configurationService = new ConfigurationService();

configurationService.AddInstanceOfType<IJsonService>(new JsonService());

configurationService.AddInstanceOfType<IConnectionService>(new ConnectionService());
configurationService.AddInstanceOfType<IRequestService>(new RequestService());

configurationService.AddInstanceOfType<ITopicInterceptorService>(new TopicInterceptorService(configurationService));

Console.WriteLine("IP address: ");
IPAddress ipAddress = IPAddress.Parse(Console.ReadLine());

Console.WriteLine("Port: ");
int port = Convert.ToInt32(Console.ReadLine());


var routerService = new RouterService(new IPEndPoint(ipAddress, port),
    configurationService.GetInstanceOfType<ITopicInterceptorService>(),
    configurationService.GetInstanceOfType<IConnectionService>(),
    configurationService.GetInstanceOfType<IRequestService>(),
    configurationService.GetInstanceOfType<IJsonService>());

Console.WriteLine("Running broker...");
routerService.Run();