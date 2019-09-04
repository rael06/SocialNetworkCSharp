using Common.Communication;
using Newtonsoft.Json;
using Server.Models;
using Server.Models.Entities;
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
        public SocialNetworkCSContext Context = new SocialNetworkCSContext();
        public ClientManager(Socket socket)
        {
            _clientSocket = socket;
            Thread listenThread = new Thread(Listen);
            listenThread.Start();
        }

        private void Send(Socket socket, string message)
        {
            socket.Send(Encoding.UTF8.GetBytes(message));
        }

        private void Listen()
        {
            while (true)
            {
                byte[] buffer = new byte[_clientSocket.ReceiveBufferSize];
                int bytesReceived = _clientSocket.Receive(buffer);
                if (bytesReceived > 0)
                {
                    var jsonStr = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    var request = JsonConvert.DeserializeObject<Request>(jsonStr);
                    Console.WriteLine($"La requête du client est de type : {request.RequestType} " +
                        $"et contient {request.RequestContent}");

                    var response = RequestTreatment(request);
                    Send(_clientSocket, JsonConvert.SerializeObject(response));
                }
            }
        }

        private object RequestTreatment(Request request)
        {
            switch (request.RequestType)
            {
                case "read":
                    switch (request.RequestContent)
                    {
                        case "members":
                            return null;

                        case "clubs":
                            return null;

                        case "sports":                            
                            return Context.Sports.ToList();

                        default:
                            return null;
                    }

                default:
                    return null;

            }
        }
    }
}
