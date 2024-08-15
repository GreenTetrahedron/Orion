using Orion.Client;
using Orion.Client.TopicHandlers;
using Orion.Configuration;
using Orion.JsonParser;
using System.Net;
using System.Net.Sockets;
using System.Text;

var configurationService = new ConfigurationService();

configurationService.AddInstanceOfType<IJsonService>(new JsonService());

configurationService.AddInstanceOfType<ITopicHandlerService>(new TopicHandlerService(configurationService));

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

var clientService = new ClientService(routerIPEndPoint,
    configurationService.GetInstanceOfType<IJsonService>(),
    configurationService.GetInstanceOfType<ITopicHandlerService>());

clientService.Run();

Console.ReadLine();