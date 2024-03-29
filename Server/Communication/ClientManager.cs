﻿using Common.Communication;
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
							return new Request
							{
								RequestTarget = "members",
								RequestContent = Context.Members.OrderBy(x => x.LastName).ToList(),
								RequestSuccess = true
							};

						case "clubs":
							return new Request
							{
								RequestTarget = "clubs",
								RequestContent = Context.Clubs.OrderBy(x => x.Name).ToList(),
								RequestSuccess = true
							};

						case "sports":
							return new Request
							{
								RequestTarget = "sports",
								RequestContent = Context.Sports.OrderBy(x => x.Name).ToList(),
								RequestSuccess = true
							};

						default:
							return null;
					}

				case "create":
					switch (request.RequestTarget)
					{
						case "sport":
							var sportFromClient = JsonConvert.DeserializeObject<Sport>(request.RequestContent.ToString());
							Context.Sports.Add(sportFromClient);
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "sports",
								RequestContent = Context.Sports.OrderBy(x => x.Name).ToList(),
								RequestSuccess = true
							};

						case "club":
							var clubFromClient = JsonConvert.DeserializeObject<Club>(request.RequestContent.ToString()) as Club;
							var clubSportToCreate = Context.Sports.Find(clubFromClient.Sport.Id);
							clubFromClient.Sport = clubSportToCreate;
							Console.WriteLine(clubFromClient.Sport.Id);
							Context.Clubs.Add(clubFromClient);
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "clubs",
								RequestContent = Context.Clubs.OrderBy(x => x.Name).ToList(),
								RequestSuccess = true
							};

						case "member":
							var memberFromClient = JsonConvert.DeserializeObject<Member>(request.RequestContent.ToString()) as Member;

							var memberSportsToUpdate = new List<Sport>();
							var memberClubsToUpdate = new List<Club>();

							foreach (Sport sport in memberFromClient.Sports)
							{
								memberSportsToUpdate.Add(Context.Sports.Find(sport.Id) as Sport);
							}
							memberFromClient.Sports = memberSportsToUpdate;

							foreach (Club club in memberFromClient.Clubs)
							{
								memberClubsToUpdate.Add(Context.Clubs.Find(club.Id) as Club);
							}
							memberFromClient.Clubs = memberClubsToUpdate;

							Context.Members.Add(memberFromClient);
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "members",
								RequestContent = Context.Members.OrderBy(x => x.LastName).ToList(),
								RequestSuccess = true
							};

						default:
							return null;
					}

				case "update":
					switch (request.RequestTarget)
					{
						case "sport":
							var sportFromClient = JsonConvert.DeserializeObject<Sport>(request.RequestContent.ToString());
							var sportToUpdate = Context.Sports.Find(sportFromClient.Id) as Sport;
							sportToUpdate.Name = sportFromClient.Name;
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "sports",
								RequestContent = Context.Sports.OrderBy(x => x.Name).ToList(),
								RequestSuccess = true
							};

						case "club":
							var clubFromClient = JsonConvert.DeserializeObject<Club>(request.RequestContent.ToString()) as Club;
							var clubToUpdate = Context.Clubs.Find(clubFromClient.Id) as Club;
							var clubSportToUpdate = Context.Sports.Find(clubFromClient.Sport.Id) as Sport;
							Console.WriteLine(clubSportToUpdate.Id + " " + clubFromClient.Sport.Id);
							clubToUpdate.Name = clubFromClient.Name;
							clubToUpdate.Sport = clubSportToUpdate;
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "clubs",
								RequestContent = Context.Clubs.OrderBy(x => x.Name).ToList(),
								RequestSuccess = true
							};

						case "member":
							var memberFromClient = JsonConvert.DeserializeObject<Member>(request.RequestContent.ToString()) as Member;
							var memberToUpdate = Context.Members.Find(memberFromClient.Id) as Member;
							var memberSportsToUpdate = new List<Sport>();
							var memberClubsToUpdate = new List<Club>();
							foreach(Sport sport in memberFromClient.Sports)
							{
								memberSportsToUpdate.Add(Context.Sports.Find(sport.Id) as Sport);
							}
							memberToUpdate.Sports = memberSportsToUpdate;

							foreach (Club club in memberFromClient.Clubs)
							{
								memberClubsToUpdate.Add(Context.Clubs.Find(club.Id) as Club);
							}
							memberToUpdate.Clubs = memberClubsToUpdate;

							memberToUpdate.LastName = memberFromClient.LastName;
							memberToUpdate.FirstName = memberFromClient.FirstName;
							memberToUpdate.Age = memberFromClient.Age;
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "members",
								RequestContent = Context.Members.OrderBy(x => x.LastName).ToList(),
								RequestSuccess = true
							};

						default:
							return null;
					}

				case "delete":
					switch (request.RequestTarget)
					{
						case "sport":
							var sportFromClient = JsonConvert.DeserializeObject<Sport>(request.RequestContent.ToString());
							var sportToDelete = Context.Sports.Find(sportFromClient.Id);
							Context.Sports.Remove(sportToDelete);
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "sports",
								RequestContent = Context.Sports.OrderBy(x => x.Name).ToList(),
								RequestSuccess = true
							};

						case "club":
							var clubFromClient = JsonConvert.DeserializeObject<Club>(request.RequestContent.ToString());
							var clubToDelete = Context.Clubs.Find(clubFromClient.Id);
							Context.Clubs.Remove(clubToDelete);
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "clubs",
								RequestContent = Context.Clubs.OrderBy(x => x.Name).ToList(),
								RequestSuccess = true
							};

						case "member":
							var memberFromClient = JsonConvert.DeserializeObject<Member>(request.RequestContent.ToString());
							var memberToDelete = Context.Members.Find(memberFromClient.Id);
							Context.Members.Remove(memberToDelete);
							Context.SaveChanges();
							return new Request
							{
								RequestTarget = "members",
								RequestContent = Context.Members.OrderBy(x => x.LastName).ToList(),
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
