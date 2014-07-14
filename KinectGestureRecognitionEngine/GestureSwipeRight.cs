using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectGestureRecognitionEngine
{
    class GestureSwipeRight : GestureBase
    {
        public GestureSwipeRight() : base(GestureType.SwipeRight)
        {
        }

        protected override bool ValidateGestureStartCondition(Skeleton skeleton)
        {
            throw new NotImplementedException();
        }

        protected override bool ValidateGestureEndCondition(Skeleton skeleton)
        {
            //hand is moving from 
            throw new NotImplementedException();
        }

        protected override bool ValidateGestureBaseCondition(Skeleton skeleton)
        {
            throw new NotImplementedException();
        }

        protected override bool IsGestureValid(Skeleton skeleton)
        {
            throw new NotImplementedException();
        }
    }
}
