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
    /// The first part of a <see cref="LeftArmRaise"/> gesture.
    /// </summary>
    public class LeftArmRaiseSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {

            // left hand in front of left elbow
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
            {
                // left hand below shoulder height but above hip height
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // left hand left of left shoulder
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Undetermined;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The second part of a <see cref="GestureType.SwipeUp"/> gesture.
    /// </summary>
    public class LeftArmRaiseSegment2 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // left hand in front of left shoulder
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ShoulderLeft].Position.Z)
            {
                // left hand above left shoulder
                if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ShoulderLeft].Position.Y)
                {
                    // left hand left of left shoulder
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Undetermined;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The third part of a <see cref="GestureType.SwipeUp"/> gesture.
    /// </summary>
    public class LeftArmRaiseSegment3 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //left hand in front of left shoulder
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ShoulderLeft].Position.Z)
            {
                // left hand above head
                if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.Head].Position.Y)
                {
                    // left hand left of left shoulder
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Undetermined;
                }

                // Debug.WriteLine("GesturePart 2 - left hand below shoulder height but above hip height - FAIL");
                return GesturePartResult.Failed;
            }

            // Debug.WriteLine("GesturePart 2 - left hand in front of left shoulder - FAIL");
            return GesturePartResult.Failed;
        }
    }


}
