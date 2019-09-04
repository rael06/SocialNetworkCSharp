using Common.Communication;
using SocialNetwork_CS.Communication;
using SocialNetwork_CS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static SocialNetwork_CS.Views.Routes.MainRoutes;

namespace SocialNetwork_CS.Views.Managers
{
    /// <summary>
    /// Interaction logic for SportManager.xaml
    /// </summary>
    public partial class SportManager : UserControl
    {
        private SocketManager _socketManager = SocketManager.Instance;

        public event EventHandler<PageType> PageChanged;
        public List<Sport> Sports { get; set; }
        public SportManager()
        {
            InitializeComponent();
            MouseLeftButtonDown += new MouseButtonEventHandler(ListView_MouseLeftButtonDown);
        }

        private void ListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItems;
            Debug.WriteLine(item.GetType().Name);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PageChanged?.Invoke(this, PageType.Menu);
        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            Sports = _socketManager.ServerResponse;
            foreach (Sport sport in Sports) Debug.WriteLine(sport.Name);
        }
    }
}
