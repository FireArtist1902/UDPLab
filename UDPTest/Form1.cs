using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPTest
{
    public partial class Form1 : Form
    {
        Thread thread;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread != null)
            {
                return;
            }

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.IP);
            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[1];
            IPEndPoint point = new IPEndPoint(address, 11000);

            socket.Bind(point);
            thread = new Thread(ReceiveFunc);
            thread.IsBackground = true;
            thread.Start(socket);
            Text = "Working";
        }

        private void ReceiveFunc(object? obj)
        {
            Socket? socket = obj as Socket;
            byte[] buff = new byte[1024];
            EndPoint ep = new IPEndPoint(IPAddress.Any, 11000);
            do
            {
                int len = socket!.ReceiveFrom(buff, ref ep);
                StringBuilder sb = new StringBuilder(textBox1.Text);
                sb.AppendLine($"{len} byte received from {ep}");
                sb.AppendLine(Encoding.Unicode.GetString(buff, 0, len));
                textBox1.BeginInvoke(new Action<string>(AddText), sb.ToString());
            } while (true);
        }

        private void AddText(string obj)
        {
            StringBuilder builder = new StringBuilder(textBox1.Text);
            builder.AppendLine(obj);
            textBox1.Text = builder.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName())[1];
            IPEndPoint point = new IPEndPoint(ip, 11000);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.IP);
            socket.SendTo(Encoding.Unicode.GetBytes(DateTime.Now.ToString()), point);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}