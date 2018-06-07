using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using LightBuzz.Vitruvius;
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
        RightArmRaise _gesture;  //right arm raise
        LeftArmRaise _gesture2;  //left arm raise
        Kinect2CSV _recorder = null;
        Stopwatch _timer;
        DateTime startTime = new DateTime();
        TimeSpan duration;

        bool onRightArmRaise = true;
        int rcounter;
        int lcounter;
        int startreps;
        bool dataSaved = false;

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

            //Set exercise repetitions based on settings or default (10)
            startreps = Int32.Parse(Application.Current.Properties["armReps"].ToString());
            rcounter = startreps;
            lcounter = startreps;

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
                        viewer.DrawBody(body, 15, Brushes.White, 8, Brushes.Aqua);
                        // Draw the Arm Angles
                        rightArmAngle.Update(body.Joints[_startRight], body.Joints[_centerRight], body.Joints[_endRight], 100);
                        leftArmAngle.Update(body.Joints[_startLeft], body.Joints[_centerLeft], body.Joints[_endLeft], 100);



                        if (rcounter < startreps && onRightArmRaise)
                        {
                            //If data recording set to begin at exercise start, start recording after first rep
                            if (rcounter == startreps - 1 && Application.Current.Properties["beginAtExerciseStart"].ToString() == "True" && !dataSaved && !_recorder.IsRecording)
                            {
                                _recorder.Start();
                            }

                        }

                        _gesture.Update(body);
                        _gesture2.Update(body);

                        if (_recorder.IsRecording)
                        {
                            _recorder.Update(body);

                            DateTime current = DateTime.Now;
                            TimeSpan elapsed = current - startTime;

                            //Stop recording if duration is not default (0) and reached duration
                            if (Application.Current.Properties["duration"].ToString() != "0")
                            {
                                if (elapsed >= duration)
                                {
                                    _recorder.IsRecording = false;
                                }
                            }
                        }


                        if (onRightArmRaise)
                        {
                            Instructions.Text = "Raise your right arm above your head";
                            //Display remaining repititions
                            ArmRaiseCount.Text = rcounter.ToString();
                            //Display arm angle value
                            tblAngle.Text = ((int)rightArmAngle.Angle).ToString();
                        }
                        else
                        {
                            //Update instructions
                            Instructions.Text = "Raise your left arm above your head";
                            //Display arm angle value
                            pageTitle.Text = "Left Arm Angle:";
                            tblAngle.Text = ((int)leftArmAngle.Angle).ToString();
                            //Display remaining repetitions
                            ArmRaiseCount.Text = lcounter.ToString();
                        }
                        if (lcounter == 0)  //end of exercise. Save node data
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
                                string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\ArmRaisesReportData.csv");
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

                                // This text is always added, making the file longer over time
                                // if it is not deleted.
                                else
                                {
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
            leftArmAngle.Clear();
            rightArmAngle.Clear();

            tblAngle.Text = "-";
        }

        void Gesture_GestureRecognized(object sender, EventArgs e)
        {
            Instructions.Text = "Lower your right arm";
            rcounter--;
            if (rcounter == 0)
            {
                onRightArmRaise = false;
            }
        }

        void Gesture2_GestureRecognized(object sender, EventArgs e)
        {
            Instructions.Text = "Lower your left arm";
            lcounter--;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Begin node data recording based on settings
            if (Application.Current.Properties["beginAtExerciseStart"].ToString() == "False")
            {
                _recorder.Start();
            }
            _timer.Start();

            startTime = DateTime.Now;
            //Set duration of data recording based on settings
            if (Application.Current.Properties["durationUnit"].ToString() == "seconds")
            {
                float seconds = float.Parse(Application.Current.Properties["duration"].ToString());
                if(seconds == Math.Floor(seconds)) //if no decimals
                {
                    duration = new TimeSpan(0, 0, Convert.ToInt32(seconds));
                }
                else
                {
                    duration = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(1000 * seconds));  //convert to milliseconds
                }
                
            }
            else  //duration in minutes
            {
                float minutes = float.Parse(Application.Current.Properties["duration"].ToString());
                if(minutes == Math.Floor(minutes))  //if no decimals
                {
                    duration = new TimeSpan(0, Convert.ToInt32(minutes), 0);
                }
                else
                {
                    duration = new TimeSpan(0, 0, Convert.ToInt32(60 *minutes));
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
