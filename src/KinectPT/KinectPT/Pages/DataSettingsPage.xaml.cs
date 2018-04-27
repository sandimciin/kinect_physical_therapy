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

namespace KinectPT
{
    /// <summary>
    /// Interaction logic for DataSettingsPage.xaml
    /// </summary>
    public partial class DataSettingsPage : Page
    {
        public DataSettingsPage()
        {
            InitializeComponent();
        }

        private void Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
