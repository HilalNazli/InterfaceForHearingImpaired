using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterfaceForHI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean isRepeated = false;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        double svAnswersHorizontalOffset = 0;
        public MainWindow()
        {
            InitializeComponent();
            mainWindow.WindowState = WindowState.Maximized;
            //ResizeMode = System.Windows.ResizeMode.CanResize;
            //SystemParameters.VirtualScreenWidth
            //System.Windows.SystemParameters.PrimaryScreenWidth;
            //SystemParameters.VirtualScreenHeight
            //System.Windows.SystemParameters.PrimaryScreenHeight;
            //double ratio = System.Windows.SystemParameters.PrimaryScreenWidth / mainWindow.Width;
            System.Diagnostics.Debug.WriteLine("Width is: " + mainWindow.Width);
            System.Diagnostics.Debug.WriteLine("Height is: " + mainWindow.Height);

            System.Diagnostics.Debug.WriteLine("Row Height is: " + mainWindow.Row2.Height);
            double ratio = System.Windows.SystemParameters.PrimaryScreenHeight/mainWindow.Height;
            
            mainWindow.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            mainWindow.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            restartVideo();

            initializeSizes(ratio);

            /*
             *dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
             *dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
             *dispatcherTimer.Start(); 
             */


        }

        private void initializeSizes(double ratio)
        {
           
            //Original sizes of all objects
            double heightOfIDropdown_ico = 64;
            double widthOfIDropdown_ico = 64;

            double heightOfIUser_ico = 64;
            double widthOfIUser_ico = 64;

            double heightOfIRe = 128;
            double widthOfIRe = 128;

            double fontSizeOfTBHow = 48;

            double heightOfI_Prev = 240;
            double widthOfI_Prev = 64;

            //Add additional things in slide bar here
            //double heightOfspAnswers = 350;
            //double widthOfspAnswers = 960;
            //double widthOfsvAnswers = 960;
            double heightOfsvAnswers = 350;
            double widthOfgAnswer = 295;

            double heightOfI_Next = 240;
            double widthOfI_Next = 64;

            double heightOfIBoun_ = 85;
            double widthOfIBoun_ = 85;

            double heightOfIPi_ = 85;
            double widthOfIPi_ = 85;

            double fontSizeOfTBProject = 24;

            //Resize everything with THE ratio yeah!
            mainWindow.IDropdown_ico.Height = heightOfIDropdown_ico * ratio;
            mainWindow.IDropdown_ico.Width = widthOfIDropdown_ico * ratio;

            mainWindow.IUser_ico.Height = heightOfIUser_ico * ratio;
            mainWindow.IUser_ico.Width = widthOfIUser_ico * ratio;

            mainWindow.IRe.Height = heightOfIRe * ratio;
            mainWindow.IRe.Width = widthOfIRe * ratio;

            mainWindow.TBHow.FontSize = fontSizeOfTBHow * ratio;

            mainWindow.I_Prev.Height = heightOfI_Prev * ratio;
            mainWindow.I_Prev.Width = widthOfI_Prev * ratio;


            //Add additional things in slide bar here
            //mainWindow.spAnswers.Height = heightOfspAnswers * ratio;
           // mainWindow.spAnswers.Width = widthOfspAnswers * ratio;
            //mainWindow.svAnswers.Width = widthOfsvAnswers * ratio;
            mainWindow.svAnswers.Height = heightOfsvAnswers * ratio;
            mainWindow.gAnswer1.Width = widthOfgAnswer * ratio;
            mainWindow.gAnswer2.Width = widthOfgAnswer * ratio;
            mainWindow.gAnswer3.Width = widthOfgAnswer * ratio;
            mainWindow.gAnswer4.Width = widthOfgAnswer * ratio;

            mainWindow.I_Next.Height = heightOfI_Next * ratio;
            mainWindow.I_Next.Width = widthOfI_Next * ratio;

            mainWindow.IBoun_.Height = heightOfIBoun_ * ratio;
            mainWindow.IBoun_.Width = widthOfIBoun_ * ratio;

            mainWindow.IPi_.Height = heightOfIPi_ * ratio;
            mainWindow.IPi_.Width = widthOfIPi_ * ratio;

            mainWindow.TBProject.FontSize = fontSizeOfTBProject * ratio;

           // mainWindow.meMainVideo.Height = mainWindow.Row1.ActualHeight; 
            mainWindow.meMainVideo.Height = meMainVideo.Height * ratio;
            //Resize margins

        }

        private void meMainVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (!isRepeated)
            {
                mainWindow.svAnswers.Visibility = System.Windows.Visibility.Visible;
                meBasAgrisiVideo.Play();
                meBasimDonuyorVideo.Play();
                meBogazimSistiVideo.Play();
                meBaskaVideo.Play();
            }
           
            restartVideo();
        }

        private void restartVideo()
        {
            
            meMainVideo.LoadedBehavior = MediaState.Manual;

            meMainVideo.Position = System.TimeSpan.Zero;
            meMainVideo.Play();
        }

        private void meBasAgrisiVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            meBasAgrisiVideo.Position = System.TimeSpan.Zero;
            meBasAgrisiVideo.Play();
        }

        private void meBasDonmesiVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
             meBasimDonuyorVideo.Position = System.TimeSpan.Zero;
             meBasimDonuyorVideo.Play();
        }

        private void meBogazimSistiVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
             meBogazimSistiVideo.Position = System.TimeSpan.Zero;
             meBogazimSistiVideo.Play();
        }

        private void I_Next_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            svAnswers.LineRight();
        }

        private void I_Prev_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            svAnswers.LineLeft();
           

        }

        private void meBaskaVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            meBaskaVideo.Position = System.TimeSpan.Zero;
            meBaskaVideo.Play();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //dispatcherTimer.Stop();
            // code goes here

            System.Diagnostics.Debug.WriteLine(svAnswers.HorizontalOffset);
           // System.Diagnostics.Debug.WriteLine(svAnswers.ContentHorizontalOffset);
            svAnswers.LineRight();

            //If viewer is at its right end, get it to left end
            if (svAnswersHorizontalOffset == svAnswers.HorizontalOffset && svAnswersHorizontalOffset != 0) {
                svAnswers.ScrollToLeftEnd();
            }
            svAnswersHorizontalOffset = svAnswers.HorizontalOffset;
        }

        private void svAnswers_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Visible changed!!!");

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start(); 
        }

    }
}
