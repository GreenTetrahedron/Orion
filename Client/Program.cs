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


Console.WriteLine("CLIENT");

var hostName = Dns.GetHostName();
Console.WriteLine($"On host: {hostName}");

Console.WriteLine("Router IP address: ");
IPAddress routerIPAddress = IPAddress.Parse(Console.ReadLine());

Console.WriteLine("Router port: ");
int routerPort = Convert.ToInt32(Console.ReadLine());

var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);

var clientService = new ClientService(routerIPEndPoint,
    configurationService.GetInstanceOfType<IJsonService>(),
    configurationService.GetInstanceOfType<ITopicHandlerService>());

clientService.Run();

Console.ReadLine();