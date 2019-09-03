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
using static SocialNetwork_CS.Views.Routes.MainRoutes;

namespace SocialNetwork_CS.Views.Managers
{
    /// <summary>
    /// Interaction logic for Club.xaml
    /// </summary>
    public partial class Club : UserControl
    {
        private SocketManager _socketManager = SocketManager.Instance;
        public event EventHandler<PageType> PageChanged;
        public Club()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PageChanged?.Invoke(this, PageType.Menu);
        }
    }
}
