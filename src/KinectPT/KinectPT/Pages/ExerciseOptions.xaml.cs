using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KinectPT
{
    /// <summary>
    /// Interaction logic for ExerciseOptions.xaml
    /// </summary>
    public partial class ExerciseOptions : Page
    {
        public ExerciseOptions()
        {
            InitializeComponent();
        }

        //Event handler for clicking Arm Raises menu option
        private void Click_ArmRaises(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ArmRaisesPage());
        }

        //Event handler for clicking Walking menu option
        private void Click_Walking(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WalkingPage());
        }

        //Event handler for clicking Sitting and Standing menu option
        private void Click_SittingStanding(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SittingStandingPage());
        }

        //Event handler for clicking Back button
        private void Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
