using Newtonsoft.Json;
using SocialNetwork_CS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork_CS.Communication
{
    internal class SocketManager
    {
        private const int PORT = 10000;
        private Socket _socket;

        private static SocketManager _socketManager;

        private SocketManager()
        {
                
        }

        public static SocketManager Instance =>
            _socketManager ?? (_socketManager = new SocketManager());

        internal void Launch()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = new IPEndPoint(GetHostIPAddress(), PORT);
            _socket.Connect(ip);
        }

        internal void RequestServer()
        {
            var thread = new Thread(() =>
            {
                Send();
            });
            thread.Start();
        }


        private void Send()
        {
            var obj = new TestObj { LastName = "calitro", FirstName = "rael" };
            var jsonObject = JsonConvert.SerializeObject(obj);
            _socket.Send(Encoding.UTF8.GetBytes(jsonObject));

            //_socket.Send(Encoding.UTF8.GetBytes(message));

            var buffer = new byte[_socket.ReceiveBufferSize];
            int bytesReceived = _socket.Receive(buffer);
            if (bytesReceived > 0)
            {
                var jsonObj = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                var objReceived = JsonConvert.DeserializeObject<TestObj>(jsonObj);
                Console.WriteLine($"{objReceived.FirstName} {objReceived.LastName}");

            }
        }

        private IPAddress GetHostIPAddress()
        {
            Console.WriteLine(Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork) + "");
            return Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}

