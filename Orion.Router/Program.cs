using Orion.JsonParser;
using Orion.Router;
using Orion.Configuration;
using Orion.Router.Clients;
using Orion.Router.Requests;
using Orion.Router.TopicInterceptors;
using System.Net;
using Orion.Transport.TransmissionServices;
using Orion.Transport.ConnectionServices;

var configurationService = new ConfigurationService();

configurationService.AddSingleton<IJsonService>(new JsonService());

configurationService.AddSingleton<IClientService, ClientService>();
configurationService.AddSingleton<IRequestService>(new RequestService());

configurationService.AddSingleton<ITopicInterceptorService>(new TopicInterceptorService(configurationService));

//Console.WriteLine("Router IP address: ");
IPAddress ipAddress = IPAddress.Parse("192.168.0.26");

//Console.WriteLine("Router port: ");
int port = Convert.ToInt32(50000);

Console.WriteLine($"On IP address: {ipAddress} and port: {port}");

var routerService = new RouterService(new IPEndPoint(ipAddress, port),
    configurationService.GetSingletonOfType<ITopicInterceptorService>(),
    configurationService.GetSingletonOfType<IClientService>(),
    configurationService.GetSingletonOfType<IRequestService>());

Console.WriteLine("Running broker...");
routerService.Run();
Console.ReadLine();