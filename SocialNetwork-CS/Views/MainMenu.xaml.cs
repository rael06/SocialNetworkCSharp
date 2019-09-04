using Common.Communication;
using SocialNetwork_CS.Communication;
using SocialNetwork_CS.Models;
using SocialNetwork_CS.Views.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocialNetwork_CS.Views
{

	/// <summary>
	/// Interaction logic for MainMenu.xaml
	/// </summary>
	public partial class MainMenu : Page
	{
		private SocketManager _socketManager = SocketManager.Instance;
		private SportManager _sportManager;
		public MainMenu()
		{
			InitializeComponent();
		}

		private void Club_Manager_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new ClubManager());
			var command = new ClientCommand { CommandType = "read", CommandContent = "clubs" };
			_socketManager.RequestServer(command);
		}

		private void Member_Manager_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new MemberManager());
			var command = new ClientCommand { CommandType = "read", CommandContent = "members" };
			_socketManager.RequestServer(command);
		}

		private void Sport_Manager_Click(object sender, RoutedEventArgs e)
		{
			var command = new ClientCommand { CommandType = "read", CommandContent = "sports" };
			_socketManager.RequestServer(command);
			while (_socketManager.ServerResponse == null) Debug.WriteLine("loading");
			_sportManager = new SportManager(_socketManager.ServerResponse);
			NavigationService.Navigate(_sportManager);
		}
	}
}
