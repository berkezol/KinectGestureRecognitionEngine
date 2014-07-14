using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Kinect;

namespace KinectGestureRecognitionEngine
{
    class GestureHandsClapping : GestureBase
    {
        private float previousDistance;

        public GestureHandsClapping() : base(GestureType.HandsClapping)
        {
            this.previousDistance = 0.0f;
            //this.handCoordinates();
        }

        #region old_code
        /*
        public void MatchHandsClapping(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                return;
            }
            if (skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked &&
                skeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked)
            {
                float currentDistance =
                    KinectGestureRecognitionEngine.GestureHelper.
                    GetJointDistance(skeleton.Joints[JointType.HandRight],
                    skeleton.Joints[JointType.HandLeft]);
                if (currentDistance < 0.1f && previousDistance > 0.1f)
                {
                    //invoke the event
                   // this.GestureRecognized(this, new GestureEventArgs(RecognitionResult.Success));
                   // if(this.GestureTimeStamp)
                }
                previousDistance = currentDistance;
            }
        }
*/
        #endregion
        
        protected override bool ValidateGestureStartCondition(Skeleton skeleton)
        {
            Helper.handCoordinates(skeleton);
            //both hands are tracked
            if (skeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked &&
                skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked &&
                skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ElbowLeft].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ElbowRight].Position.Y)
            {
                Helper.CurrentConditionDislplay("Clapping start condition validated");
                return true;
            }

/*
            bool toReturn = skeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked &&
                            skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked;
            if (toReturn == true)
            {
                Helper.CurrentConditionDislplay("Start condition valid");  
            }
            return toReturn;
*/
                return false;
        }

        protected override bool ValidateGestureEndCondition(Skeleton skeleton)
        {
            //if hands are together return true
            Helper.handCoordinates(skeleton);
            float currentDistance =
                KinectGestureRecognitionEngine.GestureHelper.
                    GetJointDistance(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.HandLeft]);

            bool toReturn = currentDistance < 0.1f && previousDistance > 0.1f;
            previousDistance = currentDistance;

            if (toReturn)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        (window as MainWindow).CurrentTextBox.Text = currentDistance.ToString();
                        (window as MainWindow).PreviousTextBox.Text = currentDistance.ToString();
                    }
                }
                Helper.CurrentConditionDislplay("End condition valid");                
            }
            else
            {
                return false;
                //AddText("motherfuckeeeeeeeeeeeeeeeeeeeeeeeer");
            }
            return toReturn;
        }

        protected override bool ValidateGestureBaseCondition(Skeleton skeleton)
        {
            //not needed, because it's a base gesture
            return true;
        }

        protected override bool IsGestureValid(Skeleton skeleton)
        {
            //not needed, because it's a base gesture
            return true;
        }
    }
}
