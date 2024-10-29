using Orion.JsonParser;
using Orion.Router;
using Orion.Configuration;
using Orion.Router.Connections;
using Orion.Router.Requests;
using Orion.Router.TopicInterceptors;
using System.Net;

var configurationService = new ConfigurationService();

configurationService.AddSingleton<IJsonService>(new JsonService());

configurationService.AddSingleton<IConnectionService>(new ConnectionService());
configurationService.AddSingleton<IRequestService>(new RequestService());

configurationService.AddSingleton<ITopicInterceptorService>(new TopicInterceptorService(configurationService));

//Console.WriteLine("Router IP address: ");
IPAddress ipAddress = IPAddress.Parse("192.168.0.26");

//Console.WriteLine("Router port: ");
int port = Convert.ToInt32(50000);

Console.WriteLine($"On IP address: {ipAddress} and port: {port}");

var routerService = new RouterService(new IPEndPoint(ipAddress, port),
    configurationService.GetSingletonOfType<ITopicInterceptorService>(),
    configurationService.GetSingletonOfType<IConnectionService>(),
    configurationService.GetSingletonOfType<IRequestService>(),
    configurationService.GetSingletonOfType<IJsonService>());

Console.WriteLine("Running broker...");
routerService.Run();
Console.ReadLine();