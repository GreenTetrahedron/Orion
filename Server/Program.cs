using Server.DataLayer;
using Server.Users;
using Server.Users.Repositories;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("SERVER");

var hostName = Dns.GetHostName();
Console.WriteLine($"On host: {hostName}");

Console.WriteLine("Broker IP address: ");
IPAddress brokerIPAddress = IPAddress.Parse(Console.ReadLine());

Console.WriteLine("Broker port: ");
int brokerPort = Convert.ToInt32(Console.ReadLine());

var brokerIPEndPoint = new IPEndPoint(brokerIPAddress, brokerPort);

var dataLayer = new DataLayer();
var userRepository = new UserRepository(dataLayer);

using (Socket server = new Socket(brokerIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
{
    await server.ConnectAsync(brokerIPEndPoint);

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
            User user = userRepository.AddUser(splitMessage[1]);
            Console.WriteLine($"New user: {user.Username}");

            var header = $"{user.Username}Connection";

            var sendingMessage = $"{header} {user.UserId}";
            var sendingBytes = Encoding.UTF8.GetBytes(sendingMessage);

            await server.SendAsync(sendingBytes, SocketFlags.None);
            Console.WriteLine("New user information sent...");
        }
    }
}