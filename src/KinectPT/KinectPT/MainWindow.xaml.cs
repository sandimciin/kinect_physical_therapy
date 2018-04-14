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
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using Microsoft.Kinect.Toolkit;

namespace KinectPT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            KinectRegion.SetKinectRegion(this, kinectRegion);

            App app = ((App)Application.Current);
            app.KinectRegion = kinectRegion;

            // Use the default sensor
            this.kinectRegion.KinectSensor = KinectSensor.GetDefault();

            _NavigationFrame.Navigate(new Home());
        }

        /*private void Click_Exercises(object sender, RoutedEventArgs e)
        {
            //ExerciseOptions exerciseOptionsPage = new ExerciseOptions();
            //NavigationService.Navigate(exerciseOptionsPage);
            //System.Uri uri = new Uri("//Pages/ExerciseOptions.xaml", UriKind.Relative);
            //NavigationService.Navigate(uri);

            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri("/Pages/ExerciseOptions.xaml", UriKind.Relative));
            //nav.Navigate(exerciseOptionsPage);


            //ExerciseOptions exerciseOptionsPage = new ExerciseOptions();
            //this.Content = exerciseOptionsPage;

        }*/
        
    }
}
