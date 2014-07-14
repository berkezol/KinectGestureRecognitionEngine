using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectGestureRecognitionEngine
{
    class GesturesSwipeLeft : GestureBase
    {
        public GesturesSwipeLeft() : base(GestureType.SwipeLeft)
        {
            previousDistance = 100.0f;
        }

        private Joint validePosition;
        private Joint startingPosition;
        private float previousDistance;
        

        protected override bool ValidateGestureStartCondition(Skeleton skeleton)
        {
            // left hand joint below left elbow and spine
            // right hand joint below right shoulder and above right elbow
            if (skeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked && 
                skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ElbowLeft].Position.Y &&
                skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.Spine].Position.Y && 
                skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ShoulderRight].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.ElbowRight].Position.Y)
            { 
                Helper.handCoordinates(skeleton);
                startingPosition = skeleton.Joints[JointType.HandLeft];
                Helper.CurrentConditionDislplay("Swipe start position validated");
                return true;
            }
            // return true if start condition is valid else return false
            return false;
        }

        protected override bool ValidateGestureEndCondition(Skeleton skeleton)
        {
            // return true if end condition is valid else return false
            validePosition = skeleton.Joints[JointType.HandLeft];
            if (GestureHelper.GetJointDistance(startingPosition,skeleton.Joints[JointType.ShoulderLeft]) > 
                GestureHelper.GetJointDistance(validePosition,skeleton.Joints[JointType.ShoulderLeft]))
            {
                //Helper.CurrentConditionDislplay("Swipe to left gesture validated");
                return true;
            }
            //Helper.CurrentConditionDislplay("Swipe to left gesture invalid");
            return false;
        }

        protected override bool ValidateGestureBaseCondition(Skeleton skeleton)
        {            
            // check if the right hand position is between the the shoulder and the spine
           if (skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ShoulderRight].Position.Y &&
                skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipRight].Position.Y)
            {
                Helper.CurrentConditionDislplay("valid");
                return true;
            }
            Helper.CurrentConditionDislplay("Left swiping base condition invalid");
            return false;
            //return true;
        }
            
        protected override bool IsGestureValid(Skeleton skeleton)
        {
            // check if the hand is moving from the left to the right
            // return true if current position of gesture is still valid

            if(skeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked &&
                skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
            {                
                float CurrentDistance = GestureHelper.GetJointDistance(skeleton.Joints[JointType.ShoulderLeft],
                    skeleton.Joints[JointType.HandRight]);
                Helper.handCoordinates(skeleton);
                if (CurrentDistance < previousDistance)
                {
                    previousDistance = CurrentDistance;
                    //Helper.CurrentConditionDislplay("Left swiping still valid");
                    return true;
                }
            }
            //Helper.CurrentConditionDislplay("Left swiping corrupted");
            return false;

//           return true;
        }
    }
}
