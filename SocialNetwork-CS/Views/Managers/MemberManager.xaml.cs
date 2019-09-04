using SocialNetwork_CS.Communication;
using System;
using System.Collections.Generic;
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

namespace SocialNetwork_CS.Views.Managers
{
	/// <summary>
	/// Interaction logic for MemberManager.xaml
	/// </summary>
	public partial class MemberManager : Page
	{
		private SocketManager _socketManager = SocketManager.Instance;

		public MemberManager()
		{
			InitializeComponent();
		}

		private void LoadData_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
