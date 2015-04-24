using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using Microsoft.Kinect;



namespace InterfaceForHI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {

        /////////////////////////////////////////////////////////
        /// Active Kinect sensor
        private KinectSensor kinectSensor = null;

        /// Size of the RGB pixel in the bitmap
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        // MultiSourceFrame Variables
        MultiSourceFrameReader myMultiSourceFrameReader = null;
        FrameSourceTypes myFrameSources = FrameSourceTypes.Color;


        // Color Image Variables
        private WriteableBitmap colorImageBitmap = null;
        private byte[] colorImage = null;
        /////////////////////////////////////////////////////////

        Person person = new Person();
        double ratio = 0;
        Boolean isRepeated = false;
        Boolean isInQuestion4 = false;
        int count = 0;
        int letItLoadCount = 0;
        int videoTrickCount = 0;
        double svAnswersHorizontalOffset = 0;

        System.Windows.Threading.DispatcherTimer dispatcherTimerForCamera = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            /////////////////////////////////////////////////////////
            // for Alpha, one sensor is supported
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {

                // open the sensor
                this.kinectSensor.Open();

                // Display Multi Source Frame Reader
                this.myMultiSourceFrameReader = this.kinectSensor.OpenMultiSourceFrameReader(myFrameSources);

                // Color Variable Initialization
                FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.FrameDescription;
                this.colorImage = new byte[colorFrameDescription.Width * colorFrameDescription.Height * this.bytesPerPixel];
                this.colorImageBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

            }
            /////////////////////////////////////////////////////////

            InitializeComponent();
            mainWindow.WindowState = WindowState.Maximized;
            ratio = System.Windows.SystemParameters.PrimaryScreenHeight/mainWindow.Height;
            
            mainWindow.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            mainWindow.Height = System.Windows.SystemParameters.PrimaryScreenHeight;



            //1- Question is loaded and played once(or twice)
            //2- Answers are shown. Camera is on, for 5 seconds.
            //3- Question is shown again (for now). 

            loadQuestion1();
            initializeSizes(ratio);

            this.imgDisplayImage.Source = colorImageSource;
        }
        public ImageSource colorImageSource
        {
            get
            {
                return this.colorImageBitmap;
            }
        }

        /// Execute start up tasks
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.myMultiSourceFrameReader != null)
            {
                this.myMultiSourceFrameReader.MultiSourceFrameArrived += this.Reader_DisplayMultiSourceFrameArrived;
            }
        }


        private void Reader_DisplayMultiSourceFrameArrived(Object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrameReference multiSourceFrameReference = e.FrameReference;

            MultiSourceFrame multiSourceFrame = multiSourceFrameReference.AcquireFrame();

            if (multiSourceFrame == null) return;

            // Get FrameReferences for each source
            ColorFrameReference colorFrameReference = multiSourceFrame.ColorFrameReference;

            using (ColorFrame colorFrame = colorFrameReference.AcquireFrame())
            {
                if (colorFrame == null) return;


                FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                #region colorFrame processing

                // verify data and write the new color frame data to the display bitmap
                if ((colorFrameDescription.Width == this.colorImageBitmap.PixelWidth) &&
                    (colorFrameDescription.Height == this.colorImageBitmap.PixelHeight))
                {
                    if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                    {
                        colorFrame.CopyRawFrameDataToArray(this.colorImage);
                    }
                    else
                    {
                        colorFrame.CopyConvertedFrameDataToArray(this.colorImage, ColorImageFormat.Bgra);
                    }

                    this.colorImageBitmap.WritePixels(
                        new Int32Rect(0, 0, colorFrameDescription.Width, colorFrameDescription.Height),
                        this.colorImage,
                        colorFrameDescription.Width * this.bytesPerPixel,
                        0);
                }
                #endregion
            }
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

            double fontSizeOfTBQuestion = 48;

            double heightOfI_Prev = 240;
            double widthOfI_Prev = 64;

            //Add additional things in slide bar here
            //double heightOfspAnswers = 350;
            //double widthOfspAnswers = 960;
            //double widthOfsvAnswers = 960;
            double heightOfsvAnswers = 350;

            double heightOfI_Next = 240;
            double widthOfI_Next = 64;

            double heightOfIBoun_ = 85;
            double widthOfIBoun_ = 85;

            double heightOfIPi_ = 85;
            double widthOfIPi_ = 85;

            double fontSizeOfTBProject = 24;

            double heightOfTBName = 48;
            double widthOfTbName = 480;
            double fontOfTBName = 24;

            double heightOfBName = 48;
            double widthOfBName = 100;
            double fontOfBName = 24;

            double heightOfMeMainVideo = 480;

            double widthOfLVMenu = 280;
            //Resize everything with THE ratio yeah!
            mainWindow.IDropdown_ico.Height = heightOfIDropdown_ico * ratio;
            mainWindow.IDropdown_ico.Width = widthOfIDropdown_ico * ratio;

            mainWindow.IUser_ico.Height = heightOfIUser_ico * ratio;
            mainWindow.IUser_ico.Width = widthOfIUser_ico * ratio;

            mainWindow.IRe.Height = heightOfIRe * ratio;
            mainWindow.IRe.Width = widthOfIRe * ratio;

            mainWindow.TBQuestion.FontSize = fontSizeOfTBQuestion * ratio;

            mainWindow.I_Prev.Height = heightOfI_Prev * ratio;
            mainWindow.I_Prev.Width = widthOfI_Prev * ratio;


            //Add additional things in slide bar here
            //mainWindow.spAnswers.Height = heightOfspAnswers * ratio;
           // mainWindow.spAnswers.Width = widthOfspAnswers * ratio;
            //mainWindow.svAnswers.Width = widthOfsvAnswers * ratio;
            mainWindow.svAnswers.Height = heightOfsvAnswers * ratio;

            mainWindow.I_Next.Height = heightOfI_Next * ratio;
            mainWindow.I_Next.Width = widthOfI_Next * ratio;

            mainWindow.IBoun_.Height = heightOfIBoun_ * ratio;
            mainWindow.IBoun_.Width = widthOfIBoun_ * ratio;

            mainWindow.IPi_.Height = heightOfIPi_ * ratio;
            mainWindow.IPi_.Width = widthOfIPi_ * ratio;

            mainWindow.TBProject.FontSize = fontSizeOfTBProject * ratio;

           // mainWindow.meMainVideo.Height = mainWindow.Row1.ActualHeight; 
            mainWindow.meMainVideo.Height = heightOfMeMainVideo * ratio;
            mainWindow.imgDisplayImage.Height = heightOfMeMainVideo * ratio;
            //Resize margins

            tbName.Height = heightOfTBName * ratio;
            tbName.Width = widthOfTbName * ratio;
            tbName.FontSize = fontOfTBName * ratio;

            bName.Height = heightOfBName * ratio;
            bName.Width = widthOfBName * ratio;
            bName.FontSize = fontOfBName * ratio;
            bOtherName.Height = heightOfBName * ratio;
            bOtherName.Width = widthOfBName * ratio;
            bOtherName.FontSize = fontOfBName * ratio;

            lvMenu.Width = widthOfLVMenu * ratio;

        }


        private void initializeAnswerVideoProperties(object sender, RoutedEventArgs args) {
            MediaElement me = (MediaElement)args.Source;
            DockPanel.SetDock(me, Dock.Top);
            me.Height = 240 * ratio;

            me.LoadedBehavior = System.Windows.Controls.MediaState.Manual;
            me.MediaEnded += new RoutedEventHandler(AnswerMediaEnded);
            me.MouseLeftButtonDown += new MouseButtonEventHandler(AnswerMediaMouseLeftButtonDown);
            //me.Stretch = Stretch.Uniform;

        }
        private void initializeAnswerTextProperties(object sender, RoutedEventArgs args)
        {
            TextBlock tb = (TextBlock)args.Source;
            tb.FontSize = 24 * ratio;
            //tb.Width = 100 * ratio;
            tb.TextAlignment = TextAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.Foreground = (System.Windows.Media.Brush)mainWindow.Resources["BlackPen"];
        }

        private void initializeAnswerGridProperties(object sender, RoutedEventArgs args)
        {
            Grid g = (Grid)args.Source;
            Thickness margin = new System.Windows.Thickness();
            margin.Left = 32 * ratio;
            g.Margin = margin;
            g.Width = 295 * ratio;
        }
        private void initializeAnswerDockPanelProperties(object sender, RoutedEventArgs args)
        {
            DockPanel dp = (DockPanel)args.Source;
            dp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

        }
        private void AnswerMediaEnded(object sender, RoutedEventArgs args)
        {
            MediaElement me = (MediaElement)args.Source;
            me.Position = System.TimeSpan.Zero;
            me.Play();
        }
        private void AnswerMediaMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            MediaElement me = (MediaElement)args.Source;
            DockPanel dp = new DockPanel();
            dp = (DockPanel)me.Parent;
            TextBlock tb = new TextBlock();
            tb = dp.Children.OfType<TextBlock>().FirstOrDefault();

            ListViewItem lvi = new ListViewItem();
            lvi.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            if (tb.Text.Equals("Hastayım"))
            {
                person.programFlow = person.programFlow + "Bu kişi hastadır.\n";
                person.isSick = true;
                person.isSeekingInfo = false;
 
                loadQuestion3();
            }
            else if (tb.Text.Equals("Bilgi Almak İstiyorum"))
            {
                person.programFlow = person.programFlow + "Bu kişi bilgi almak istiyor.\n";
                person.isSeekingInfo = true;
                person.isSick = false;
       
                loadQuestion2();
            }
            else if (tb.Text.Equals("Hasta Ziyareti İçin Geldim"))
            {
                person.programFlow = person.programFlow + "Hasta ziyareti için geldi.\n";
                person.soughtInfo = tb.Text;

                //Direction hastanin adini giriniz.
                loadDirection1();
            }
            else if (tb.Text.Equals("Sigorta Bilgisi Almak İstiyorum"))
            {
                person.programFlow = person.programFlow + "Sigorta bilgisi almak istiyor.\n";
                person.soughtInfo = tb.Text;
 
                //Kimliginizle danismaya
                loadDirection2();
            }
            else if (tb.Text.Equals("Ücret Sormak İstiyorum"))
            {
                person.programFlow = person.programFlow + "Ücret sormak istiyor.\n";
                person.soughtInfo = tb.Text;

                //Kimliginizle danismaya
                loadDirection2();
            }
            else if (tb.Text.Equals("Yer Sormak İstiyorum"))
            {
                person.programFlow = person.programFlow + "Yer sormak istiyor.\n";
                person.soughtInfo = tb.Text;
  
                //Kimliginizle danismaya
                loadDirection2();
            }
            else if (tb.Text.Equals("Evet, Acil"))
            {
                person.programFlow = person.programFlow + "Durumu acil.\n";
                person.isEmergency = true;

                loadQuestion4Emergency();
            }
            else if (tb.Text.Equals("Hayır, Acil Değil"))
            {
                person.programFlow = person.programFlow + "Durumu acil değil.\n";
                person.isEmergency = false;

                loadQuestion4NotEmergency();
            }
            else if (tb.Text.Equals("Hayır, Yok"))
            {
                person.programFlow = person.programFlow + "Randevusu yok.\n";
                person.hasAppointment = false;

                //Isminizi yaziniz.
                loadDirection3();
            }
            else if (tb.Text.Equals("Evet, Var"))
            {
                person.programFlow = person.programFlow + "Randevusu var.\n";
                person.hasAppointment = true;

                //isminizi yaziniz
                loadDirection3();
            }
            else if (!tb.Text.Equals(""))
            {
                person.programFlow = person.programFlow + "Şikayeti:  " + tb.Text + "\n";
                person.grievance = tb.Text;
 
                if (!person.isEmergency) {
                    loadQuestion5();
                }
                else
                {
                    //isminizi yaziniz
                    loadDirection3();
                }

            }
            System.Diagnostics.Debug.WriteLine(person.programFlow);


        }

        private void listViewItemMouseDown(object sender, MouseButtonEventArgs args)
        {
            //System.Diagnostics.Debug.WriteLine("Menude birseye basildi.");
            bClose.Visibility = System.Windows.Visibility.Hidden;
            lvMenu.Visibility = System.Windows.Visibility.Hidden;
            ListViewItem lvi = new ListViewItem();
            lvi = (ListViewItem)args.Source;
            if (lvi.Content.Equals("Giriş")) {
                loadQuestion1();
                //System.Diagnostics.Debug.WriteLine("Menude girise basildi.");
            }
            else if (lvi.Content.Equals("Hasta")) {
                loadQuestion3();
            }
            else if (lvi.Content.Equals("Bilgi")) {
                loadQuestion2();
            }
            else if (lvi.Content.Equals("Ziyaret")) {
                loadDirection1();
            }
            else if (lvi.Content.Equals("Acil")) {
                loadQuestion3();
            }
            else if (lvi.Content.Equals("Acil Değil")) {
                loadQuestion3();
            }
            else if (lvi.Content.Equals("Randevu Yok"))
            {
                loadQuestion5();
            }
            else if (lvi.Content.Equals("Randevu Var")) {
                loadQuestion5();
            }
            else if (lvi.Content.Equals("Hasta İsmi")) {
                loadDirection1();
            }
            else if (lvi.Content.Equals("İsim"))
            {
                loadDirection3();
            }
            else if(!lvi.Content.Equals("")){
                if (!person.isEmergency)
                {
                    loadQuestion4NotEmergency();
                }
                else
                {
                    loadQuestion4Emergency();
                }
            }

        }

        private void meMainVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
         

            if (!isRepeated)
            {
                mainWindow.svAnswers.Visibility = System.Windows.Visibility.Visible;
                if (!isInQuestion4)
                {
                    playAllChildren(spAnswers);
                }
                else {
                    playSomeChildren(spAnswers);
                }
            }
            
            //restartVideo();
            
            isRepeated = true;

            //
            //Do not restart.
            //When the media is ended, make it hidden, so that the user can see himself/herself on the screen.
            //Set timer to 5 seconds. Record the user for 5 seconds and then restrart the video.
            mainWindow.meMainVideo.Visibility = System.Windows.Visibility.Hidden;
            mainWindow.imgDisplayImage.Visibility = System.Windows.Visibility.Visible;
            mainWindow.MiddleLineBorder.Background = (System.Windows.Media.Brush)mainWindow.Resources["GreenBrush"];
            mainWindow.MiddleLineBorder_.Background = (System.Windows.Media.Brush)mainWindow.Resources["GreenBrush_"];

            //Recording
            dispatcherTimerForCamera.Tick += new EventHandler(dispatcherTimer_forCamera_Tick);
            dispatcherTimerForCamera.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimerForCamera.Start();



        }
        public void playAllChildren(StackPanel stackPanel)
        {
            //System.Collections.Generic.List<MediaElement> meAnswers = new System.Collections.Generic.List<MediaElement>();
            foreach(Grid g in stackPanel.Children){
                //System.Diagnostics.Debug.WriteLine("Grid found");
                DockPanel d = g.Children.OfType<DockPanel>().FirstOrDefault();
                MediaElement me = d.Children.OfType<MediaElement>().FirstOrDefault();
                me.Play();
            }
        }
        public void playSomeChildren(StackPanel stackPanel) {

            //System.Collections.Generic.List<MediaElement> meAnswers = new System.Collections.Generic.List<MediaElement>();
            int runningChildCount = 0;
            foreach (Grid g in stackPanel.Children)
            {
                if (!(isInQuestion4 && runningChildCount == 5))
                {
                    //System.Diagnostics.Debug.WriteLine("Grid found");
                    DockPanel d = g.Children.OfType<DockPanel>().FirstOrDefault();
                    MediaElement me = d.Children.OfType<MediaElement>().FirstOrDefault();
                    me.Play();
                    runningChildCount++;
                }
                else {
                    DockPanel d = g.Children.OfType<DockPanel>().FirstOrDefault();
                    MediaElement me = d.Children.OfType<MediaElement>().FirstOrDefault();
                    me.Stop();
                }

            }
        }

        private void restartVideo()
        {
            meMainVideo.Position = System.TimeSpan.Zero;
            meMainVideo.Play();
        }

        private void I_Next_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            svAnswers.LineRight();
            incrementVideoTrickCount();
            svAnswers.LineRight();
            incrementVideoTrickCount();
            svAnswers.LineRight();
            incrementVideoTrickCount();
        }

        private void I_Prev_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            svAnswers.LineLeft();
            decrementVideoTrickCount();
            svAnswers.LineLeft();
            decrementVideoTrickCount();
            svAnswers.LineLeft();
            decrementVideoTrickCount();

        }
        private void incrementVideoTrickCount() {
            videoTrickCount++;
            System.Diagnostics.Debug.WriteLine(videoTrickCount);
            if (videoTrickCount % 15 == 0 && videoTrickCount != 0)
            {
                int videoOff = videoTrickCount / 15 - 1;
                //0.yi kapat 5.yi ac
                
                videoTrick(spAnswers, videoOff, true);
                
            }
        }

        private void decrementVideoTrickCount() {
            videoTrickCount--;
            System.Diagnostics.Debug.WriteLine(videoTrickCount);
            if (videoTrickCount % 15 == 0 && videoTrickCount != 0)
            {
                int videoOff = videoTrickCount / 15 - 1;
                //0.i kapat -5.i ac
  
                videoTrick(spAnswers, videoOff, false);
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (letItLoadCount < 3) {
                letItLoadCount++;
            }else{ 
                if (isInQuestion4)
                {
                    incrementVideoTrickCount();
                }
                //System.Diagnostics.Debug.WriteLine(svAnswers.HorizontalOffset);
                svAnswers.LineRight();

                //If viewer is at its right end, get it to left end
                if (svAnswersHorizontalOffset == svAnswers.HorizontalOffset && svAnswersHorizontalOffset != 0) {
                    count++;
                }
                if (count == 2) {
                    count = 0;
                    svAnswers.ScrollToLeftEnd();
                    videoTrickCount = 0;
                    if (isInQuestion4) {
                        playSomeChildren(spAnswers);
                    }
                }
                svAnswersHorizontalOffset = svAnswers.HorizontalOffset;
            }
        }

        private void dispatcherTimer_forCamera_Tick(object sender, EventArgs e) {
            //StopRecording
            //Restart the question video for now.
            //Later we will process what we've recorded.

            mainWindow.meMainVideo.Visibility = System.Windows.Visibility.Visible;
            mainWindow.imgDisplayImage.Visibility = System.Windows.Visibility.Hidden;

            mainWindow.MiddleLineBorder.Background = (System.Windows.Media.Brush)mainWindow.Resources["OrangeBrush"];
            mainWindow.MiddleLineBorder_.Background = (System.Windows.Media.Brush)mainWindow.Resources["OrangeBrush_"];
            restartVideo();
            dispatcherTimerForCamera.Stop();


        }
        private void videoTrick(StackPanel stackPanel, int videoOff, Boolean isIncrement)
        {
            foreach (Grid g in stackPanel.Children)
            {
               
                if (stackPanel.Children.IndexOf(g) == videoOff) {
                    if (isIncrement)
                    {
                        System.Diagnostics.Debug.WriteLine("Video to stop is found");
                        DockPanel d = g.Children.OfType<DockPanel>().FirstOrDefault();
                        TextBlock tb = d.Children.OfType<TextBlock>().FirstOrDefault();
                        System.Diagnostics.Debug.WriteLine(tb.Text);
                        MediaElement me = d.Children.OfType<MediaElement>().FirstOrDefault();
                        me.Stop();
                    }
                    else {
                        System.Diagnostics.Debug.WriteLine("Video to start is found");
                        DockPanel d = g.Children.OfType<DockPanel>().FirstOrDefault();
                        TextBlock tb = d.Children.OfType<TextBlock>().FirstOrDefault();
                        System.Diagnostics.Debug.WriteLine(tb.Text);
                        MediaElement me = d.Children.OfType<MediaElement>().FirstOrDefault();
                        me.Play();
                    }
                }
                else if (stackPanel.Children.IndexOf(g) == (videoOff + 5))
                {
                    if (isIncrement)
                    {
                        System.Diagnostics.Debug.WriteLine("Video to start is found");
                        DockPanel d = g.Children.OfType<DockPanel>().FirstOrDefault();
                        TextBlock tb = d.Children.OfType<TextBlock>().FirstOrDefault();
                        System.Diagnostics.Debug.WriteLine(tb.Text);
                        MediaElement me = d.Children.OfType<MediaElement>().FirstOrDefault();
                        me.Play();
                    
                    }
                    else {
                        System.Diagnostics.Debug.WriteLine("Video to stop is found");
                        DockPanel d = g.Children.OfType<DockPanel>().FirstOrDefault();
                        TextBlock tb = d.Children.OfType<TextBlock>().FirstOrDefault();
                        System.Diagnostics.Debug.WriteLine(tb.Text);
                        MediaElement me = d.Children.OfType<MediaElement>().FirstOrDefault();
                        me.Stop();
                    }
                }
               
            }

        }
        private void svAnswers_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Visible changed!!!");
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

            if (svAnswers.Visibility.Equals(System.Windows.Visibility.Visible))
            {
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                if (isInQuestion4)
                {
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                }
                else {
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 3);

                }
                dispatcherTimer.Start();
            }
            else {
                spAnswers.Children.Clear();
                dispatcherTimer.Stop();
                videoTrickCount = 0;
                letItLoadCount = 0;
            }
        }

        private void loadQuestion(String question, String questionPath, int answersCount, String[] answers, String[] answersPath) {
            //If clicked when camera is on.
            dispatcherTimerForCamera.Stop();

            mainWindow.meMainVideo.Visibility = System.Windows.Visibility.Visible;
            mainWindow.imgDisplayImage.Visibility = System.Windows.Visibility.Hidden;

            mainWindow.MiddleLineBorder.Background = (System.Windows.Media.Brush)mainWindow.Resources["OrangeBrush"];
            mainWindow.MiddleLineBorder_.Background = (System.Windows.Media.Brush)mainWindow.Resources["OrangeBrush_"];

            //Set the question in the main video
            TBQuestion.Text = question;
            meMainVideo.Source = new System.Uri(questionPath, UriKind.Relative);
            isRepeated = false;
            svAnswers.Visibility = System.Windows.Visibility.Hidden;
            restartVideo();
            //Clear the stack panel for the answers
            spAnswers.Children.Clear();
            //Fill the stack panel with answers
            for (int i = 0; i < answersCount; i++)
            {
                //ANSWER i
                //  Declare answer grid
                Grid g = new Grid();
                spAnswers.Children.Add(g);
                g.Loaded += new RoutedEventHandler(initializeAnswerGridProperties);
                //    Declare answer dock panel
                DockPanel dp = new DockPanel();
                g.Children.Add(dp);
                dp.Loaded += new RoutedEventHandler(initializeAnswerDockPanelProperties);
                //      Declare answer media element
                MediaElement m = new MediaElement();
                dp.Children.Add(m);
                m.Source = new System.Uri(answersPath[i], UriKind.Relative);
                m.Loaded += new RoutedEventHandler(initializeAnswerVideoProperties);
                //      Declare answer text block
                TextBlock tb = new TextBlock();
                dp.Children.Add(tb);
                tb.Text = answers[i];
                tb.Loaded += new RoutedEventHandler(initializeAnswerTextProperties);
            }
        }

        private void loadQuestion1() {
            svAnswers.Visibility = System.Windows.Visibility.Hidden;
            ListViewItem lvi = new ListViewItem();
            lvi.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            String programFlow = person.programFlow;
            person = new Person();
            person.programFlow = programFlow;
            lvi.Content = "Giriş";
            lvMenu.Items.Clear();
            lvMenu.Items.Add(lvi);
            I_Next.Visibility = System.Windows.Visibility.Visible;
            I_Prev.Visibility = System.Windows.Visibility.Visible;
            String question = "Nasıl Yardımcı Olabilirim?";
            String questionPath = "SignVideos/Questions/Question1/NasilYardimciOlabilirim.mp4";
            int answersCount = 2;
            String[] answers = new String[] { 
                "Hastayım", 
                "Bilgi Almak İstiyorum" };
            String[] answersPath = new String[] { 
                "SignVideos/Answers/AnswersToQuestion1/BenHastayim.mp4",
                "SignVideos/Answers/AnswersToQuestion1/BilgiAlmakIstiyorum.mp4" };
            loadQuestion(question, questionPath, answersCount, answers, answersPath);
        }

        private void loadQuestion2() {
            svAnswers.Visibility = System.Windows.Visibility.Hidden;

            //clear list view menu
            lvMenu.Items.Clear();
            ListViewItem lvi1 = new ListViewItem();
            lvi1.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi1.Content = "Giriş";
            lvMenu.Items.Add(lvi1);
            ListViewItem lvi2 = new ListViewItem();
            lvi2.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi2.Content = "Bilgi";
            lvMenu.Items.Add(lvi2);

            I_Next.Visibility = System.Windows.Visibility.Visible;
            I_Prev.Visibility = System.Windows.Visibility.Visible;
            String question = "Ne Bilgisi İstiyorsunuz?";
            String questionPath = "SignVideos/Questions/Question2/SizNeBilgisiIstiyorsunuz.mp4";
            int answersCount = 4;
            String[] answers = new String[] { 
                "Hasta Ziyareti İçin Geldim", 
                "Sigorta Bilgisi Almak İstiyorum",
                "Ücret Sormak İstiyorum",
                "Yer Sormak İstiyorum"};
            String[] answersPath = new String[] {  
                "SignVideos/Answers/AnswersToQuestion2/HastaZiyaretiIcinGeldim.mp4", 
                "SignVideos/Answers/AnswersToQuestion2/SigortaBilgisiAlmakIstiyorum.mp4", 
                "SignVideos/Answers/AnswersToQuestion2/UcretSormakIstiyorum.mp4",
                "SignVideos/Answers/AnswersToQuestion2/YerSormakIstiyorum.mp4"};
            loadQuestion(question, questionPath, answersCount, answers, answersPath);
        }

        private void loadQuestion3() {
            svAnswers.Visibility = System.Windows.Visibility.Hidden;

            //clear list view menu
            lvMenu.Items.Clear();
            ListViewItem lvi1 = new ListViewItem();
            lvi1.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi1.Content = "Giriş";
            lvMenu.Items.Add(lvi1);
            ListViewItem lvi2 = new ListViewItem();
            lvi2.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi2.Content = "Hasta";
            lvMenu.Items.Add(lvi2);
            I_Next.Visibility = System.Windows.Visibility.Visible;
            I_Prev.Visibility = System.Windows.Visibility.Visible;
            String question = "Acil mi?";
            String questionPath = "SignVideos/Questions/Question3/AcilMi.mp4";
            int answersCount = 2;
            String[] answers = new String[] { 
                "Evet, Acil", 
                "Hayır, Acil Değil"};
            String[] answersPath = new String[] {  
                "SignVideos/Answers/AnswersToQuestion3/EvetAcil.mp4", 
                "SignVideos/Answers/AnswersToQuestion3/HayirAcilDegil.mp4"};
            loadQuestion(question, questionPath, answersCount, answers, answersPath);
        }

        private void loadQuestion4Emergency()
        {
            svAnswers.Visibility = System.Windows.Visibility.Hidden;

            isInQuestion4 = true;

            //clear list view menu
            lvMenu.Items.Clear();
            ListViewItem lvi1 = new ListViewItem();
            lvi1.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi1.Content = "Giriş";
            lvMenu.Items.Add(lvi1);
            ListViewItem lvi2 = new ListViewItem();
            lvi2.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi2.Content = "Hasta";
            lvMenu.Items.Add(lvi2);
            ListViewItem lvi3 = new ListViewItem();
            lvi3.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi3.Content = "Acil";
            lvMenu.Items.Add(lvi3);
            I_Next.Visibility = System.Windows.Visibility.Visible;
            I_Prev.Visibility = System.Windows.Visibility.Visible;
            String question = "Şikayetiniz Nedir?";
            String questionPath = "SignVideos/Questions/Question4/SizinSikayetinizNedir.mp4";
            int answersCount = 9;
            String[] answers = new String[] { 
                "Kusuyorum", 
                "Nefes Darlığı Yaşıyorum",
                "Başım Dönüyor",
                "Karnım Ağrıyor",
                "Kalp Çarpıntım Var",
                "Göğsüm Ağrıyor",
                "Bayılıyorum",
                "Başım Ağrıyor",
                "Ateşim Var"};
            String[] answersPath = new String[] {  
                "SignVideos/Answers/AnswersToQuestion4/Emergency/Kusmak.mp4", 
                "SignVideos/Answers/AnswersToQuestion4/Emergency/NefesDarligi.mp4",
                "SignVideos/Answers/AnswersToQuestion4/Emergency/BenimBasimDonuyor.mp4", 
                "SignVideos/Answers/AnswersToQuestion4/Emergency/KarnimAgriyor.mp4", 
                "SignVideos/Answers/AnswersToQuestion4/Emergency/KalpCarpintisi.mp4", 
                "SignVideos/Answers/AnswersToQuestion4/Emergency/GogusAgrisi.mp4", 
                "SignVideos/Answers/AnswersToQuestion4/Emergency/Bayilmak.mp4",
                "SignVideos/Answers/AnswersToQuestion4/Emergency/BasAgrisi.mp4",
                "SignVideos/Answers/AnswersToQuestion4/Emergency/AtesiOlmak.mp4"};
            loadQuestion(question, questionPath, answersCount, answers, answersPath);
        }
        private void loadQuestion4NotEmergency() {
            svAnswers.Visibility = System.Windows.Visibility.Hidden;

            isInQuestion4 = true;
            //clear list view menu
            lvMenu.Items.Clear();
            ListViewItem lvi1 = new ListViewItem();
            lvi1.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi1.Content = "Giriş";
            lvMenu.Items.Add(lvi1);
            ListViewItem lvi2 = new ListViewItem();
            lvi2.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi2.Content = "Hasta";
            lvMenu.Items.Add(lvi2);
            ListViewItem lvi3 = new ListViewItem();
            lvi3.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi3.Content = "Acil Değil";
            lvMenu.Items.Add(lvi3);
            I_Next.Visibility = System.Windows.Visibility.Visible;
            I_Prev.Visibility = System.Windows.Visibility.Visible;

            String question = "Şikayetiniz Nedir?";
            String questionPath = "SignVideos/Questions/Question4/SizinSikayetinizNedir.mp4";
            int answersCount = 14;
            String[] answers = new String[] { 
                "Sırtım Ağrıyor",
                "Belim Ağrıyor",
                "Öksürüyorum",
                "Boğazım Şişti",
                "Aşı Yaptırmak İstiyorum",
                "Uyuyamıyorum",
                "Sinirliyim",
                "Burun Akıntım Var",
                "Adet Düzensizliği Yaşıyorum",
                "İlaç Yazdırmak İstiyorum",
                "Kan Tahlili Yaptırmak İstiyorum",
                "Tuvalete Çıkamıyorum",
                "İdrar Tahlili Yaptırmak İstiyorum",
                "İshalim"};
            String[] answersPath = new String[] {  
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/SirtimAgriyor.mp4", 
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/BelimAgriyor.mp4", 
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/Oksurmek.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/BogazimSisti.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/BenAsiYaptirmakIstiyorum.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/Uyuyamiyorum.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/BenCokSinirliyim.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/BurnumAkiyor.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/AdetDuzensizligi.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/IlacYazdirmakIstiyorum.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/KanTahliliYaptirmakIstiyorum.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/TuvaleteCikamiyorum.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/IdrarTahliliYaptirmakIstiyorum.mp4",
                "SignVideos/Answers/AnswersToQuestion4/NotEmergency/Ishalim.mp4"};
            loadQuestion(question, questionPath, answersCount, answers, answersPath);
        }

        private void loadQuestion5() {
            svAnswers.Visibility = System.Windows.Visibility.Hidden;

            isInQuestion4 = false;
            //clear list view menu
            lvMenu.Items.Clear();
            ListViewItem lvi1 = new ListViewItem();
            lvi1.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi1.Content = "Giriş";
            lvMenu.Items.Add(lvi1);
            ListViewItem lvi2 = new ListViewItem();
            lvi2.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi2.Content = "Hasta";
            lvMenu.Items.Add(lvi2);
            ListViewItem lvi3 = new ListViewItem();
            lvi3.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi3.Content = "Acil Değil";
            lvMenu.Items.Add(lvi3);
            ListViewItem lvi4 = new ListViewItem();
            lvi4.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi4.Content = person.grievance;
            lvMenu.Items.Add(lvi4);

            I_Next.Visibility = System.Windows.Visibility.Visible;
            I_Prev.Visibility = System.Windows.Visibility.Visible;
            String question = "Randevunuz Var mı?";
            String questionPath = "SignVideos/Questions/Question5/SizinRandevunuzVarMi.mp4";
            int answersCount = 2;
            String[] answers = new String[] { 
                "Evet, Var", 
                "Hayır, Yok"};
            String[] answersPath = new String[] {  
                "SignVideos/Answers/AnswersToQuestion5/EvetVar.mp4", 
                "SignVideos/Answers/AnswersToQuestion5/HayirYok.mp4"};
            loadQuestion(question, questionPath, answersCount, answers, answersPath);
        }

        private void loadDirection1(){
            //clear list view menu
            lvMenu.Items.Clear();
            ListViewItem lvi1 = new ListViewItem();
            lvi1.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi1.Content = "Giriş";
            lvMenu.Items.Add(lvi1);
            ListViewItem lvi2 = new ListViewItem();
            lvi2.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi2.Content = "Bilgi";
            lvMenu.Items.Add(lvi2);
        
            TBQuestion.Text = "Hastanın İsmini Yazınız";
            meMainVideo.Source = new System.Uri("SignVideos/Directions/HastaninIsminiYaziniz.mp4", UriKind.Relative);
            isRepeated = true;
            svAnswers.Visibility = System.Windows.Visibility.Hidden;
            I_Next.Visibility = System.Windows.Visibility.Hidden;
            I_Prev.Visibility = System.Windows.Visibility.Hidden;
            spInput.Visibility = System.Windows.Visibility.Visible;
            bOtherName.Visibility = System.Windows.Visibility.Visible;
            restartVideo();
        }
        private void loadDirection2()
        {
            TBQuestion.Text = "Teşekkürler";
            meMainVideo.Source = new System.Uri("SignVideos/Directions/LutfenKimliginizleDanismayaKayit.mp4", UriKind.Relative);
            isRepeated = true;
            svAnswers.Visibility = System.Windows.Visibility.Hidden;
            I_Next.Visibility = System.Windows.Visibility.Hidden;
            I_Prev.Visibility = System.Windows.Visibility.Hidden;
            restartVideo();

            //Closing
            
            bClose.Visibility = System.Windows.Visibility.Visible;

        }

        private void loadDirection3()
        {
            isInQuestion4 = false;
            //clear list view menu
            lvMenu.Items.Clear();
            ListViewItem lvi1 = new ListViewItem();
            lvi1.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi1.Content = "Giriş";
            lvMenu.Items.Add(lvi1);
            ListViewItem lvi2 = new ListViewItem();
            lvi2.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi2.Content = "Hasta";
            lvMenu.Items.Add(lvi2);
            ListViewItem lvi3 = new ListViewItem();
            lvi3.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            ListViewItem lvi4 = new ListViewItem();
            ListViewItem lvi5 = new ListViewItem();


            if (person.isEmergency)
            {
                lvi3.Content = "Acil";
                lvMenu.Items.Add(lvi3);
                lvi4.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
                lvi4.Content = person.grievance;
                lvMenu.Items.Add(lvi4);
            }
            else {
                lvi3.Content = "Acil Değil";
                lvMenu.Items.Add(lvi3);
                lvi4.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
                lvi4.Content = person.grievance;
                lvMenu.Items.Add(lvi4);
                lvi5.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
                if (person.hasAppointment)
                {
                    lvi5.Content = "Randevu Var";
                }
                else {
                    lvi5.Content = "Randevu Yok";
                }
                lvMenu.Items.Add(lvi5);

            }
          

            TBQuestion.Text = "Lütfen İsminizi Yazınız";
            meMainVideo.Source = new System.Uri("SignVideos/Directions/LutfenSizIsminiziYaziniz.mp4", UriKind.Relative);
            isRepeated = true;
            svAnswers.Visibility = System.Windows.Visibility.Hidden;
            I_Next.Visibility = System.Windows.Visibility.Hidden;
            I_Prev.Visibility = System.Windows.Visibility.Hidden;
            spInput.Visibility = System.Windows.Visibility.Visible;
            bName.Visibility = System.Windows.Visibility.Visible;

            restartVideo();
        }

        private void bOtherName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            person.otherPatientName = tbName.Text;
            person.programFlow = person.programFlow + "Ziyaret etmek istediği hastanın ismi: " + tbName.Text + "\n";
            System.Diagnostics.Debug.WriteLine(person.programFlow);
            ListViewItem lvi = new ListViewItem();
            lvMenu.Items.Add(lvi);
            lvi.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi.Content = "Hasta İsmi";

            loadDirection2();
            bOtherName.Visibility = System.Windows.Visibility.Hidden;
            spInput.Visibility = System.Windows.Visibility.Hidden;


        }

        private void bName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            person.name = tbName.Text;
            person.programFlow = person.programFlow + "Hastanın ismi: " + tbName.Text +"\n";
            System.Diagnostics.Debug.WriteLine(person.programFlow);
            ListViewItem lvi = new ListViewItem();
            lvMenu.Items.Add(lvi);
            lvi.PreviewMouseDown += new MouseButtonEventHandler(listViewItemMouseDown);
            lvi.Content = "İsim";

            loadDirection2();
            bName.Visibility = System.Windows.Visibility.Hidden;
            spInput.Visibility = System.Windows.Visibility.Hidden;

        }

        private void bClose_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            person.printOut();
            Application.Current.Shutdown();
        }

        private void I_Prev_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (I_Prev.Visibility.Equals(System.Windows.Visibility.Visible))
            {
                spInput.Visibility = System.Windows.Visibility.Hidden;
                bClose.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void IDropdown_ico_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lvMenu.Visibility.Equals(System.Windows.Visibility.Visible))
            {
                lvMenu.Visibility = System.Windows.Visibility.Hidden;
            }
            else {
                lvMenu.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }

    public class Person
    {
        public string name;
        public Boolean isSick;
        public Boolean isSeekingInfo;
        public Boolean isEmergency;
        public Boolean hasAppointment;
        public string otherPatientName;
        public string grievance;
        public string soughtInfo;
        public string programFlow;

        public Person()
        {
            this.name = "";
            this.otherPatientName = "";
            this.grievance = "";
            this.soughtInfo = "";
            this.programFlow = "";
            this.isEmergency = false;
            this.isSeekingInfo = false;
            this.isSick = false;
            this.hasAppointment = false;
        }

        public void printOut() {
            String fileName = string.Format(@"{0}.txt", Guid.NewGuid());
            String filePath = "C:/Users/hilal/Desktop/Printouts/"+fileName;
            if (System.IO.File.Exists(filePath) == true)
            {
                FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                StreamWriter objWrite = new StreamWriter(fs);
                objWrite.WriteLine("Kisi Bilgileri:");
                if (this.isSeekingInfo) {
                    objWrite.WriteLine("Bu kisi bilgi istiyor.");
                    objWrite.WriteLine("Istedigi bilgi: "+this.soughtInfo);
                    if (this.soughtInfo.Equals("Hasta Ziyareti İçin Geldim"))
                    {
                        objWrite.WriteLine("Ziyaret etmek istedigi hastanin ismi: " + this.otherPatientName);
                    }

                }
                else if (this.isSick) {
                    objWrite.WriteLine("Bu kisi hasta.");
                    if (this.isEmergency)
                    {
                        objWrite.WriteLine("Durumu acil.");
                    }
                    else {
                        objWrite.WriteLine("Durumu acil degil.");
                    }
                    objWrite.WriteLine("Sikayeti: "+this.grievance);
                    if (!this.isEmergency) {
                        if (this.hasAppointment)
                        {
                            objWrite.WriteLine("Rendevusu var.");
                        }
                        else {
                            objWrite.WriteLine("Randevusu yok.");
                        }
                    }
                    objWrite.WriteLine("Hastanin ismi: "+this.name);


                }
                objWrite.WriteLine("Program Akisi:");
                objWrite.Write(this.programFlow);
                objWrite.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                StreamWriter objWrite = new StreamWriter(fs);
                objWrite.WriteLine("Kisi Bilgileri:");
                if (this.isSeekingInfo)
                {
                    objWrite.WriteLine("Bu kisi bilgi istiyor.");
                    objWrite.WriteLine("Istedigi bilgi: " + this.soughtInfo);
                    if (this.soughtInfo.Equals("Hasta Ziyareti İçin Geldim"))
                    {
                        objWrite.WriteLine("Ziyaret etmek istedigi hastanin ismi: " + this.otherPatientName);
                    }

                }
                else if (this.isSick)
                {
                    objWrite.WriteLine("Bu kisi hasta.");
                    if (this.isEmergency)
                    {
                        objWrite.WriteLine("Durumu acil.");
                    }
                    else
                    {
                        objWrite.WriteLine("Durumu acil degil.");
                    }
                    objWrite.WriteLine("Sikayeti: " + this.grievance);
                    if (!this.isEmergency)
                    {
                        if (this.hasAppointment)
                        {
                            objWrite.WriteLine("Rendevusu var.");
                        }
                        else
                        {
                            objWrite.WriteLine("Randevusu yok.");
                        }
                    }
                    objWrite.WriteLine("Hastanin ismi: " + this.name);


                }
                objWrite.WriteLine("Program Akisi:");
                objWrite.Write(this.programFlow);
                objWrite.Close();
            }
        }
    }
}
