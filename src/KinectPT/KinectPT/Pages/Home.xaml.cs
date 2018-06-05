using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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

        //Event handler for clicking Exercises menu option
        private void Click_Exercises(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExerciseOptions());

        }

        //Event handler for clicking Reports menu option
        private void Click_Reports(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportsPage());

        }

        //Event handler for clicking Settings menu option
        private void Click_Data(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DataSettingsPage());

        }
    }
}
