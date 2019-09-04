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

		#region Data
		private ObservableCollection<Sport> _data = new ObservableCollection<Sport>();
		public ObservableCollection<Sport> Data
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
		public SportManager()
		{
			InitializeComponent();
			DataContext = this;
			_socketManager.RequestCompleted += SetData;
		}

		private void SetData(object sender, PropertyChangedEventArgs e)
		{
			Data = _socketManager.ServerResponse as ObservableCollection<Sport>;
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
