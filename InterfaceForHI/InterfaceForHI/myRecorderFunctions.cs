namespace InterfaceForHI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using System.Windows.Controls;
    using System.Threading;

    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private const int MapDepthToByte = 8000 / 256;

        //If not fired after some time, check the following issue:
        //https://social.msdn.microsoft.com/Forums/en-US/608d1bac-059c-4ea5-aa28-8c2f9ebb88c7/kinect-is-not-firing-multisourceframearrived-event-after-some-time?forum=kinectv2sdk
        //Hope you'll not need it.
        private void Reader_RecordMultiSourceFrameArrived(Object sender, MultiSourceFrameArrivedEventArgs e)
        {

            nFrames = nFrames + 1;
            System.Console.WriteLine("nFrame:" + nFrames);

            MultiSourceFrameReference multiSourceFrameReference = e.FrameReference;
            MultiSourceFrame multiSourceFrame = multiSourceFrameReference.AcquireFrame();

            if (multiSourceFrame == null) return;

            ColorFrameReference colorFrameReference = multiSourceFrame.ColorFrameReference;
            //DepthFrameReference depthFrameReference = multiSourceFrame.DepthFrameReference;

            //DepthFrame depthFrame = depthFrameReference.AcquireFrame();
            ColorFrame colorFrame = colorFrameReference.AcquireFrame();
            if (colorFrameReference == null || colorFrame == null) return;
            //if (depthFrameReference == null || depthFrame == null) return;


            BodyFrameReference bodyFrameReference = multiSourceFrame.BodyFrameReference;
            BodyFrame bodyFrame = bodyFrameReference.AcquireFrame();

            if (bodyFrameReference == null || bodyFrame == null)
            {
                myCSVwriter.Append("Body Not Found\n");
            }
            // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
            // As long as those body objects are not disposed and not set to null in the array,
            // those body objects will be re-used.
            else
            {
                //handle null
                bodyFrame.GetAndRefreshBodyData(this.bodies);
                int i = 0;
                foreach (Body body in this.bodies)
                {
                    if (body.IsTracked)
                    {
                        i = i + 1;
                        string bodyFeatures = getBodyFeatures(body);
                        myCSVwriter.Append(bodyFeatures);
                    }
                }
                if (i < 1) myCSVwriter.Append("Body Not Found\n");

                bodyFrame.Dispose();
                
            }

            
            // Start Showing vıdeo
            #region recordColorFrame

            FrameDescription colorFrameDescription = colorFrame.FrameDescription;
            if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                colorFrame.CopyRawFrameDataToArray(this.colorImage);
            }
            else
            {
                colorFrame.CopyConvertedFrameDataToArray(this.colorImage, ColorImageFormat.Bgra);
            }

            try {
                colorFrameBGRA.Bytes = colorImage;
           
                colorFrameBGR = colorFrameBGRA.Convert<Bgr, Byte>();
                vwColor.WriteFrame(colorFrameBGR);
            }
            catch (Exception ee)
            {
                Console.WriteLine("{0} Exception caught.", ee);
            }
            this.colorImageBitmap.WritePixels(
                new Int32Rect(0, 0, colorFrameDescription.Width, colorFrameDescription.Height),
                this.colorImage,
                colorFrameDescription.Width * this.bytesPerPixel,
                0);
            colorFrame.Dispose();


            #endregion 
             
        }

        private String getJointFeatures(Joint myJoint, JointOrientation myJointOrientation)
        {
            String myJointFeatures = "";

            // Feature Type
            myJointFeatures += myJoint.JointType.ToString() + ";";

            // Tracking State
            myJointFeatures += myJoint.TrackingState.ToString() + ";";

            // Coordinates
            myJointFeatures += myJoint.Position.X.ToString() + ";";
            myJointFeatures += myJoint.Position.Y.ToString() + ";";
            myJointFeatures += myJoint.Position.Z.ToString() + ";";

            // Orientation
            myJointFeatures += myJointOrientation.Orientation.W.ToString() + ";";
            myJointFeatures += myJointOrientation.Orientation.X.ToString() + ";";
            myJointFeatures += myJointOrientation.Orientation.Y.ToString() + ";";
            myJointFeatures += myJointOrientation.Orientation.Z.ToString() + ";";
            
            // Mapping to Color Space
            ColorSpacePoint colorSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(myJoint.Position);
            myJointFeatures += colorSpacePoint.X.ToString() + ";";
            myJointFeatures += colorSpacePoint.Y.ToString() + ";";

            // Mapping to Depth Space
            DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(myJoint.Position);
            myJointFeatures += depthSpacePoint.X.ToString() + ";";
            myJointFeatures += depthSpacePoint.Y.ToString() + ";";
            
            return myJointFeatures;
        }

        private String getBodyFeatures(Body myBody)
        {
            String myBodyFeatures = "";

            IReadOnlyDictionary<JointType, Joint> myJoints = myBody.Joints;
            IReadOnlyDictionary<JointType, JointOrientation> myJointOrientations = myBody.JointOrientations;

            for (JointType i = JointType.SpineBase; i <= JointType.ThumbRight; i++)
            {
                myBodyFeatures += getJointFeatures(myJoints[i], myJointOrientations[i]);
            }

            // Hand Status and Confidience
            myBodyFeatures += myBody.HandLeftState.GetType().ToString() + ";" + myBody.HandLeftState.ToString() + ";" + myBody.HandLeftConfidence.ToString() + ";";
            myBodyFeatures += myBody.HandRightState.GetType().ToString() + ";" + myBody.HandRightState.ToString() + ";" + myBody.HandRightConfidence.ToString() + ";";

            // Lean Feature
            myBodyFeatures += myBody.Lean.GetType().ToString() + ";" + myBody.LeanTrackingState.ToString() + ";";
            myBodyFeatures += myBody.Lean.X.ToString() + ";" + myBody.Lean.Y.ToString() + ";";

            myBodyFeatures = myBodyFeatures.Substring(0, myBodyFeatures.Length - 1) + "\n";

            return myBodyFeatures;
        }
    }
}