using System.Net.Sockets;
using System.Net;
using System.Text;




while(true)
{
    IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName())[1];
    IPEndPoint point = new IPEndPoint(ip, 11000);
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
        ProtocolType.IP);
    socket.SendTo(Encoding.Unicode.GetBytes(DateTime.Now.ToString()), point);
    socket.Shutdown(SocketShutdown.Both);
    socket.Close();
}
