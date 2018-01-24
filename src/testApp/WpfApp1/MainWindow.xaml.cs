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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor sensor;
        ColorFrameReader colorFrameReader;
        BodyFrameReader bodyFrameReader;
        WriteableBitmap colorBitmap;
        Body[] bodies;
        // misc stuff
        double birdHeight;
        double prevRightHandHeight;
        double prevLeftHandHeight;
        double pipeX;
        double pipeGapY;
        double pipeGapLength;
        Random randomGenerator;
        DrawingGroup drawingGroup;



        public MainWindow()
        {

            // Get the sensor
            sensor = KinectSensor.GetDefault();
            sensor.Open();

            // Setup readers for each source of data we want to use
            colorFrameReader = sensor.ColorFrameSource.OpenReader();
            bodyFrameReader = sensor.BodyFrameSource.OpenReader();

            // Setup event handlers that use what we get from the readers
            colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;
            bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            // Get ready to draw graphics

            // Init the components (controls) of the window
            InitializeComponent();

            // Init color components
            drawingGroup = new DrawingGroup();


            // create the bitmap to display
            colorBitmap = new WriteableBitmap(1920, 1080, 96.0, 96.0, PixelFormats.Bgr32, null);
            ColorImage.Source = colorBitmap;
            //init game components
        }

        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                // Defensive programming: just in case the sensor frame is no longer valid
                if (bodyFrame == null)
                {
                    return;
                }

                if(bodies == null)
                {
                    // Create an array of the bodies in the scene and update it
                    bodies = new Body[bodyFrame.BodyCount];
                }
                bodyFrame.GetAndRefreshBodyData(bodies);

                // For each body in the scene
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        var joints = body.Joints; // Get all of the joints in that body
                        if (joints[JointType.HandRight].TrackingState == TrackingState.Tracked && joints[JointType.HandLeft].TrackingState == TrackingState.Tracked)
                        {
                            txtLeft.Text = joints[JointType.HandLeft].Position.Y.ToString();
                            txtRight.Text = joints[JointType.HandRight].Position.Y.ToString();
                        }
                    }
                }

                birdHeight += 4; // Gravity
                birdHeight = Math.Max(0, Math.Min(birdHeight, this.Height - 20)); // So the bird doesnt fall off the screen

                using (var canvas = drawingGroup.Open())
                {
                    // Create canvas to cover the screen
                    canvas.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, Width, Height)); // Draws transparent rect that is full Width and Height of screen

                    // Draw a bird in middle of screen at height of 200
                    canvas.DrawEllipse(Brushes.Blue, null, new Point(Width / 2, birdHeight), 20, 20);

                    GameImage.Source = new DrawingImage(drawingGroup);
                }
            }
        }

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            // Get the current image frame in a memory-safe manner
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                // Defensive programming: just in case the sensor fram is no longer valid
                if (colorFrame == null)
                {
                    return;
                }

                using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                {
                    // Put a thread-safe lock on this data so it doesn't get modified elsewhere
                    colorBitmap.Lock();

                    // Let the application know where the image is being stored
                    colorFrame.CopyConvertedFrameDataToIntPtr(colorBitmap.BackBuffer, (uint)(1920 * 1080 * 4), // Width * Height * BytesPerPixel
                        ColorImageFormat.Bgra);

                    // Let the app know that it needs to redraw the screen in this area (the whole image)
                    colorBitmap.AddDirtyRect(new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight));

                    // REmove the thread-safe lock on this data
                    colorBitmap.Unlock();
                }
            }
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Close the sensor when we close the window and the app
            sensor.Close();
        }
    }
}
