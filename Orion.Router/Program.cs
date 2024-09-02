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

//Console.WriteLine("Router IP address: ");
IPAddress ipAddress = IPAddress.Parse("192.168.0.26");

//Console.WriteLine("Router port: ");
int port = Convert.ToInt32(50000);

Console.WriteLine($"On IP address: {ipAddress} and port: {port}");

var routerService = new RouterService(new IPEndPoint(ipAddress, port),
    configurationService.GetInstanceOfType<ITopicInterceptorService>(),
    configurationService.GetInstanceOfType<IConnectionService>(),
    configurationService.GetInstanceOfType<IRequestService>(),
    configurationService.GetInstanceOfType<IJsonService>());

Console.WriteLine("Running broker...");
routerService.Run();