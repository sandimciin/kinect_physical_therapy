using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;

namespace Microsoft.Samples.Kinect.ControlsBasics
{
    
    public class ArmRaise
    {
        readonly int WINDOW_SIZE = 50;

        IGestureSegment[] _segments;

        int _currentSegment = 0;
        int _frameCount = 0;

        public event EventHandler GestureRecognized;

        public ArmRaise()
        {
            ArmRaiseSegment1 ArmRaiseSegment1 = new ArmRaiseSegment1();
            ArmRaiseSegment2 ArmRaiseSegment2 = new ArmRaiseSegment2();
            ArmRaiseSegment3 ArmRaiseSegment3 = new ArmRaiseSegment3();
            ArmRaiseSegment4 ArmRaiseSegment4 = new ArmRaiseSegment4();

            _segments = new IGestureSegment[]
            {

                ArmRaiseSegment4
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
