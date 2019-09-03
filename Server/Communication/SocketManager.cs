using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Communication
{
    internal class SocketManager
    {
        private const int PORT = 10000;
        private List<ClientManager> Clients { get; set; } = new List<ClientManager>();
        private Socket _socket;
        internal SocketManager()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ip = new IPEndPoint(GetHostIPAddress(), PORT);
            _socket.Bind(ip);

            _socket.Listen(0);
            while (true)
            {
                var clientSocket = _socket.Accept();
                var client = new ClientManager(clientSocket);
                Clients.Add(client);
            }
        }

        private static IPAddress GetHostIPAddress()
        {
            Console.WriteLine(Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork) + "");
            return Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}


