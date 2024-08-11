using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("CLIENT");

var hostName = Dns.GetHostName();
Console.WriteLine($"On host: {hostName}");

Console.WriteLine("Broker IP address: ");
IPAddress brokerIPAddress = IPAddress.Parse(Console.ReadLine());

Console.WriteLine("Broker port: ");
int brokerPort = Convert.ToInt32(Console.ReadLine());

var brokerIPEndPoint = new IPEndPoint(brokerIPAddress, brokerPort);

using (Socket client = new Socket(brokerIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
{
    await client.ConnectAsync(brokerIPEndPoint);

    await client.SendAsync(Encoding.UTF8.GetBytes("CLIENT"), SocketFlags.None);

    Console.WriteLine("Client connected");
    Console.WriteLine("Username: ");

    var username = Console.ReadLine();

    var usernameBytes = Encoding.UTF8.GetBytes(username);

    await client.SendAsync(usernameBytes);
}

Console.ReadLine();