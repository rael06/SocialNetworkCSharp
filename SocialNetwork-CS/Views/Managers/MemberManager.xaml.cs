using Common.Communication;
using Newtonsoft.Json;
using SocialNetwork_CS.Communication;
using SocialNetwork_CS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
	public partial class MemberManager : Page, INotifyPropertyChanged
	{
		private SocketManager _socketManager = SocketManager.Instance;

		private MainWindow _mainWindow = MainWindow._mainWindow;

		#region Data
		private ObservableCollection<Member> _data = new ObservableCollection<Member>();
		public ObservableCollection<Member> Data
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

		#region AllClubs
		private ObservableCollection<Club> _allClubs = new ObservableCollection<Club>();
		public ObservableCollection<Club> AllClubs
		{
			get { return _allClubs; }
			set
			{
				if (_allClubs != value)
				{
					_allClubs = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllClubs)));
				}
			}
		}
		#endregion

		#region Member
		private Member _member = new Member();
		public Member Member
		{
			get { return _member; }
			set
			{
				if (_member != value)
				{
					_member = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
				}
			}
		}
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		public MemberManager()
		{
			InitializeComponent();
			DataContext = this;
			_mainWindow.Navigating += _mainWindow_Navigating;
			_socketManager.RequestCompleted += SetData;

			_socketManager.RequestServer(new Request
			{
				RequestType = "read",
				RequestTarget = "members"
			});

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
			if (_socketManager.ServerResponse.RequestTarget == "members")
			{
				Data = JsonConvert.DeserializeObject<ObservableCollection<Member>>(
					_socketManager.ServerResponse.RequestContent.ToString());
			}

			if (_socketManager.ServerResponse.RequestTarget == "clubs")
			{
				AllClubs = JsonConvert.DeserializeObject<ObservableCollection<Club>>(
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
			var item = (sender as ListView).SelectedItem as Member;
			if (item != null) Member = item;
		}

		private void CreateItem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void UpdateItem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DeleteItem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ClearFields()
		{
			Member = new Member();
		}
	}
}
