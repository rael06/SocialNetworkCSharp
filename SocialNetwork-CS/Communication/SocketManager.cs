using Common.Communication;
using Newtonsoft.Json;
using SocialNetwork_CS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public dynamic ServerResponse { get; set; }

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

        internal void RequestServer(ClientCommand command)
        {
            var thread = new Thread(() =>
            {
                ServerResponse = Send(command);
            });
            thread.Start();
        }


        private dynamic Send(ClientCommand command)
        {
            var jsonToSend = JsonConvert.SerializeObject(command);
            _socket.Send(Encoding.UTF8.GetBytes(jsonToSend));

            var buffer = new byte[_socket.ReceiveBufferSize];
            int bytesReceived = _socket.Receive(buffer);
            if (bytesReceived > 0)
            {
                var jsonReceived = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                return ResponseTreatment(jsonReceived, command.CommandContent.ToString());
            }
            return null;
        }

        private dynamic ResponseTreatment(string jsonReceived, string commandContent)
        {
            switch (commandContent)
            {
                case "sports":
                    return JsonConvert.DeserializeObject<List<Sport>>(jsonReceived);

                case "members":
                    return JsonConvert.DeserializeObject<List<Member>>(jsonReceived);

                case "clubs":
                    return JsonConvert.DeserializeObject<List<Club>>(jsonReceived);

                default:
                    return null;
            }

        }

        private IPAddress GetHostIPAddress()
        {
            Console.WriteLine(Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork) + "");
            return Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}

