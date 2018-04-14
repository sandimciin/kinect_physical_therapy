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
    /// Interaction logic for ExerciseOptions.xaml
    /// </summary>
    public partial class ExerciseOptions : Page
    {
        public ExerciseOptions()
        {
            InitializeComponent();
        }

        private void Click_ArmRaises(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ArmRaisesPage());


        }

        private void Click_Walking(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WalkingPage());
        }

        private void Click_SittingStanding(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SittingStandingPage());
        }

        private void Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
