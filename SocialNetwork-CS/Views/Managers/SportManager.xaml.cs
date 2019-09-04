using Common.Communication;
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
	/// Interaction logic for SportManager.xaml
	/// </summary>
	public partial class SportManager : Page, INotifyPropertyChanged
	{
		private SocketManager _socketManager = SocketManager.Instance;
		public ObservableCollection<Sport> Data { get; set; } = new ObservableCollection<Sport>();

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
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_sport)));
				}
			}
		}
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		public SportManager()
		{
			InitializeComponent();
		}

		public SportManager(ObservableCollection<Sport> sports) : this()
		{
			DataContext = this;
			foreach (Sport s in sports) Data.Add(s);
		}

		private void ListView_ItemSelection(object sender, SelectionChangedEventArgs e)
		{
			var item = (sender as ListView).SelectedItem as Sport;
			Sport = item;
			Debug.WriteLine(Sport.Name);
		}

		private void CreateItem(object sender, RoutedEventArgs e)
		{

		}
	}
}
