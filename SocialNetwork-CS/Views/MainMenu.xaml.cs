using Common.Communication;
using SocialNetwork_CS.Communication;
using SocialNetwork_CS.Models;
using SocialNetwork_CS.Views.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			var request = new Request { RequestType = "read", RequestTarget = "clubs" };
			_socketManager.RequestServer(request);
		}

		private void Member_Manager_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new MemberManager());
			var request = new Request { RequestType = "read", RequestTarget = "members" };
			_socketManager.RequestServer(request);
		}

		private void Sport_Manager_Click(object sender, RoutedEventArgs e)
		{
			var request = new Request { RequestType = "read", RequestTarget = "sports" };
			_socketManager.RequestServer(request);
			_sportManager = new SportManager();
			NavigationService.Navigate(_sportManager);
		}
	}
}
