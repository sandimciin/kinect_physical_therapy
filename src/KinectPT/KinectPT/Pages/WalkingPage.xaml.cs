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
using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using LightBuzz.Kinect2CSV;
using Microsoft.Win32;

namespace KinectPT
{
    /// <summary>
    /// Interaction logic for WalkingPage.xaml
    /// </summary>
    public partial class WalkingPage : Page
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _userReporter;
        KinectCSVManager _recorder = null;

        int current = 0; //1=left, 2=right
        int laps = 0;

        public WalkingPage()
        {
            InitializeComponent();

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                _userReporter = new PlayersController();
                _userReporter.BodyEntered += UserReporter_BodyEntered;
                _userReporter.BodyLeft += UserReporter_BodyLeft;
                _userReporter.Start();
                

                _recorder = new KinectCSVManager();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_userReporter != null)
            {
                _userReporter.Stop();
            }

            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }



        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Color)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var bodies = frame.Bodies();

                    _userReporter.Update(bodies);

                    Body body = bodies.Closest();



                    if (body != null)
                    {
                        FrameEdges clippedEdges = body.ClippedEdges;

                        viewer.DrawBody(body, 15, Brushes.Red, 8, Brushes.Red);

                        _recorder.Update(body);

                        if (clippedEdges.HasFlag(FrameEdges.Left))
                        {
                            Instructions.Text = "Turn right and walk";
                            if (current == 2)
                            {
                                laps++;
                            }
                            current = 1;
                        }
                        if (clippedEdges.HasFlag(FrameEdges.Right))
                        {
                            Instructions.Text = "Turn left and walk";
                            if (current == 1)
                            {
                                laps++;
                            }
                            current = 2;
                        }
                        if (laps == 3)
                        {
                            Instructions.Text = "You have completed this exercise!";
                            if (_recorder.IsRecording)
                            {
                                _recorder.Stop();


                                SaveFileDialog dialog = new SaveFileDialog
                                {
                                    Filter = "Excel files|*.csv"
                                };

                                dialog.ShowDialog();

                                if (!string.IsNullOrWhiteSpace(dialog.FileName))
                                {
                                    System.IO.File.Copy(_recorder.Result, dialog.FileName);
                                }
                            }
                        }
                    }
                }
            }
        }

        void UserReporter_BodyEntered(object sender, UsersControllerEventArgs e)
        {
        }

        void UserReporter_BodyLeft(object sender, UsersControllerEventArgs e)
        {
            viewer.Clear();
        }

        void Gesture_GestureRecognized(object sender, EventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _recorder.Start();
            Instructions.Text = "Make sure your whole body is visible, then turn right and walk";
        }
    }
}
