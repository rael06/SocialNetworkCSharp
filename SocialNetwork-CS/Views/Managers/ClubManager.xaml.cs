using Common.Communication;
using Newtonsoft.Json;
using SocialNetwork_CS.Communication;
using SocialNetwork_CS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SocialNetwork_CS.Views.Managers
{
	/// <summary>
	/// Interaction logic for ClubManager.xaml
	/// </summary>
	public partial class ClubManager : Page
	{
		private SocketManager _socketManager = SocketManager.Instance;

		#region Data
		private ObservableCollection<Club> _data = new ObservableCollection<Club>();
		public ObservableCollection<Club> Data
		{
			get { return _data; }
			set
			{
				if (_data != value)
				{
					_data = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
				}
			}
		}
		#endregion

		#region Club
		private Club _club = new Club();
		public Club Club
		{
			get { return _club; }
			set
			{
				if (_club != value)
				{
					_club = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Club)));
				}
			}
		}
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		public ClubManager()
		{
			InitializeComponent();
			DataContext = this;

			_socketManager.RequestServer(new Request
			{
				RequestType = "read",
				RequestTarget = "clubs"
			});

			_socketManager.RequestCompleted += SetData;
		}

		private void SetData(object sender, PropertyChangedEventArgs e)
		{
			Data = JsonConvert.DeserializeObject<ObservableCollection<Club>>(_socketManager.ServerResponse.RequestContent.ToString());
			foreach (Club c in Data) Debug.WriteLine(c.Sport.Name);
		}

		private void UpdateItem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ListView_ItemSelection(object sender, SelectionChangedEventArgs e)
		{

		}

		private void DeleteItem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void CreateItem_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
