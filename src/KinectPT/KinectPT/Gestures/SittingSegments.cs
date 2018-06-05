using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;

namespace KinectPT
{
    /// The first part of a <see cref="Sitting"/> gesture.
    public class SittingSegment1 : IGestureSegment
    {
        /// Updates the current gesture.
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // head above hips
            if (body.Joints[JointType.Head].Position.Y > body.Joints[JointType.HipLeft].Position.Y)
            {
                // feet below knees
                if (body.Joints[JointType.FootLeft].Position.Y < body.Joints[JointType.KneeLeft].Position.Y && body.Joints[JointType.FootRight].Position.Y < body.Joints[JointType.FootRight].Position.Y)
                {
                    // hips above knees
                    if (body.Joints[JointType.HipLeft].Position.Y > body.Joints[JointType.KneeLeft].Position.Y)
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
    
    /// The second part of a <see cref="Sitting"/> gesture.
    public class SittingSegment2 : IGestureSegment
    {
        /// Updates the current gesture.
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // head above hips
            if (body.Joints[JointType.Head].Position.Y > body.Joints[JointType.HipLeft].Position.Y)
            {
                // feet below knees
                if (body.Joints[JointType.FootLeft].Position.Y < body.Joints[JointType.KneeLeft].Position.Y && body.Joints[JointType.FootRight].Position.Y < body.Joints[JointType.FootRight].Position.Y)
                {
                    // hips behind knees
                    if (body.Joints[JointType.HipLeft].Position.X < body.Joints[JointType.KneeLeft].Position.X + .2)
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
    
    /// The third part of a <see cref="Sitting"/> gesture.
    public class SittingSegment3 : IGestureSegment
    {
        /// Updates the current gesture.
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // head above hips
            if (body.Joints[JointType.Head].Position.Y > body.Joints[JointType.HipLeft].Position.Y)
            {
                // feet below knees
                if (body.Joints[JointType.FootLeft].Position.Y < body.Joints[JointType.KneeLeft].Position.Y && body.Joints[JointType.FootRight].Position.Y < body.Joints[JointType.FootRight].Position.Y)
                {
                    // knee height close to hip height
                    if (body.Joints[JointType.HipLeft].Position.Y <= body.Joints[JointType.KneeLeft].Position.Y + .2)
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
}
