using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectGestureRecognitionEngine
{
    class GestureController
    {
        int _skipFramesAfterGestureIsDetected = 0;
        public event EventHandler<GestureEventArgs> GestureRecognized;
        public GestureType GestureType { get; set; }
        public Skeleton Skeleton { get; set; }
        public bool IsGestureDetected { get; set; }

        private List<GestureBase> _gestureCollection = null;

        private void InitilizeGesture()
        {
            this._gestureCollection = new List<GestureBase>();
            //this._gestureCollection.Add(new GestureSwipeRight());
            this._gestureCollection.Add(new GesturesSwipeLeft());
            this._gestureCollection.Add(new GestureHandsClapping());
        }

        public GestureController()
        {
            this.InitilizeGesture();
        }

        public void StartRecognize()
        {
            if (this.IsGestureDetected)
            {
                while (this._skipFramesAfterGestureIsDetected <= 30)
                {
                    this._skipFramesAfterGestureIsDetected++;
                }
                this.RestGesture();
                return;
            }
            foreach (var item in this._gestureCollection)
            {
                if (item.CheckForGesture(this.Skeleton))
                {
                    if (this.GestureRecognized != null)
                    {
                        this.GestureRecognized(
                            this, new GestureEventArgs(
                                RecognitionResult.Success,
                                item.GestureType));
                        this.IsGestureDetected = true;
                    }
                }
            }
        }

        private void RestGesture()
        {
            this._gestureCollection = null;
            this.InitilizeGesture();
            this._skipFramesAfterGestureIsDetected = 0;
            this.IsGestureDetected = false;
        }
    }
}
