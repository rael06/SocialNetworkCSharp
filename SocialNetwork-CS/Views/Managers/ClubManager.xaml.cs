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
	public partial class ClubManager : Page, INotifyPropertyChanged
	{
		private SocketManager _socketManager = SocketManager.Instance;

		private MainWindow _mainWindow = MainWindow._mainWindow;

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

		#region AllSports
		private ObservableCollection<Sport> _allSports = new ObservableCollection<Sport>();
		public ObservableCollection<Sport> AllSports
		{
			get { return _allSports; }
			set
			{
				if (_allSports != value)
				{
					_allSports = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllSports)));
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

		#region Sport
		private Sport _sport = new Sport();
		public Sport Sport
		{
			get { return _sport; }
			set
			{
				if (_sport != value)
				{
					_sport = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sport)));
				}
			}
		}
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		public ClubManager()
		{
			InitializeComponent();
			DataContext = this;
			_mainWindow.Navigating += _mainWindow_Navigating;
			_socketManager.RequestCompleted += SetData;

			_socketManager.RequestServer(new Request
			{
				RequestType = "read",
				RequestTarget = "clubs"
			});

			_socketManager.RequestServer(new Request
			{
				RequestType = "read",
				RequestTarget = "sports"
			});
		}

		private void _mainWindow_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			if (e.NavigationMode == NavigationMode.Back) _socketManager.RequestCompleted -= SetData;
		}

		private void SetData(object sender, PropertyChangedEventArgs e)
		{
			if (_socketManager.ServerResponse.RequestTarget == "clubs")
			{
				Data = JsonConvert.DeserializeObject<ObservableCollection<Club>>(
					_socketManager.ServerResponse.RequestContent.ToString());
			}

			if (_socketManager.ServerResponse.RequestTarget == "sports")
			{
				AllSports = JsonConvert.DeserializeObject<ObservableCollection<Sport>>(
					_socketManager.ServerResponse.RequestContent.ToString());
			}
		}

		private void ListView_ItemSelection(object sender, SelectionChangedEventArgs e)
		{
			var item = (sender as ListView).SelectedItem as Club;
			if (item != null) Club = item;
		}

		private void ComboBox_ItemSelection(object sender, SelectionChangedEventArgs e)
		{
			var item = (sender as ComboBox).SelectedItem as Sport;
			if (item != null) Sport = item;
		}

		private void CreateItem_Click(object sender, RoutedEventArgs e)
		{
			if (Club.Id == 0 && Club.Name != null && Sport.Id != 0)
			{
				_socketManager.RequestServer(new Request
				{
					RequestType = "create",
					RequestTarget = "club",
					RequestContent = new Club {
						Name = Club.Name,
						Sport = Sport
					}
				});
				ClearFields();
			}
		}

		private void UpdateItem_Click(object sender, RoutedEventArgs e)
		{
			if (Club.Id != 0 && Sport.Id != 0)
			{
				Club.Sport = Sport;
				_socketManager.RequestServer(new Request
				{
					RequestType = "update",
					RequestTarget = "club",
					RequestContent = Club
				});
				ClearFields();
			}
		}

		private void DeleteItem_Click(object sender, RoutedEventArgs e)
		{
			if (Club.Id != 0)
			{
				_socketManager.RequestServer(new Request
				{
					RequestType = "delete",
					RequestTarget = "club",
					RequestContent = Club
				});
				ClearFields();
			}
		}

		private void ClearFields()
		{
			Club = new Club();
		}
	}
}
