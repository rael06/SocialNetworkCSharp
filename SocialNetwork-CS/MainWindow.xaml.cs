using SocialNetwork_CS.Communication;
using SocialNetwork_CS.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace SocialNetwork_CS
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : NavigationWindow
	{
		private SocketManager _socketManager = SocketManager.Instance;
		public static MainWindow _mainWindow;

		public MainWindow()
		{
			InitializeComponent();
			_mainWindow = this;
			_socketManager.Launch();
			Navigating += NavigationService_Navigating;
		}

		private void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			if (e.NavigationMode == NavigationMode.Back)
			{
				Debug.WriteLine(e.Uri.ToString());
			}
		}
	}
}
