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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Google_Calendar_API
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static public string HomeworkColors = Environment.CurrentDirectory + @"/SavedColors.txt";
        Color danishBrushColor;
        Color englishBrushColor;
        Color mathBrushColor;
        Color physicsBrushColor;

        #region get/set
        public Color DanishBrushColor { get => danishBrushColor; set => danishBrushColor = value; }
        public Color EnglishBrushColor { get => englishBrushColor; set => englishBrushColor = value; }
        public Color MathBrushColor { get => mathBrushColor; set => mathBrushColor = value; }
        public Color PhysicsBrushColor { get => physicsBrushColor; set => physicsBrushColor = value; }
        #endregion

        public MainWindow(int j) {
            string read = "";
            StreamReader sr = new StreamReader(HomeworkColors);
            while (!sr.EndOfStream)
            {
                read = sr.ReadLine();
            }

            sr.Close();
            char[] SplitSymbols = { ' ', ':' };
            string[] DecideSubjectColor = read.Split(SplitSymbols);

            for (int i = 0; i < DecideSubjectColor.Length; i++)
            {
                if (DecideSubjectColor[i] == "Danish")
                {
                    danishBrushColor = (Color)ColorConverter.ConvertFromString(DecideSubjectColor[i + 1]);
                }
                else if (DecideSubjectColor[i] == "English")
                {
                    englishBrushColor = (Color)ColorConverter.ConvertFromString(DecideSubjectColor[i + 1]);
                }
                else if (DecideSubjectColor[i] == "Math")
                {
                    mathBrushColor = (Color)ColorConverter.ConvertFromString(DecideSubjectColor[i + 1]);
                }
                else if (DecideSubjectColor[i] == "Physics")
                {
                    physicsBrushColor = (Color)ColorConverter.ConvertFromString(DecideSubjectColor[i + 1]);
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            RefreshHomework();

        }
        RadioButton CheckedRadio = new RadioButton();

        public CalendarService ConnectToService()
        {
            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/calendar-dotnet-quickstart.json
            string[] Scopes = { CalendarService.Scope.CalendarEvents };
            string ApplicationName = "Homework adder";


            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }
        public void RefreshHomework()
        {
            var service = ConnectToService();
            homeworkList.Items.Clear();
            homeworkListDate.Text = "";

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 20;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.

            Events events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    string[] date = when.Split(" ");
                    string[] DayAndMonth = date[0].Split("-");

                    string desc = eventItem.Description;
                    if (desc != null)
                    {
                        string[] SubjectSplit = desc.Split(":");





                        //Add event to list.
                        if (SubjectSplit[0] == "Lektie")
                        {

                            if (SubjectSplit.Length >= 2)
                            {
                                if (!File.Exists(HomeworkColors))
                                {
                                    using (FileStream fs = File.Create(HomeworkColors)) ;
                                    StreamWriter sw = new StreamWriter(HomeworkColors);

                                    sw.WriteLine("Danish #FFCD5C5C:" + "English #FF5F9EA0:" + "Math #FF228B22:" + "Physics #FF7CFC00:");
                                    //sw.WriteLine("#FF5F9EA0");
                                    //sw.WriteLine("#FF228B22");
                                    //sw.WriteLine("#FF7CFC00");

                                    sw.Close();

                                }
                                string read = "";
                                StreamReader sr = new StreamReader(HomeworkColors);
                                while (!sr.EndOfStream)
                                {
                                    read = sr.ReadLine();
                                }

                                sr.Close();

                                char[] SplitSymbols = { ' ', ':' };
                                string[] DecideSubjectColor = read.Split(SplitSymbols);

                                for (int i = 0; i < DecideSubjectColor.Length; i++)
                                {
                                    if (DecideSubjectColor[i] == "Danish")
                                    {
                                        danishBrushColor = (Color)ColorConverter.ConvertFromString(DecideSubjectColor[i + 1]);
                                    }
                                    else if (DecideSubjectColor[i] == "English")
                                    {
                                        englishBrushColor = (Color)ColorConverter.ConvertFromString(DecideSubjectColor[i + 1]);
                                    }
                                    else if (DecideSubjectColor[i] == "Math")
                                    {
                                        mathBrushColor = (Color)ColorConverter.ConvertFromString(DecideSubjectColor[i + 1]);
                                    }
                                    else if (DecideSubjectColor[i] == "Physics")
                                    {
                                        physicsBrushColor = (Color)ColorConverter.ConvertFromString(DecideSubjectColor[i + 1]);
                                    }
                                }

                                if (SubjectSplit[1] == "Dansk")
                                {
                                    SolidColorBrush brush = new SolidColorBrush(danishBrushColor);
                                    homeworkList.Items.Add(new ListBoxItem { Content = "Dansk: " + eventItem.Summary, Background = brush });
                                }
                                else if (SubjectSplit[1] == "Engelsk")
                                {
                                    SolidColorBrush brush = new SolidColorBrush(englishBrushColor);
                                    homeworkList.Items.Add(new ListBoxItem { Content = "Engelsk: " + eventItem.Summary, Background = brush });
                                }
                                else if (SubjectSplit[1] == "Matematik")
                                {
                                    SolidColorBrush brush = new SolidColorBrush(mathBrushColor);
                                    homeworkList.Items.Add(new ListBoxItem { Content = "Matematik: " + eventItem.Summary, Background = brush });
                                }
                                else if (SubjectSplit[1] == "Fysik")
                                {
                                    SolidColorBrush brush = new SolidColorBrush(physicsBrushColor);
                                    homeworkList.Items.Add(new ListBoxItem { Content = "Fysik: " + eventItem.Summary, Background = brush });
                                }
                            }
                            else
                            {
                                homeworkList.Items.Add(new ListBoxItem { Content = eventItem.Summary, Background = Brushes.Gray });
                            }
                            DateTime test = new DateTime(Int32.Parse(DayAndMonth[2]), Int32.Parse(DayAndMonth[1]), Int32.Parse(DayAndMonth[0]));
                            int DaysRemain = (test - DateTime.Today).Days;
                            if (homeworkListDate.Text == "")
                            {
                                homeworkListDate.Text += DayAndMonth[0] + "-" + DayAndMonth[1] + "   Dage tilbage: " + DaysRemain;
                            }
                            else
                            {
                                homeworkListDate.Text += Environment.NewLine + DayAndMonth[0] + "-" + DayAndMonth[1] + "   Dage tilbage: " + DaysRemain ;
                            }
                        }

                    }

                }
            }

            else
            {
                Console.WriteLine("No upcoming events found.");
                homeworkList.Items.Add("Ingen lektier!");
            }
            if (homeworkList.Items.Count < 1)
            {
                homeworkList.Items.Add("Ingen lektier!");
            }

        }




        private void addHomeworkBTN_Click(object sender, RoutedEventArgs e)
        {
            var calendarService = ConnectToService();
            var ev = new Event();
            int year = addDate.SelectedDate.Value.Year;
            int month = addDate.SelectedDate.Value.Month;
            int day = addDate.SelectedDate.Value.Day;


            EventDateTime start = new EventDateTime();
            start.DateTime = new DateTime(year, month, day);

            EventDateTime end = new EventDateTime();
            try
            {
                end.DateTime = new DateTime(year, month, day + 1);

            }
            catch (Exception)
            {
                end.DateTime = new DateTime(year, month + 1, 1);
            }
            string subject = "";
            if (CheckedRadio == RadioDanish)
            {
                subject = "Dansk";
            }
            else if (CheckedRadio == RadioEnglish)
            {
                subject = "Engelsk";
            }
            if (CheckedRadio == RadioMath)
            {
                subject = "Matematik";
            }
            if (CheckedRadio == RadioPhysics)
            {
                subject = "Fysik";
            }



            ev.Start = start;
            ev.End = end;
            ev.Summary = addDescription.Text;
            ev.Description = "Lektie:" + subject;

            try
            {
                Event recurringEvent = calendarService.Events.Insert(ev, "primary").Execute();
                addStatusLabel.Content = "Event tilføjet!";
                ResetAddHomework();
                RefreshHomework();
            }
            catch (Exception E)
            {
                addStatusLabel.Content = E;
            }

        }

        private void ResetAddHomework()
        {
            addDate.Text = "";
            addDescription.Text = "";
        }

        private void DeleteAHomework(string summaryID)
        {
            var calendarService = ConnectToService();

            EventsResource.ListRequest request = calendarService.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 20;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            //List events.
            Events events = request.Execute();
            string[] summarySplit = summaryID.Split(": ");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {

                    //Add event to list.

                    if (eventItem.Summary == summarySplit[2])
                    {
                        calendarService.Events.Delete("primary", eventItem.Id).Execute();
                    }

                }
            }


        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {

            if (homeworkList.SelectedIndex == -1)
            {
                return;
            }
            string summaryID = homeworkList.SelectedValue.ToString();
            DeleteAHomework(summaryID);
            RefreshHomework();


        }


        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton ck = sender as RadioButton;
            if (ck.IsChecked.Value)
                CheckedRadio = ck;
        }

        private void SettingsBTN_Click(object sender, RoutedEventArgs e)
        {
            ColorPicker win = new ColorPicker();
            win.Show();
        }

        private void SyncScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var textToSync = (sender == homeworkListScroll) ? TxtBlockScroll : homeworkListScroll;

            textToSync.ScrollToVerticalOffset(e.VerticalOffset);
            textToSync.ScrollToHorizontalOffset(e.HorizontalOffset);
            //if (homeworkListScroll != TxtBlockScroll )
            //{
            //    textToSync.ScrollToVerticalOffset(homeworkListScroll.VerticalOffset);
            //}
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            //Program doesent close properly without this
            Environment.Exit(0);
        }
    }
}




