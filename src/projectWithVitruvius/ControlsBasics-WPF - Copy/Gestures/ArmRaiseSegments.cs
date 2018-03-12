using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;

namespace Microsoft.Samples.Kinect.ControlsBasics
{
 
    /// <summary>
    /// Represents a single gesture segment which uses relative positioning of body parts to detect a gesture.
    /// </summary>
    public interface IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        GesturePartResult Update(Body skeleton);
    }

    public class ArmRaiseSegment1 : IGestureSegment
    {
        public GesturePartResult Update(Body body)
        {
            // Right and Left Hands below hips
            if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.ElbowLeft].Position.Y && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.ElbowRight].Position.Y)
            {
                // Right and Left Hands are Outside Shoulders
                if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X && body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
                {
                    // Right and Left Elbows Within +/- .3 From Shoulders 
                    //if (body.Joints[JointType.ElbowRight].Position.Z <= body.Joints[JointType.ShoulderRight].Position.Z + .3 && body.Joints[JointType.ElbowRight].Position.Z >= body.Joints[JointType.ShoulderRight].Position.Z - .3 &&
                    //    body.Joints[JointType.ElbowLeft].Position.Z <= body.Joints[JointType.ShoulderLeft].Position.Z + .3 && body.Joints[JointType.ElbowLeft].Position.Z >= body.Joints[JointType.ShoulderLeft].Position.Z - .3)
                    //{
                        return GesturePartResult.Succeeded;
                    //}

                    //return GesturePartResult.Undetermined;
                }

                return GesturePartResult.Failed;
            }

            return GesturePartResult.Failed;
        }
    }

    public class ArmRaiseSegment2 : IGestureSegment
    {
        public GesturePartResult Update(Body body)
        {
            // Right and Left Hands are outside Shoulders
            if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X && body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
            {
                // Right and Left Hands are outside Elbows
                if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ElbowRight].Position.X && body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ElbowLeft].Position.X)
                {
                    // Right and Left Elbows within .3 from Shoulders
                    //if (body.Joints[JointType.ElbowRight].Position.Z <= body.Joints[JointType.ShoulderRight].Position.Z + .3 && body.Joints[JointType.ElbowRight].Position.Z >= body.Joints[JointType.ShoulderRight].Position.Z - .3 &&
                    //    body.Joints[JointType.ElbowLeft].Position.Z <= body.Joints[JointType.ShoulderLeft].Position.Z + .3 && body.Joints[JointType.ElbowLeft].Position.Z >= body.Joints[JointType.ShoulderLeft].Position.Z - .3)
                    //{
                        // Right and Left Elbows above Spine Mid
                        if (body.Joints[JointType.ElbowRight].Position.Y > body.Joints[JointType.SpineMid].Position.Y && body.Joints[JointType.ElbowLeft].Position.Y > body.Joints[JointType.SpineMid].Position.Y)
                        {
                            return GesturePartResult.Succeeded;
                        }
                        return GesturePartResult.Undetermined;
                   // }

                    //return GesturePartResult.Failed;
                }

                return GesturePartResult.Failed;
            }

            return GesturePartResult.Failed;
        }
    }

    public class ArmRaiseSegment3 : IGestureSegment
    {
        public GesturePartResult Update(Body body)
        {
            // Right and Left Hands are outside Shoulders
            if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X && body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
            {
                // Right and Left Hands are outside Elbows
                if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ElbowRight].Position.X && body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ElbowLeft].Position.X)
                {
                    // Right and Left Elbows within .3 from Shoulders Z axis (depth)
                    //if (body.Joints[JointType.ElbowRight].Position.Z <= body.Joints[JointType.ShoulderRight].Position.Z + .3 && body.Joints[JointType.ElbowRight].Position.Z >= body.Joints[JointType.ShoulderRight].Position.Z - .3 &&
                    //    body.Joints[JointType.ElbowLeft].Position.Z <= body.Joints[JointType.ShoulderLeft].Position.Z + .3 && body.Joints[JointType.ElbowLeft].Position.Z >= body.Joints[JointType.ShoulderLeft].Position.Z - .3)
                    //{
                        // Right and Left Elbows above or at the level to shoulders
                        if (body.Joints[JointType.ElbowRight].Position.Y >= body.Joints[JointType.ShoulderRight].Position.Y && body.Joints[JointType.ElbowLeft].Position.Y >= body.Joints[JointType.ShoulderLeft].Position.Y)
                        {
                            return GesturePartResult.Succeeded;
                        }
                        return GesturePartResult.Undetermined;
                    //}

                    //return GesturePartResult.Failed;
                }

                return GesturePartResult.Failed;
            }

            return GesturePartResult.Failed;
        }
    }

    public class ArmRaiseSegment4 : IGestureSegment
    {
        public GesturePartResult Update(Body body)
        {
            // Right and Left Elbows within .3 from Shoulder Z axis (depth)
            //if (body.Joints[JointType.ElbowRight].Position.Z <= body.Joints[JointType.ShoulderRight].Position.Z + .3 && body.Joints[JointType.ElbowRight].Position.Z >= body.Joints[JointType.ShoulderRight].Position.Z - .3 &&
            //            body.Joints[JointType.ElbowLeft].Position.Z <= body.Joints[JointType.ShoulderLeft].Position.Z + .3 && body.Joints[JointType.ElbowLeft].Position.Z >= body.Joints[JointType.ShoulderLeft].Position.Z - .3)
            //{
                // Right and Left Elbows above shoulders
                if (body.Joints[JointType.ElbowRight].Position.Y > body.Joints[JointType.ShoulderRight].Position.Y && body.Joints[JointType.ElbowLeft].Position.Y > body.Joints[JointType.ShoulderLeft].Position.Y)
                {
                    // Right and Left Hands are above Head 
                    if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.Head].Position.Y)
                    {
                        return GesturePartResult.Succeeded;
                    }

                    return GesturePartResult.Failed;
                }

                return GesturePartResult.Failed;
           // }

           // return GesturePartResult.Failed;
        }
    }

}
