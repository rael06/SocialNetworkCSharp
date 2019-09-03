using Newtonsoft.Json;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Communication
{
    internal class ClientManager
    {
        private static Socket _clientSocket { get; set; }
        public ClientManager(Socket socket)
        {
            _clientSocket = socket;
            Thread listenThread = new Thread(Listen);
            listenThread.Start();
        }

        private static void Send(Socket socket, string message)
        {
            socket.Send(Encoding.UTF8.GetBytes(message));
        }

        private static void Listen()
        {
            while (true)
            {
                byte[] buffer = new byte[_clientSocket.ReceiveBufferSize];
                int bytesReceived = _clientSocket.Receive(buffer);
                if (bytesReceived > 0)
                {
                    var jsonStr = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    var obj = JsonConvert.DeserializeObject<TestObj>(jsonStr);
                    Console.WriteLine($"{obj.Name} {obj.FirstName}");

                    //var message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    //Console.WriteLine(message);

                    //string receivedMessage = $"{message} reçu";
                    Send(_clientSocket, JsonConvert.SerializeObject(obj));
                }
            }
        }
    }
}
