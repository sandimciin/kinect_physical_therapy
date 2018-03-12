//------------------------------------------------------------------------------
// <copyright file="ButtonSample.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
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
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ButtonSample
    /// </summary>
    public partial class ButtonSampleCopy : UserControl
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
        //DrawingGroup drawingGroup;


        /// <summary>
        /// Radius of drawn hand circles
        /// </summary>
        private const double HandSize = 30;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Constant for clamping Z values of camera space points from being negative
        /// </summary>
        private const float InferredZPositionClamp = 0.1f;

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as closed
        /// </summary>
        private readonly Brush handClosedBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as opened
        /// </summary>
        private readonly Brush handOpenBrush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as in lasso (pointer) position
        /// </summary>
        private readonly Brush handLassoBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for body frames
        /// </summary>
        //private BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        //private Body[] bodies = null;

        /// <summary>
        /// definition of bones
        /// </summary>
        private List<Tuple<JointType, JointType>> bones;

        /// <summary>
        /// Width of display (depth space)
        /// </summary>
        private int displayWidth;

        /// <summary>
        /// Height of display (depth space)
        /// </summary>
        private int displayHeight;

        /// <summary>
        /// List of colors for each body tracked
        /// </summary>
        private List<Pen> bodyColors;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonSampleCopy" /> class.
        /// </summary>
        public ButtonSampleCopy()
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
            this.InitializeComponent();

            // Init color components
            drawingGroup = new DrawingGroup();

            // get the coordinate mapper
            this.coordinateMapper = this.sensor.CoordinateMapper;

            // populate body colors, one for each BodyIndex
            this.bodyColors = new List<Pen>();

            this.bodyColors.Add(new Pen(Brushes.Red, 6));
            this.bodyColors.Add(new Pen(Brushes.Orange, 6));
            this.bodyColors.Add(new Pen(Brushes.Green, 6));
            this.bodyColors.Add(new Pen(Brushes.Blue, 6));
            this.bodyColors.Add(new Pen(Brushes.Indigo, 6));
            this.bodyColors.Add(new Pen(Brushes.Violet, 6));


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

                if (bodies == null)
                {
                    // Create an array of the bodies in the scene and update it
                    bodies = new Body[bodyFrame.BodyCount];
                }
                bodyFrame.GetAndRefreshBodyData(bodies);

                int penIndex = 0;
                // For each body in the scene
                foreach (Body body in bodies)
                {
                    Pen drawPen = this.bodyColors[penIndex++];

                    if (body.IsTracked)
                    {
                        var joints = body.Joints; // Get all of the joints in that body
                        if (joints[JointType.HandRight].TrackingState == TrackingState.Tracked && joints[JointType.HandLeft].TrackingState == TrackingState.Tracked)
                        {
                            txtLeft.Text = joints[JointType.HandLeft].Position.Y.ToString();
                            txtRight.Text = joints[JointType.HandRight].Position.Y.ToString();
                        }

                        using (DrawingContext dc = this.drawingGroup.Open())
                        {
                            dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));

                            IReadOnlyDictionary<JointType, Joint> jointsD = body.Joints;

                            // convert the joint points to depth (display) space
                            Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                            foreach (JointType jointType in jointsD.Keys)
                            {
                                // sometimes the depth(Z) of an inferred joint may show as negative
                                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                                CameraSpacePoint position = jointsD[jointType].Position;
                                if (position.Z < 0)
                                {
                                    position.Z = InferredZPositionClamp;
                                }

                                DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(position);
                                jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);

                            }
                            this.DrawBody(joints, jointPoints, dc, drawPen);

                            this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                            this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
                            GameImage.Source = new DrawingImage(drawingGroup);
                        }
                    }
                }
                /*
                birdHeight += 4; // Gravity
                birdHeight = Math.Max(0, Math.Min(birdHeight, this.Height - 20)); // So the bird doesnt fall off the screen

                using (var canvas = drawingGroup.Open())
                {
                    // Create canvas to cover the screen
                    canvas.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, Width, Height)); // Draws transparent rect that is full Width and Height of screen

                    // Draw a bird in middle of screen at height of 200
                    canvas.DrawEllipse(Brushes.Blue, null, new Point(Width / 2, birdHeight), 20, 20);

                    GameImage.Source = new DrawingImage(drawingGroup);
                }*/
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

        /// <summary>
        /// Draws a hand symbol if the hand is tracked: red circle = closed, green circle = opened; blue circle = lasso
        /// </summary>
        /// <param name="handState">state of the hand</param>
        /// <param name="handPosition">position of the hand</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawHand(HandState handState, Point handPosition, DrawingContext drawingContext)
        {
            switch (handState)
            {
                case HandState.Closed:
                    drawingContext.DrawEllipse(this.handClosedBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Open:
                    drawingContext.DrawEllipse(this.handOpenBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Lasso:
                    drawingContext.DrawEllipse(this.handLassoBrush, null, handPosition, HandSize, HandSize);
                    break;
            }
        }

        /// <summary>
        /// Draws a body
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="drawingPen">specifies color to draw a specific body</param>
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, Pen drawingPen)
        {
            // Draw the bones
            
            // Draw the joints
            foreach (JointType jointType in joints.Keys)
            {
                Brush drawBrush = null;

                TrackingState trackingState = joints[jointType].TrackingState;

                if (trackingState == TrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (trackingState == TrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, jointPoints[jointType], JointThickness, JointThickness);
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
