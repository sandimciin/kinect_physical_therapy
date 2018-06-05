using System.Windows;
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;

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
            
            Application.Current.Properties["beginAtExerciseStart"] = false;  //if true, recording starts at exercise starts. if false, recording starts when window opens
            Application.Current.Properties["durationUnit"] = "seconds";  //unit of duration (seconds or minutes)
            Application.Current.Properties["duration"] = 0.0;  //duration of recording
            Application.Current.Properties["frequency"] = 0.0;  //frequency of recording
            Application.Current.Properties["armReps"] = 10; //arm raises exercise repetitions
            Application.Current.Properties["sittingReps"] = 10;  //sitting and standing exercise repetitions

            _NavigationFrame.Navigate(new Home());
        }
        
    }
}
