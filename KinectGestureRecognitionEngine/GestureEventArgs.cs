using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KinectGestureRecognitionEngine
{
    class GestureEventArgs : EventArgs
    {
        public RecognitionResult Result { get; internal set; }
        public GestureType GestureyType { get; internal set; }

        public GestureEventArgs(RecognitionResult result, GestureType type)
        {
            Result = result;
            GestureyType = type;
        }
    }
}
