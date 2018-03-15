using LightBuzz.Vitruvius;
using LightBuzz.Kinect2CSV;
using Microsoft.Win32;
using Microsoft.Kinect;
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

namespace Microsoft.Samples.Kinect.ControlsBasics
{
    /// <summary>
    /// Interaction logic for AnglePage.xaml
    /// </summary>
    public partial class AnglePage : UserControl
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _userReporter;
        RightArmRaise _gesture;
        LeftArmRaise _gesture2;
        KinectCSVManager _recorder = null;

        bool onRightArmRaise = true;
        int rcounter=10;
        int lcounter = 10;

        JointType _start = JointType.ShoulderRight;
        JointType _center = JointType.ElbowRight;
        JointType _end = JointType.WristRight;

        public AnglePage()
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
                        viewer.DrawBody(body, 15,Brushes.White ,8, Brushes.Red);
                        angle.Update(body.Joints[_start], body.Joints[_center], body.Joints[_end], 100);

                        tblAngle.Text = ((int)angle.Angle).ToString();
                        _gesture.Update(body);
                        _gesture2.Update(body);

                        _recorder.Update(body);


                        if (onRightArmRaise)
                        {
                            Instructions.Text = "Raise your right arm above your head";
                            ArmRaiseCount.Text = rcounter.ToString();
                        }
                        else
                        {
                            Instructions.Text = "Raise your left arm above your head";
                            ArmRaiseCount.Text = lcounter.ToString();
                        }
                        if (lcounter == 0)
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
            angle.Clear();

            tblAngle.Text = "-";
        }

        void Gesture_GestureRecognized(object sender, EventArgs e)
        {
            tblGesture.Text = "RightArmRaise Detected!";
            Instructions.Text = "Lower your right arm";
            rcounter--;
            if (rcounter == 0)
            {
                onRightArmRaise = false;
            }
        }

        void Gesture2_GestureRecognized(object sender, EventArgs e)
        {
            tblGesture.Text = "LeftArmRaise Detected!";
            Instructions.Text = "Lower your left arm";
            lcounter--;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _recorder.Start();
        }
    }
}

