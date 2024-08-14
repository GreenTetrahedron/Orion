using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Attributes;
using Orion.Server.Configuration;
using Orion.Server.DataLayer;
using Orion.Server.Messages.Repositories;
using Orion.Server.TopicHandlers;
using Orion.Server.Users;
using Orion.Server.Users.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;



var configurationService = new ConfigurationService();

configurationService.AddInstanceOfType<IDataLayer>(new DataLayer());
configurationService.AddInstanceOfType<IUserRepository>(new UserRepository(configurationService.GetInstanceOfType<IDataLayer>()));
configurationService.AddInstanceOfType<IMessageRepository>(new MessageRepository(configurationService.GetInstanceOfType<IDataLayer>()));

var topicHandlerService = new TopicHandlerService(configurationService);

var handler = topicHandlerService.GetTopicHandler("AuthenticateUser");
var response = await handler.Invoke("User1");

Console.WriteLine(((Task<ServerResult>)response).Result.OperationResult.OperationMessage.ToString());

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

using (Socket server = new Socket(routerIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
{
    await server.ConnectAsync(routerIPEndPoint);

    Console.WriteLine("Server connected");

    await server.SendAsync(Encoding.UTF8.GetBytes("SERVER"), SocketFlags.None);

    while (true)
    {
        var buffer = new byte[1024];
        int receivedBytes = await server.ReceiveAsync(buffer, SocketFlags.None);

        string message = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
        Console.WriteLine($"Transmission received: {message}");
        string[] splitMessage = message.Split(": ");
        
        if (splitMessage[0] == "NEW CLIENT")
        {
            User user = await userRepository.AddUser(splitMessage[1]);
            Console.WriteLine($"New user: {user.Username}");

            var header = $"{user.Username}Connection";

            var sendingMessage = $"{header} {user.UserId}";
            var sendingBytes = Encoding.UTF8.GetBytes(sendingMessage);

            await server.SendAsync(sendingBytes, SocketFlags.None);
            Console.WriteLine("New user information sent...");
        }
    }
}