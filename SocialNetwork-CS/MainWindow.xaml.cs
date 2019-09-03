using SocialNetwork_CS.Communication;
using SocialNetwork_CS.Views;
using System;
using System.Collections.Generic;
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
using static SocialNetwork_CS.Views.Routes.MainRoutes;

namespace SocialNetwork_CS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private SocketManager _socketManager = SocketManager.Instance;

        private PageType _pageType;

        public event PropertyChangedEventHandler PropertyChanged;

        public PageType PageType
        {
            get { return _pageType; }
            set
            {
                if (_pageType != value)
                {
                    _pageType = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageType)));
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _socketManager.Launch();
        }

        private void MainMenu_PageChanged(object sender, PageType e)
        {
            PageType = e;
        }
    }
}
