using Common.Communication;
using Newtonsoft.Json;
using SocialNetwork_CS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

		#region ServerResponse
		private Request _serverResponse = new Request();
		public Request ServerResponse
		{
			get { return _serverResponse; }
			set
			{
				if (_serverResponse != value)
				{
					_serverResponse = value;
					RequestCompleted?.Invoke(this, new PropertyChangedEventArgs(nameof(ServerResponse)));
				}
			}
		}
		#endregion

		private static SocketManager _socketManager;

		public event PropertyChangedEventHandler RequestCompleted;

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

		internal void RequestServer(Request request)
		{
			Send(request);
			//var thread = new Thread(() =>
			//{
			//	Send(request);
			//});
			//thread.Start();
		}


		private void Send(Request request)
		{
			var jsonRequest = JsonConvert.SerializeObject(request);
			_socket.Send(Encoding.UTF8.GetBytes(jsonRequest));

			var buffer = new byte[_socket.ReceiveBufferSize];
			int bytesReceived = _socket.Receive(buffer);
			if (bytesReceived > 0)
			{
				var jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
				ServerResponse = JsonConvert.DeserializeObject<Request>(jsonResponse);
				//Debug.WriteLine(ServerResponse.ToString());
			}
		}

		private IPAddress GetHostIPAddress()
		{
			Console.WriteLine(Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork) + "");
			return Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork);
		}
	}
}

