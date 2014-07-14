using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectGestureRecognitionEngine
{
    public abstract class GestureBase
    {
        protected GestureBase(GestureType gestureType)
        {
            GestureType = gestureType;
            CurrentFrameCount = 0;
        }
        private bool isRecognitionStarted { get; set; }
        private int CurrentFrameCount { get; set; }
        public GestureType GestureType { get; set; }

        protected virtual int MaximumNumberOfFramesToProcess
        {
            get { return 30; }
        }

        public long GestureTimeStamp { get; set; }

        protected abstract bool ValidateGestureStartCondition(Skeleton skeleton);
        protected abstract bool ValidateGestureEndCondition(Skeleton skeleton);
        protected abstract bool ValidateGestureBaseCondition(Skeleton skeleton);
        protected abstract bool IsGestureValid(Skeleton skeleton);

        public virtual bool CheckForGesture(Skeleton skeleton)
        {
            if (this.isRecognitionStarted == false)
            {
                if (this.ValidateGestureStartCondition(skeleton))
                {
                    this.isRecognitionStarted = true;
                    this.CurrentFrameCount = 0;
                }
            }
            else
            {
                if (this.CurrentFrameCount == this.MaximumNumberOfFramesToProcess)
                {
                    this.isRecognitionStarted = false;
                    if (ValidateGestureBaseCondition(skeleton) &&
                        ValidateGestureEndCondition(skeleton))
                    {
                        return true;
                    }
                }
                this.CurrentFrameCount++;
                if (!IsGestureValid(skeleton) &&
                    !ValidateGestureBaseCondition(skeleton))
                {
                    this.isRecognitionStarted = false;
                }
            }
            return false;
        }
    }
}
