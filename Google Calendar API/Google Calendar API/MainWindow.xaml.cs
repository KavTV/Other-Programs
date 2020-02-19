﻿using System;
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


namespace Google_Calendar_API
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RefreshHomework();

        }
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
                    //Add event to list.
                    if (eventItem.Description == "Lektie")
                    {
                        homeworkList.Items.Add(eventItem.Summary);
                        homeworkListDate.Text +=   date[0] + Environment.NewLine;
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

            ev.Start = start;
            ev.End = end;
            ev.Summary = addDescription.Text;
            ev.Description = "Lektie";
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

            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {

                    //Add event to list.
                    if (eventItem.Description == "Lektie")
                    {
                        if (eventItem.Summary == summaryID)
                        {
                            calendarService.Events.Delete("primary", eventItem.Id).Execute();
                        }
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

        private void homeworkList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            //MenuItemDelete_Click(sender, e);
        }

    }
}
