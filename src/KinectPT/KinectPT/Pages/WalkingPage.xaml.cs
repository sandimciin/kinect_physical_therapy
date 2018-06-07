using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

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
        Kinect2CSV _recorder = null;
        DateTime startTime = new DateTime();
        TimeSpan duration;
        Stopwatch _timer;
        int current = 0; //1=left, 2=right
        int laps = 0;
        bool dataSaved = false;

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
                

                _recorder = new Kinect2CSV();
                _timer = new Stopwatch();
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

                        viewer.DrawBody(body, 15, Brushes.Red, 8, Brushes.Aqua);

                        _recorder.Update(body);
                        DateTime currentT = DateTime.Now;
                        TimeSpan elapsed = currentT - startTime;

                        //Stop recording if duration is not default (0) and reached duration
                        if (Application.Current.Properties["duration"].ToString() != "0")
                        {
                            if (elapsed >= duration)
                            {
                                _recorder.IsRecording = false;
                            }
                        }

                        //Update instructions when user reaches edge of frame
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
                        if (laps == 3)  //end of exercise. Save node data
                        {
                            Instructions.Text = "You have completed this exercise!";
                            
                            _timer.Stop();

                            if (!dataSaved)
                            {
                                _recorder.Stop();
                                SaveFileDialog dialog = new SaveFileDialog
                                {
                                    Filter = "Excel files|*.csv"
                                };

                                dialog.ShowDialog();

                                if (!string.IsNullOrWhiteSpace(dialog.FileName))
                                {
                                    System.IO.File.Copy(_recorder.Result, dialog.FileName,true);
                                }

                                //Write data for report generation
                                string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\WalkingReportData.csv");
                                // This text is added only once to the file.
                                if (!File.Exists(path))
                                {
                                    // Record duration and date of exercise
                                    using (StreamWriter sw = File.CreateText(path))
                                    {
                                        sw.WriteLine("ExerciseDuration,Date");
                                        sw.WriteLine(_timer.Elapsed.ToString() + "," + DateTime.Today.ToString("d"));
                                    }
                                }
                                else
                                {
                                    // This text is always added, making the file longer over time
                                    // if it is not deleted.
                                    using (StreamWriter sw = File.AppendText(path))
                                    {
                                        sw.WriteLine(_timer.Elapsed.ToString() + "," + DateTime.Today.ToString("d"));
                                    }
                                }
                                dataSaved = true;
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
            _recorder.Start();  //recording for walking exercise always begins at window open
            Instructions.Text = "Make sure your whole body is visible, then turn right and walk";
            _timer.Start();
            startTime = DateTime.Now;
            //Set duration of data recording based on settings
            if (Application.Current.Properties["durationUnit"].ToString() == "seconds")
            {
                float seconds = float.Parse(Application.Current.Properties["duration"].ToString());
                if (seconds == Math.Floor(seconds)) //if no decimals
                {
                    duration = new TimeSpan(0, 0, Convert.ToInt32(seconds));
                }
                else
                {
                    duration = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(1000 * seconds)); //convert to milliseconds
                }

            }
            else  //duration in minutes
            {
                float minutes = float.Parse(Application.Current.Properties["duration"].ToString());
                if (minutes == Math.Floor(minutes))  //if no decimals
                {
                    duration = new TimeSpan(0, Convert.ToInt32(minutes), 0);
                }
                else
                {
                    duration = new TimeSpan(0, 0, Convert.ToInt32(60 * minutes));
                }
            }
        }

        //Event handler for clicking Back button
        private void Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
