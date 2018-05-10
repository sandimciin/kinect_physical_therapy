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
using Microsoft.Kinect;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace KinectPT
{
    /// <summary>
    /// Interaction logic for ArmRaisesPage.xaml
    /// </summary>
    public partial class ArmRaisesPage : Page
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _userReporter;
        RightArmRaise _gesture;
        LeftArmRaise _gesture2;
        //KinectCSVManager _recorder = null;
        Kinect2CSV _recorder = null;
        Stopwatch _timer;
        DateTime startTime = new DateTime();
        TimeSpan duration;

        bool onRightArmRaise = true;
        int rcounter = 10;
        int lcounter = 10;
        double userHeight = 0;
        int maxRightArmAngle = 0;
        int maxLeftArmAngle = 0;
        string sessionData = "";
        bool recording = false;

        /* Angle Calculation for the right arm */
        JointType _startRight = JointType.ShoulderRight;
        JointType _centerRight = JointType.ElbowRight;
        JointType _endRight = JointType.WristRight;

        /* Angle Calculation for the left arm */
        JointType _startLeft = JointType.ShoulderLeft;
        JointType _centerLeft = JointType.ElbowLeft;
        JointType _endLeft = JointType.WristLeft;


        public ArmRaisesPage()
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

                _gesture = new RightArmRaise();
                _gesture.GestureRecognized += Gesture_GestureRecognized;

                _gesture2 = new LeftArmRaise();
                _gesture2.GestureRecognized += Gesture2_GestureRecognized;

                //_recorder = new KinectCSVManager();
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
                        // Gather the uesr's height at the beginning of the exercise
                        userHeight = body.Height();
                        // Draw the Arm Angles
                        rightArmAngle.Update(body.Joints[_startRight], body.Joints[_centerRight], body.Joints[_endRight], 100);
                        leftArmAngle.Update(body.Joints[_startLeft], body.Joints[_centerLeft], body.Joints[_endLeft], 100);


                        // Update the corresponding armAngle value to the screen
                        if (onRightArmRaise)
                        {
                            tblAngle.Text = ((int)rightArmAngle.Angle).ToString();
                        }
                        else
                        {
                            tblAngle.Text = ((int)leftArmAngle.Angle).ToString();
                        }



                        if (rcounter < 10 && onRightArmRaise)
                        {
                            if(rcounter==9 && Application.Current.Properties["beginAtExerciseStart"].ToString() == "True")
                            {
                                _recorder.Start();
                            }

                            if (maxRightArmAngle < (int)rightArmAngle.Angle)
                            {
                                maxRightArmAngle = (int)rightArmAngle.Angle;
                                // sessionData += "Max Angle on Right Arm Raise: " + maxRightArmAngle.ToString() + Environment.NewLine;
                                // sessionData += "User Height: " + body.Height().ToString() + Environment.NewLine;
                            }
                        }
                        else if (lcounter < 10 && !onRightArmRaise)
                        {
                            if (maxLeftArmAngle < (int)leftArmAngle.Angle)
                            {
                                maxLeftArmAngle = (int)leftArmAngle.Angle;
                                // sessionData += "Max Angle on Left Arm Raise: " + maxLeftArmAngle.ToString() + Environment.NewLine;
                            }
                        }

                        _gesture.Update(body);
                        _gesture2.Update(body);

                        if (_recorder.IsRecording)
                        {
                            _recorder.Update(body);

                            DateTime current = DateTime.Now;
                            TimeSpan elapsed = current - startTime;

                            if (Convert.ToInt32(Application.Current.Properties["duration"].ToString()) > 0)
                            {
                                if (elapsed >= duration)
                                {
                                    //_recorder.Stop();
                                    _recorder.IsRecording = false;
                                    recording = false;
                                }
                            }
                        }


                        if (onRightArmRaise)
                        {
                            Instructions.Text = "Raise your right arm above your head";
                            ArmRaiseCount.Text = rcounter.ToString();
                        }
                        else
                        {
                            Instructions.Text = "Raise your left arm above your head";
                            pageTitle.Text = "Left Arm Angle:";
                            ArmRaiseCount.Text = lcounter.ToString();
                        }
                        if (lcounter == 0)
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

                                    // Report file uses the path used for writing to the CSV location and replaces .csv with .txt
                                    string sessionPath = dialog.FileName.Replace(".csv", ".txt");

                                    // Write all report data at once in this order:
                                    sessionData += "Date of Exercise: " + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + Environment.NewLine;
                                    sessionData += "Total Time: " + _timer.Elapsed.ToString() + Environment.NewLine;
                                    sessionData += "Height of User: " + userHeight.ToString() + Environment.NewLine;
                                    sessionData += "Max Angle on Right Arm Raise: " + maxRightArmAngle.ToString() + Environment.NewLine;
                                    sessionData += "Max Angle on Left Arm Raise: " + maxLeftArmAngle.ToString() + Environment.NewLine;
                                    File.WriteAllText(sessionPath, sessionData);
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
            leftArmAngle.Clear();
            rightArmAngle.Clear();

            tblAngle.Text = "-";
        }

        void Gesture_GestureRecognized(object sender, EventArgs e)
        {
            //tblGesture.Text = "RightArmRaise Detected!";
            Instructions.Text = "Lower your right arm";
            rcounter--;
            if (rcounter == 0)
            {
                onRightArmRaise = false;
            }
        }

        void Gesture2_GestureRecognized(object sender, EventArgs e)
        {
            //tblGesture.Text = "LeftArmRaise Detected!";
            Instructions.Text = "Lower your left arm";
            lcounter--;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["beginAtExerciseStart"].ToString() == "False")
            {
                _recorder.Start();
            }
            _timer.Start();
            recording = true;

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
