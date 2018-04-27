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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }
        private void Click_Exercises(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExerciseOptions());

        }

        private void Click_Reports(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportsPage());

        }

        private void Click_Data(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DataSettingsPage());

        }
    }
}
