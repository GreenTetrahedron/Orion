using Broker;
using System.Net;

Console.WriteLine("IP address: ");
IPAddress ipAddress = IPAddress.Parse(Console.ReadLine());

Console.WriteLine("Port: ");
int port = Convert.ToInt32(Console.ReadLine());


var routerService = new RouterService(new IPEndPoint(ipAddress, port));

Console.WriteLine("Running broker...");
routerService.Run();