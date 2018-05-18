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
using LightBuzz.Kinect2CSV;
using Microsoft.Win32;
using Microsoft.Kinect;
using System.IO;
using System.Diagnostics;

namespace KinectPT
{
    /// <summary>
    /// Interaction logic for SittingStandingPage.xaml
    /// </summary>
    public partial class SittingStandingPage : Page
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _userReporter;
        Sitting _gesture;
        //KinectCSVManager _recorder = null;
        Kinect2CSV _recorder = null;
        DateTime startTime = new DateTime();
        TimeSpan duration;
        Stopwatch _timer;

        bool begin = false;
        int sittingCounter = 10;
        int current = 1; //1=standing, 2=sitting

        JointType _start = JointType.ShoulderRight;
        JointType _center = JointType.ElbowRight;
        JointType _end = JointType.WristRight;

        public SittingStandingPage()
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

                _gesture = new Sitting();
                _gesture.GestureRecognized += Gesture_GestureRecognized;

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

            /*
            if (_sensor != null)
            {
                _sensor.Close();
            }
            */
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
                        viewer.DrawBody(body, 15, Brushes.White, 8, Brushes.Aqua);
                        //angle.Update(body.Joints[_start], body.Joints[_center], body.Joints[_end], 100);

                        //  tblAngle.Text = ((int)angle.Angle).ToString();
                        _gesture.Update(body);

                        if (_recorder.IsRecording)
                        {
                            _recorder.Update(body);
                            DateTime currentT = DateTime.Now;
                            TimeSpan elapsed = currentT - startTime;

                            if (Convert.ToInt32(Application.Current.Properties["duration"].ToString()) > 0)
                            {
                                if (elapsed >= duration)
                                {
                                    _recorder.IsRecording = false;
                                }
                            }
                        }

                        SittingCount.Text = sittingCounter.ToString();

                        Instructions.Text = "Raise your right arm above your head to begin this exercise";
                        if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.Head].Position.Y)
                        {
                            begin = true;
                            if (Application.Current.Properties["beginAtExerciseStart"].ToString() == "True")
                            {
                                _recorder.Start();
                            }
                        }

                        if (begin)
                        {
                            // knee height close to hip height
                            if (body.Joints[JointType.HipLeft].Position.Y <= body.Joints[JointType.KneeLeft].Position.Y + .2)
                            {
                                //tblGesture.Text = "Sitting";
                                Instructions.Text = "Stand up with your arms forward";
                                if (current == 1)
                                {
                                    sittingCounter--;
                                }
                                current = 2;
                            }
                            else
                            {
                                //tblGesture.Text = "Standing";
                                Instructions.Text = "Sit down with your arms forward";

                                current = 1;

                            }

                            if (sittingCounter == 0)
                            {
                                Instructions.Text = "You have completed this exercise!";
                                
                                    _recorder.Stop();
                                    _timer.Stop();

                                    SaveFileDialog dialog = new SaveFileDialog
                                    {
                                        Filter = "Excel files|*.csv"
                                    };

                                    dialog.ShowDialog();

                                    if (!string.IsNullOrWhiteSpace(dialog.FileName))
                                    {
                                        System.IO.File.Copy(_recorder.Result, dialog.FileName);
                                    }

                                    Directory.Delete(_recorder.Folder, true);


                                    string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\SittingReportData.csv");
                                    // This text is added only once to the file.
                                    if (!File.Exists(path))
                                    {
                                        // Create a file to write to.
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
            // angle.Clear();

            //tblAngle.Text = "-";
        }

        void Gesture_GestureRecognized(object sender, EventArgs e)
        {
            //tblGesture.Text = "Sitting Gesture Detected!";

        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["beginAtExerciseStart"].ToString() == "False")
            {
                _recorder.Start();
            }
            _timer.Start();
            Instructions.Text = "Make sure your entire body is in frame";
            startTime = DateTime.Now;
            if (Application.Current.Properties["durationUnit"].ToString() == "seconds")
            {
                duration = new TimeSpan(0, 0, Convert.ToInt32(Application.Current.Properties["duration"].ToString()));
            }
        }

        private void Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
