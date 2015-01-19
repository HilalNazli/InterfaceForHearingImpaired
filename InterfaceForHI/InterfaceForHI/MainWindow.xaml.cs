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
        String RESULT = "";
        double ratio = 0;
        Boolean isRepeated = false;
        Boolean isSvAnswersVisible = false;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        double svAnswersHorizontalOffset = 0;
        public MainWindow()
        {
            InitializeComponent();
            mainWindow.WindowState = WindowState.Maximized;
            ratio = System.Windows.SystemParameters.PrimaryScreenHeight/mainWindow.Height;
            
            mainWindow.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            mainWindow.Height = System.Windows.SystemParameters.PrimaryScreenHeight;


            loadQuestion1();
            initializeSizes(ratio);
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
            mainWindow.meMainVideo.Height = meMainVideo.Height * ratio;
            //Resize margins

        }


        private void initializeAnswerVideoProperties(object sender, RoutedEventArgs args) {
            MediaElement me = (MediaElement)args.Source;
            DockPanel.SetDock(me, Dock.Top);
            me.Height = 200 * ratio;
            me.LoadedBehavior = System.Windows.Controls.MediaState.Manual;
            me.MediaEnded += new RoutedEventHandler(AnswerMediaEnded);
            me.MouseLeftButtonDown += new MouseButtonEventHandler(AnswersToQuestion1MediaMouseLeftButtonDown);
            me.Stretch = Stretch.Uniform;

        }
        private void initializeAnswerTextProperties(object sender, RoutedEventArgs args)
        {
            TextBlock tb = (TextBlock)args.Source;
            tb.FontSize = 24;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            //Calisiyor mu?
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
        private void AnswersToQuestion1MediaMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            MediaElement me = (MediaElement)args.Source;
            DockPanel dp = new DockPanel();
            dp = (DockPanel)me.Parent;
            TextBlock tb = new TextBlock();
            tb = dp.Children.OfType<TextBlock>().FirstOrDefault();
            if (tb.Text.Equals("Hastayım"))
            {
                RESULT = RESULT + "Bu kişi hastadır.\n";
                //loadQuestion3
            }
            else if (tb.Text.Equals("Bilgi Almak İstiyorum"))
            {
                RESULT = RESULT + "Bu kişi bilgi almak istiyor.\n";
                //loadQuestion2
            }
            System.Diagnostics.Debug.WriteLine(RESULT);


        }

        private void meMainVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (!isRepeated)
            {
                mainWindow.svAnswers.Visibility = System.Windows.Visibility.Visible;
                isSvAnswersVisible = true;
                playAllChildren(spAnswers);
            }
           
            restartVideo();
        }
        public static void playAllChildren(StackPanel stackPanel)
        {

            //System.Collections.Generic.List<MediaElement> meAnswers = new System.Collections.Generic.List<MediaElement>();

            foreach(Grid g in stackPanel.Children){
                //System.Diagnostics.Debug.WriteLine("Grid found");
                DockPanel d = g.Children.OfType<DockPanel>().FirstOrDefault();
                MediaElement me = d.Children.OfType<MediaElement>().FirstOrDefault();
                me.Play();
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
        }

        private void I_Prev_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            svAnswers.LineLeft();
           

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
            if (isSvAnswersVisible) { 
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
                dispatcherTimer.Start();
            }
        }











        //Load Question1
            //isRepeated=false;
            //spAnswers.Children.Clear();
        //Load AnswersToQuestion1
        //Grid g = new Grid();
        //MediaElement m = new MediaElement();
        //m.Source = new System.Uri("SignVideos/BasAgrisi.mp4",UriKind.Relative);
        //m.Loaded += new RoutedEventHandler(initializeAnswerProperties);
        //g.Children.Add(m);
        //spAnswers.Children.Add(g);
        private void loadQuestion1() { 
            //Set the question in the main video
            TBQuestion.Text = "Nasıl Yardımcı Olabilirim?";
            meMainVideo.Source = new System.Uri("SignVideos/Questions/Question1/NasilYardimciOlabilirim.mp4",UriKind.Relative);
            isRepeated = false;
            restartVideo();
            //Clear the stack panel for the answers
            spAnswers.Children.Clear();
            //Fill the stack panel with answers
            //ANSWER1
            //  Declare Answer1ToQuestion1 grid
            Grid gAnswer1ToQuestion1 = new Grid();
            spAnswers.Children.Add(gAnswer1ToQuestion1);
            gAnswer1ToQuestion1.Loaded += new RoutedEventHandler(initializeAnswerGridProperties);
            //    Declare Answer1ToQuestion1 dock
            DockPanel dpAnswer1ToQuestion1 = new DockPanel();
            gAnswer1ToQuestion1.Children.Add(dpAnswer1ToQuestion1);
            dpAnswer1ToQuestion1.Loaded += new RoutedEventHandler(initializeAnswerDockPanelProperties);
            //      Declare Answer1ToQuestion1 media element
            MediaElement mAnswer1ToQuestion1 = new MediaElement();
            dpAnswer1ToQuestion1.Children.Add(mAnswer1ToQuestion1);
            mAnswer1ToQuestion1.Source = new System.Uri("SignVideos/Answers/AnswersToQuestion1/BenHastayim.mp4", UriKind.Relative);
            mAnswer1ToQuestion1.Loaded += new RoutedEventHandler(initializeAnswerVideoProperties);
            //      Declare Answer1ToQuestion1 text block
            TextBlock tbAnswer1ToQuestion1 = new TextBlock();
            dpAnswer1ToQuestion1.Children.Add(tbAnswer1ToQuestion1);
            tbAnswer1ToQuestion1.Text = "Hastayım";
            tbAnswer1ToQuestion1.Loaded += new RoutedEventHandler(initializeAnswerTextProperties);
            //ANSWER2
            //  Declare Answer2ToQuestion grid
            Grid gAnswer2ToQuestion1 = new Grid();
            spAnswers.Children.Add(gAnswer2ToQuestion1);
            gAnswer2ToQuestion1.Loaded += new RoutedEventHandler(initializeAnswerGridProperties);
            //    Declare Answer2ToQuestion1 dock
            DockPanel dpAnswer2ToQuestion1 = new DockPanel();
            gAnswer2ToQuestion1.Children.Add(dpAnswer2ToQuestion1);
            dpAnswer2ToQuestion1.Loaded += new RoutedEventHandler(initializeAnswerDockPanelProperties);
            //      Declare Answer2ToQuestion1 media element
            MediaElement mAnswer2ToQuestion1 = new MediaElement();
            dpAnswer2ToQuestion1.Children.Add(mAnswer2ToQuestion1);
            mAnswer2ToQuestion1.Source = new System.Uri("SignVideos/Answers/AnswersToQuestion1/BilgiAlmakIstiyorum.mp4", UriKind.Relative);
            mAnswer2ToQuestion1.Loaded += new RoutedEventHandler(initializeAnswerVideoProperties);
            //      Declare Answer2ToQuestion1 text block
            TextBlock tbAnswer2ToQuestion1 = new TextBlock();
            dpAnswer2ToQuestion1.Children.Add(tbAnswer2ToQuestion1);
            tbAnswer2ToQuestion1.Text = "Bilgi Almak İstiyorum";
            tbAnswer2ToQuestion1.Loaded += new RoutedEventHandler(initializeAnswerTextProperties);
        }





        //Load Question2
        //Load AnswersToQuestion2
        //Load Question3
        //Load AnswersToQuestion3
        //Load Question4
        //Load AnswersToQuestion4
        //Load Direction1



    }
}
