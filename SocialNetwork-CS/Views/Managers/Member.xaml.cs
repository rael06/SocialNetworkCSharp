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
    /// Interaction logic for Member.xaml
    /// </summary>
    public partial class Member : UserControl
    {
        private SocketManager _socketManager = SocketManager.Instance;
        public event EventHandler<PageType> PageChanged;
       
        public Member()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PageChanged?.Invoke(this, PageType.Menu);
        }
    }
}
