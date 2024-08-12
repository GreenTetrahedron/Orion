using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("CLIENT");

var hostName = Dns.GetHostName();
Console.WriteLine($"On host: {hostName}");

Console.WriteLine("Router IP address: ");
IPAddress routerIPAddress = IPAddress.Parse(Console.ReadLine());

Console.WriteLine("Router port: ");
int routerPort = Convert.ToInt32(Console.ReadLine());

var routerIPEndPoint = new IPEndPoint(routerIPAddress, routerPort);

using (Socket client = new Socket(routerIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
{
    await client.ConnectAsync(routerIPEndPoint);

    await client.SendAsync(Encoding.UTF8.GetBytes("CLIENT"), SocketFlags.None);

    Console.WriteLine("Client connected");
    Console.WriteLine("Username: ");

    var username = Console.ReadLine();

    var usernameBytes = Encoding.UTF8.GetBytes(username);

    await client.SendAsync(usernameBytes);
}

Console.ReadLine();