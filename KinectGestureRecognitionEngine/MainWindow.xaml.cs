using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Interaction;
using Microsoft.Kinect.Toolkit.Controls;



namespace KinectGestureRecognitionEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region local variables
        private KinectSensor _sensor;
        private InteractionStream _interactionStream;
        private Skeleton[] _skeletons;
        private UserInfo[] _userInfos;
        private GestureController _gestureController;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Helper.CurrentConditionDislplay("Application started");
            foreach (KinectSensor potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this._sensor = potentialSensor;
                    break;
                   
                }
            }
            if (this._sensor != null)
            {
                _gestureController = new GestureController();
                _gestureController.GestureRecognized += _gestureController_GestureRecognized;

                _skeletons = new Skeleton[_sensor.SkeletonStream.FrameSkeletonArrayLength];
                _userInfos = new UserInfo[InteractionFrame.UserInfoArrayLength];

                _sensor.DepthStream.Range = DepthRange.Default;
                _sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);

                _sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                _sensor.SkeletonStream.EnableTrackingInNearRange = false;
                _sensor.SkeletonStream.Enable();

                _interactionStream = new InteractionStream(_sensor, new DummyInteractionClient());
                _interactionStream.InteractionFrameReady += _interactionStream_InteractionFrameReady;

                _sensor.DepthFrameReady += _sensor_DepthFrameReady;
                _sensor.SkeletonFrameReady += _sensor_SkeletonFrameReady;

                _sensor.Start();
                //for auto scroll set to true
                Helper.IsLogsChangedPropertyInViewModel = true;
                Helper.CurrentConditionDislplay("Kinect sensor started");
            }
        }
        
        void _gestureController_GestureRecognized(object sender, GestureEventArgs e)
        {
            //System.Windows.MessageBox.Show(e.GestureyType.ToString());
            //for auto scroll set to true
            Helper.RecognizedGestureNameDisplay(e.GestureyType.ToString());
            if (e.GestureyType == GestureType.HandsClapping)
            {
                System.Media.SoundPlayer player =
                    new System.Media.SoundPlayer(
                        @"c:\Users\Zoltán\Documents\Visual Studio 2013\Projects\KinectGestureRecognitionEngine\KinectGestureRecognitionEngine\applause-01.wav");
                player.Play();
            }
            if (e.GestureyType == GestureType.SwipeLeft)
            {
                System.Media.SoundPlayer player =  
                    new System.Media.SoundPlayer(
                        @"c:\Users\Zoltán\Documents\Visual Studio 2013\Projects\KinectGestureRecognitionEngine\KinectGestureRecognitionEngine\rocket-01.wav");
                player.Play();
            }
        }

        void _sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null) return;
                try
                {
                    skeletonFrame.CopySkeletonDataTo(_skeletons);
                    var acceleroMeterReading = _sensor.AccelerometerGetCurrentReading();
                    _interactionStream.ProcessSkeleton(_skeletons,acceleroMeterReading,skeletonFrame.Timestamp);
                    if (_gestureController != null)
                    {
                        _gestureController.Skeleton = _skeletons.FirstOrDefault();
                        _gestureController.StartRecognize();
                    }
                    else
                    {
                        
                    }
                }
                catch (InvalidOperationException)
                {
                    
                } 
            }
        }

        void _sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame depthImageFrame = e.OpenDepthImageFrame())
            {
                if (depthImageFrame == null) return;

                try
                {
                    _interactionStream.ProcessDepth(depthImageFrame.GetRawPixelData(),depthImageFrame.Timestamp);
                }
                catch (InvalidOperationException)
                {

                }
            }
        }

        void _interactionStream_InteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
            using (InteractionFrame interactionFrame = e.OpenInteractionFrame())
            {
                if (interactionFrame == null) return;
                interactionFrame.CopyInteractionDataTo(_userInfos);
            }
        }
    }
}
