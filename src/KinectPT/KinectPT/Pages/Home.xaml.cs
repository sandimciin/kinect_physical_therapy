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
            //ExerciseOptions exerciseOptionsPage = new ExerciseOptions();
            //NavigationService.Navigate(exerciseOptionsPage);
            //System.Uri uri = new Uri("//Pages/ExerciseOptions.xaml", UriKind.Relative);
            //NavigationService.Navigate(uri);

            //NavigationService nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(new Uri("/Pages/ExerciseOptions.xaml", UriKind.Relative));
            //nav.Navigate(exerciseOptionsPage);


            //ExerciseOptions exerciseOptionsPage = new ExerciseOptions();
            //this.Content = exerciseOptionsPage;

            NavigationService.Navigate(new ExerciseOptions());

        }
    }
}
