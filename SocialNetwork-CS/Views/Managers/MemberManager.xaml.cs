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

		#region Clubs
		private ObservableCollection<Club> _clubs = new ObservableCollection<Club>();
		public ObservableCollection<Club> Clubs
		{
			get { return _clubs; }
			set
			{
				if (_clubs != value)
				{
					_clubs = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Clubs)));
				}
			}
		}
		#endregion

		#region Sports
		private ObservableCollection<Sport> _sports = new ObservableCollection<Sport>();
		public ObservableCollection<Sport> Sports
		{
			get { return _sports; }
			set
			{
				if (_sports != value)
				{
					_sports = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sports)));
				}
			}
		}
		#endregion

		#region MemberClubs
		private ObservableCollection<Club> _memberClubs = new ObservableCollection<Club>();
		public ObservableCollection<Club> MemberClubs
		{
			get { return _memberClubs; }
			set
			{
				if (_memberClubs != value)
				{
					_memberClubs = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberClubs)));
				}
			}
		}
		#endregion

		#region MemberSports
		private ObservableCollection<Sport> _memberSports = new ObservableCollection<Sport>();
		public ObservableCollection<Sport> MemberSports
		{
			get { return _memberSports; }
			set
			{
				if (_memberSports != value)
				{
					_memberSports = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberSports)));
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

				Clubs.Clear();
				foreach (Club c in AllClubs) Clubs.Add(c);
			}

			if (_socketManager.ServerResponse.RequestTarget == "sports")
			{
				AllSports = JsonConvert.DeserializeObject<ObservableCollection<Sport>>(
					_socketManager.ServerResponse.RequestContent.ToString());

				Sports.Clear();
				foreach (Sport s in AllSports) Sports.Add(s);
			}
		}

		private void ListView_ItemSelection(object sender, SelectionChangedEventArgs e)
		{
			var item = (sender as ListView).SelectedItem as Member;
			if (item != null)
			{
				Member = item as Member;
				MemberClubs = new ObservableCollection<Club>();
				MemberSports = new ObservableCollection<Sport>();
				Clubs.Clear();
				Sports.Clear();
				foreach (Club c in AllClubs) Clubs.Add(c);
				foreach (Sport s in AllSports) Sports.Add(s);


				foreach (Club club in Member.Clubs) MemberClubs.Add(club);
				foreach (Sport sport in Member.Sports) MemberSports.Add(sport);

				foreach (Club club in MemberClubs)
				{
					var filteredClub = AllClubs.FirstOrDefault(c => c.Id == club.Id);
					Clubs.Remove(filteredClub);
				}

				foreach (Sport sport in MemberSports)
				{
					var filteredSport = AllSports.FirstOrDefault(s => s.Id == sport.Id);
					Sports.Remove(filteredSport);
				}
			}
		}

		private void ListView_ClubInClubsSelection(object sender, SelectionChangedEventArgs e)
		{
			var item = (sender as ListView).SelectedItem as Club;
			if (item != null)
			{
				Clubs.Remove(item);
				MemberClubs.Add(item);
			}
		}

		private void ListView_SportInSportsSelection(object sender, SelectionChangedEventArgs e)
		{
			var item = (sender as ListView).SelectedItem as Sport;
			if (item != null)
			{
				Sports.Remove(item);
				MemberSports.Add(item);
				Clubs.Clear();
				foreach (Sport s in MemberSports)
				{
					foreach(Club c in s.Clubs)
					{
						Clubs.Add(c);
					}
				}
			}
		}

		private void ListView_MemberClubsSelection(object sender, SelectionChangedEventArgs e)
		{
			var item = (sender as ListView).SelectedItem as Club;
			if (item != null)
			{
				Clubs.Add(item);
				MemberClubs.Remove(item);
			}
		}

		private void ListView_MemberSportsSelection(object sender, SelectionChangedEventArgs e)
		{
			var item = (sender as ListView).SelectedItem as Sport;
			if (item != null)
			{
				Sports.Add(item);
				MemberSports.Remove(item);
				Clubs.Clear();
				foreach (Sport memberSport in MemberSports)
				{
					foreach (Club clubMemberSport in memberSport.Clubs)
					{
						Clubs.Add(clubMemberSport);
					}

					//foreach (Club memberClub in MemberClubs)
					//{
					//	if (!memberSport.Clubs.Contains(memberClub)) MemberClubs.Remove(memberClub);
					//}
				}
			}
		}

		private void CreateItem_Click(object sender, RoutedEventArgs e)
		{
			if (MemberClubs.Count != 0 &&
				MemberSports.Count != 0 &&
				Member.Id == 0 &&
				Member.LastName != null &&
				Member.FirstName != null &&
				Member.Age != 0)
			{
				Member.Sports = MemberSports;
				Member.Clubs = MemberClubs;

				_socketManager.RequestServer(new Request
				{
					RequestType = "create",
					RequestTarget = "member",
					RequestContent = Member
				});
				ClearFields();
			}
		}

		private void UpdateItem_Click(object sender, RoutedEventArgs e)
		{
			if (MemberClubs.Count != 0 && 
				MemberSports.Count != 0 && 
				Member.Id != 0 &&
				Member.LastName != null &&
				Member.FirstName != null &&
				Member.Age != 0)
			{
				Member.Sports = MemberSports;
				Member.Clubs = MemberClubs;

				_socketManager.RequestServer(new Request
				{
					RequestType = "update",
					RequestTarget = "member",
					RequestContent = Member
				});
				ClearFields();
			}
		}

		private void DeleteItem_Click(object sender, RoutedEventArgs e)
		{
			if (Member.Id != 0)
			{
				_socketManager.RequestServer(new Request
				{
					RequestType = "delete",
					RequestTarget = "member",
					RequestContent = Member
				});
				ClearFields();
			}
		}

		private void ClearFields()
		{
			Member = new Member();
			MemberClubs.Clear();
			MemberSports.Clear();
		}

		private void Unselect_Click(object sender, RoutedEventArgs e)
		{
			ClearFields();
			Clubs.Clear();
			Sports.Clear();
			foreach (Club c in AllClubs) Clubs.Add(c);
			foreach (Sport s in AllSports) Sports.Add(s);
		}
	}
}
