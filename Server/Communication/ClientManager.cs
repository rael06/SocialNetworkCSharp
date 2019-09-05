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
					var jsonRequest = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
					var request = JsonConvert.DeserializeObject<Request>(jsonRequest);

					var response = RequestTreatment(request);
					var jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented,
						new JsonSerializerSettings()
						{
							ReferenceLoopHandling = ReferenceLoopHandling.Ignore
						}
					);
					Send(_clientSocket, jsonResponse);
				}
			}
		}

		private Request RequestTreatment(Request request)
		{
			switch (request.RequestType)
			{
				case "read":
					switch (request.RequestTarget)
					{
						case "members":
							return null;

						case "clubs":
							var clubs = new List<Club>();
							var contextClubs = Context.Clubs.ToList();
							foreach (Club c in contextClubs) clubs.Add(new Club
							{
								Name = c.Name,
								SportId = c.SportId,
								Sport = c.Sport
							});

							return new Request
							{
								RequestTarget = "clubs",
								RequestContent = clubs,
								RequestSuccess = true
							};

						case "sports":
							return new Request
							{
								RequestTarget = "sports",
								RequestContent = Context.Sports.ToList(),
								RequestSuccess = true
							};

						default:
							return null;
					}

				case "update":
					switch (request.RequestTarget)
					{
						case "sport":
							var sport = JsonConvert.DeserializeObject<Sport>(request.RequestContent.ToString());
							var sportToUpdate = Context.Sports.Find(sport.Id);
							sportToUpdate.Name = sport.Name;
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "sports",
								RequestContent = Context.Sports.ToList(),
								RequestSuccess = true
							};

						default:
							return null;
					}

				case "delete":
					switch (request.RequestTarget)
					{
						case "sport":
							var sport = JsonConvert.DeserializeObject<Sport>(request.RequestContent.ToString());
							var sportToDelete = Context.Sports.Find(sport.Id);
							Context.Sports.Remove(sportToDelete);
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "sports",
								RequestContent = Context.Sports.ToList(),
								RequestSuccess = true
							};

						default:
							return null;
					}

				case "create":
					switch (request.RequestTarget)
					{
						case "sport":
							var sport = JsonConvert.DeserializeObject<Sport>(request.RequestContent.ToString());
							Context.Sports.Add(sport);
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "sports",
								RequestContent = Context.Sports.ToList(),
								RequestSuccess = true
							};

						default:
							return null;
					}


				default:
					return null;

			}
		}
	}
}
