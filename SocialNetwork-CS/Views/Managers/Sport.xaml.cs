using Common.Communication;
using SocialNetwork_CS.Communication;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Sport.xaml
    /// </summary>
    public partial class Sport : UserControl
    {
        public List<Sport> Sports { get; set; }
        private SocketManager _socketManager = SocketManager.Instance;
        public event EventHandler<PageType> PageChanged;

        public Sport()
        {
            InitializeComponent();
        }

        private void SetSports()
        {
            Sports = _socketManager.ServerResponse is List<Sport> ? _socketManager.ServerResponse as List<Sport> : new List<Sport>();
            Debug.WriteLine("test");
            foreach (Sport sport in Sports) Debug.WriteLine(sport.Name);
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PageChanged?.Invoke(this, PageType.Menu);
            SetSports();
        }
    }
}
