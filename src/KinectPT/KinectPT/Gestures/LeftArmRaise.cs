using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;

namespace KinectPT
{
    class LeftArmRaise
    {
        readonly int WINDOW_SIZE = 50;

        IGestureSegment[] _segments;

        int _currentSegment = 0;
        int _frameCount = 0;

        public event EventHandler GestureRecognized;

        public LeftArmRaise()
        {
            LeftArmRaiseSegment1 LeftArmRaiseSegment1 = new LeftArmRaiseSegment1();
            LeftArmRaiseSegment2 LeftArmRaiseSegment2 = new LeftArmRaiseSegment2();
            LeftArmRaiseSegment3 LeftArmRaiseSegment3 = new LeftArmRaiseSegment3();

            _segments = new IGestureSegment[]
            {

                LeftArmRaiseSegment1, LeftArmRaiseSegment2, LeftArmRaiseSegment3
            };
        }

        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="skeleton">The skeleton data.</param>
        public void Update(Body skeleton)
        {
            GesturePartResult result = _segments[_currentSegment].Update(skeleton);

            if (result == GesturePartResult.Succeeded)
            {
                if (_currentSegment + 1 < _segments.Length)
                {
                    _currentSegment++;
                    _frameCount = 0;
                }
                else
                {
                    if (GestureRecognized != null)
                    {
                        GestureRecognized(this, new EventArgs());
                        Reset();
                    }
                }
            }
            else if (result == GesturePartResult.Failed || _frameCount == WINDOW_SIZE)
            {
                Reset();
            }
            else
            {
                _frameCount++;
            }
        }

        /// <summary>
        /// Resets the current gesture.
        /// </summary>
        public void Reset()
        {
            _currentSegment = 0;
            _frameCount = 0;
        }
    }
}
