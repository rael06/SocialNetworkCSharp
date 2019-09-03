using Newtonsoft.Json;
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
        private static Socket _socket;
        private static int _port = 10000;
        internal void Launch()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = new IPEndPoint(GetHostIPAddress(), _port);
            _socket.Connect(ip);

            var thread = new Thread(() =>
            {
                Send();
            });
            thread.Start();
        }

        private static void Send()
        {
            var obj = new TestObject { Name = "calitro", FirstName = "rael" };
            var jsonObject = JsonConvert.SerializeObject(obj);
            _socket.Send(Encoding.UTF8.GetBytes(jsonObject));

            //_socket.Send(Encoding.UTF8.GetBytes(message));

            var buffer = new byte[_socket.ReceiveBufferSize];
            int bytesReceived = _socket.Receive(buffer);
            if (bytesReceived > 0)
            {
                var jsonObj = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                var objReceived = JsonConvert.DeserializeObject<TestObject>(jsonObj);
                Console.WriteLine($"{objReceived.FirstName} {objReceived.Name}");

            }
        }

        private static IPAddress GetHostIPAddress()
        {
            Console.WriteLine(Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork) + "");
            return Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork);
        }
    }

    internal class TestObject
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
    }
}

