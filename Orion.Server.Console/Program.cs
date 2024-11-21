using Orion.JsonParser;
using Orion.Server;
using Orion.Configuration;
using Orion.Server.DataLayer;
using Orion.Server.Messages.Repositories;
using Orion.Server.TopicHandlers;
using Orion.Server.Users.Repositories;
using System.Net;
using Orion.Server.DirectCommunications.Repositories;
using Orion.Cryptography.HashingServices;
using Orion.Transport.ConnectionServices;

var configurationService = new ConfigurationService();

configurationService.AddSingleton<IConfigurationService>(configurationService);

configurationService.AddSingleton<IJsonService, JsonService>();
configurationService.AddSingleton<IHashingService, HashingService>();


configurationService.AddScoped<OrionDbContext, OrionDbContext>();
configurationService.AddScoped<IUserRepository, UserRepository>();
configurationService.AddScoped<IDirectCommunicationRepository, DirectCommunicationRepository>();
configurationService.AddScoped<IMessageRepository, MessageRepository>();

configurationService.AddSingleton<ITopicHandlerService>(new TopicHandlerService(configurationService));

Console.WriteLine("SERVER");

var hostName = Dns.GetHostName();
Console.WriteLine($"On host: {hostName}");

//Console.WriteLine("Router IP address: ");
IPAddress routerIPAddress = IPAddress.Parse("192.168.0.26");

//Console.WriteLine("Router port: ");
int routerPort = Convert.ToInt32(50000);

Console.WriteLine($"On IP address: {routerIPAddress} and port: {routerPort}");

var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);
configurationService.AddSingleton<IConnectionService>(new ConnectionService(routerIPEndPoint));

configurationService.AddSingleton<ServerService, ServerService>();

var serverService = configurationService.GetInstanceOfType<ServerService>();

serverService.Run();

Console.ReadLine();