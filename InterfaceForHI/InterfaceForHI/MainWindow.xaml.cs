using System;
using System.Collections.Generic;
using System.IO;
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
        Person person = new Person();
        double ratio = 0;
        Boolean isNotEmergency = false;
        Boolean isRepeated = false;
        Boolean isSvAnswersVisible = false;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer dispatcherTimer2 = new System.Windows.Threading.DispatcherTimer();

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

            double heightOfTBName = 48;
            double widthOfTbName = 480;
            double fontOfTBName = 24;

            double heightOfBName = 48;
            double widthOfBName = 100;
            double fontOfBName = 24;

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

            tbName.Height = heightOfTBName * ratio;
            tbName.Width = widthOfTbName * ratio;
            tbName.FontSize = fontOfTBName * ratio;

            bName.Height = heightOfBName * ratio;
            bName.Width = widthOfBName * ratio;
            bName.FontSize = fontOfBName * ratio;
            bOtherName.Height = heightOfBName * ratio;
            bOtherName.Width = widthOfBName * ratio;
            bOtherName.FontSize = fontOfBName * ratio;

        }


        private void initializeAnswerVideoProperties(object sender, RoutedEventArgs args) {
            MediaElement me = (MediaElement)args.Source;
            DockPanel.SetDock(me, Dock.Top);
            me.Height = 240 * ratio;

            me.LoadedBehavior = System.Windows.Controls.MediaState.Manual;
            me.MediaEnded += new RoutedEventHandler(AnswerMediaEnded);
            me.MouseLeftButtonDown += new MouseButtonEventHandler(AnswerMediaMouseLeftButtonDown);
            me.Stretch = Stretch.Uniform;

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
                if (isNotEmergency) {
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
           
            //System.Diagnostics.Debug.WriteLine(svAnswers.HorizontalOffset);
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

        private void loadQuestion(String question, String questionPath, int answersCount, String[] answers, String[] answersPath) {
            //Set the question in the main video
            TBQuestion.Text = question;
            meMainVideo.Source = new System.Uri(questionPath, UriKind.Relative);
            isRepeated = false;
            svAnswers.Visibility = System.Windows.Visibility.Hidden;
            isSvAnswersVisible = false;
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
            I_Next.Visibility = System.Windows.Visibility.Visible;
            I_Prev.Visibility = System.Windows.Visibility.Visible;
            String question = "Acil Mi?";
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
            I_Next.Visibility = System.Windows.Visibility.Visible;
            I_Prev.Visibility = System.Windows.Visibility.Visible;
            isNotEmergency = true;
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
            I_Next.Visibility = System.Windows.Visibility.Visible;
            I_Prev.Visibility = System.Windows.Visibility.Visible;
            String question = "Randevunuz Var Mı?";
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
            TBQuestion.Text = "Hastanın İsmini Yazınız";
            meMainVideo.Source = new System.Uri("SignVideos/Directions/HastaninIsminiYaziniz.mp4", UriKind.Relative);
            isRepeated = true;
            svAnswers.Visibility = System.Windows.Visibility.Hidden;
            I_Next.Visibility = System.Windows.Visibility.Hidden;
            I_Prev.Visibility = System.Windows.Visibility.Hidden;
            spInput.Visibility = System.Windows.Visibility.Visible;
            bOtherName.Visibility = System.Windows.Visibility.Visible;
            isSvAnswersVisible = false;
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
            isSvAnswersVisible = false;
            restartVideo();

            //Closing
            //Printuot function of person
            person.printOut();
            //timer
            dispatcherTimer2.Tick += new EventHandler(dispatcherTimer2_Tick);
            dispatcherTimer2.Interval = new TimeSpan(0, 0, 16);
            dispatcherTimer2.Start();

        }

        private void dispatcherTimer2_Tick(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void loadDirection3()
        {
            TBQuestion.Text = "Lütfen İsminizi Yazınız";
            meMainVideo.Source = new System.Uri("SignVideos/Directions/LutfenSizIsminiziYaziniz.mp4", UriKind.Relative);
            isRepeated = true;
            svAnswers.Visibility = System.Windows.Visibility.Hidden;
            I_Next.Visibility = System.Windows.Visibility.Hidden;
            I_Prev.Visibility = System.Windows.Visibility.Hidden;
            spInput.Visibility = System.Windows.Visibility.Visible;
            bName.Visibility = System.Windows.Visibility.Visible;

            isSvAnswersVisible = false;
            restartVideo();
        }

        private void bOtherName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            person.otherPatientName = tbName.Text;
            person.programFlow = person.programFlow + "Ziyaret etmek istediği hastanın ismi: " + tbName.Text + "\n";
            System.Diagnostics.Debug.WriteLine(person.programFlow);
            loadDirection2();
            bOtherName.Visibility = System.Windows.Visibility.Hidden;
            spInput.Visibility = System.Windows.Visibility.Hidden;


        }

        private void bName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            person.name = tbName.Text;
            person.programFlow = person.programFlow + "Hastanın ismi: " + tbName.Text +"\n";
            System.Diagnostics.Debug.WriteLine(person.programFlow);
            loadDirection2();
            bName.Visibility = System.Windows.Visibility.Hidden;
            spInput.Visibility = System.Windows.Visibility.Hidden;

        }
    }

    public class Person
    {
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
                objWrite.Write(this.programFlow);
                objWrite.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                StreamWriter objWrite = new StreamWriter(fs);
                objWrite.Write(this.programFlow);
                objWrite.Close();
            }
        }

        public string name;
        public Boolean isSick;
        public Boolean isSeekingInfo;
        public Boolean isEmergency;
        public Boolean hasAppointment;
        public string otherPatientName;
        public string grievance;
        public string soughtInfo;
        public string programFlow;
    }
}
