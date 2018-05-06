using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectPT
{
    public class Globals
    {
        bool beginAtExerciseStart=false;  //if true, recording starts at exercise starts. if false, recording starts when window opens

        bool customDuration=false;  //if true, set Duration. if false, recording ends at exercise end

        float Duration;

        bool customFrequency = false;

        float Frequency;  //frequency of recording

    }
}
