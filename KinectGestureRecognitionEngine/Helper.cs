using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Kinect;

namespace KinectGestureRecognitionEngine
{
    public static class Helper
    {
        public static bool IsLogsChangedPropertyInViewModel;

        public static bool IsLogsChangedPropertyInViewModel1
        {
            get { return IsLogsChangedPropertyInViewModel; }
            set { IsLogsChangedPropertyInViewModel = value; }
        }

        public static void RecognizedGestureNameDisplay(string gestureName)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).GestureNameTextBox.Text = gestureName;
                }
            }
        }
        public static void CurrentConditionDislplay(string gestureName)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).CurrentConditionTextBox.Text = gestureName;
                }
            }
        }

        public static void handCoordinates(Skeleton skeleton)
        {
            //Skeleton skeleton = new Skeleton();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).LeftTextBox.Text = "X:" +
                                                              skeleton.Joints[JointType.HandLeft].Position.X.ToString() +
                                                              "\nY:" +
                                                              skeleton.Joints[JointType.HandLeft].Position.Y.ToString() +
                                                              "\nZ: " +
                                                              skeleton.Joints[JointType.HandLeft].Position.Z.ToString();

                    (window as MainWindow).RighTextBox.Text = "X:" +
                                                              skeleton.Joints[JointType.HandRight].Position.X.ToString() +
                                                              "\nY:" +
                                                              skeleton.Joints[JointType.HandRight].Position.Y.ToString() +
                                                              "\nZ: " +
                                                              skeleton.Joints[JointType.HandRight].Position.Z.ToString();


                    (window as MainWindow).DistanceTextBox.Text = "Distance between hands: " +
                                                             Convert.ToString(KinectGestureRecognitionEngine.GestureHelper
                                                                 .GetJointDistance(
                                                                     skeleton.Joints[JointType.HandRight],
                                                                     skeleton.Joints[JointType.HandLeft]));
                }
            }
        }


        public static bool GetAutoScroll(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollProperty, value);
        }

        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(Helper), new PropertyMetadata(false, AutoScrollPropertyChanged));

        private static void AutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;

            if (scrollViewer != null && (bool)e.NewValue)
            {
                scrollViewer.ScrollToBottom();
            }
        }
    }
}
