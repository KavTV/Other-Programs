using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using OfficeOpenXml; //EPPlus library
using System.Diagnostics;

namespace Find_praktiksted
{
    class Program
    {
        static string fileLocation = @"C:\Users\kasp609g\Documents\GitHub\Other-Programs\Find praktiksted\Find praktiksted\bin\Debug\netcoreapp3.1\søgeresultat.xlsx";
        static string sentEmailsLocation = Path.Combine(Environment.CurrentDirectory, "SentEmails.txt");
        static ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileLocation));
        static List<Company> companiesList;

        static void Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            companiesList = GetCompanyList();
            while (true)
            {
                //Clear all previous information from console
                Console.Clear();

                Company company = GetCompany();

                //if there is no students, no need to show that information
                if (company.Students > 0)
                {
                    Console.WriteLine($"{company.Name}\nAdresse: {company.Adress} {company.Postal}\nTelefon: {company.Phone}\nEmail: {company.Email}\nElever: {company.Students}\nNæste aftaleudløb: {company.NextAgreementEnd}");
                }
                else
                {
                    Console.WriteLine($"{company.Name}\nAdresse: {company.Adress} {company.Postal}\nTelefon: {company.Phone}\nEmail: {company.Email}");
                }

                bool menuActive = true;
                while (menuActive)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nÅbn deres hjemmeside: W \nNæste virksomhed: Enter \nSend mail til virksomhed: T");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Enter:
                            menuActive = false;
                            break;
                        case ConsoleKey.W:
                            OpenBrowser($"https://www.google.com/search?q={company.Name}");
                            break;
                        case ConsoleKey.T:
                            SaveCompany(company);
                            break;
                        default:
                            break;
                    }
                }


            }

        }

        /// <summary>
        /// Returns a random company without any limitations
        /// </summary>
        private static Company GetRandomCompany()
        {
            //Finds the first sheet inside the file
            var firstSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

            int rows = firstSheet.Dimension.Rows;
            string[] information = new string[13];

            Random random = new Random();
            //First company starts at line 2
            int rnd = random.Next(2, rows);
            //Make it easier to insert into company instructor
            for (int i = 0; i < information.Length; i++)
            {
                information[i] = firstSheet.Cells[rnd, i + 1].Value.ToString();

            }
            Company company = new Company(information[0], information[1], information[2], Int32.Parse(information[3]), information[4], Int32.Parse(information[5]), information[6], information[7], information[11], Int32.Parse(information[12]));
            return company;
        }
        
        /// <summary>
        /// Returns a random company from the companiesList
        /// </summary>
        private static Company GetCompany()
        {
            Random random = new Random();
            int rnd = random.Next(0, companiesList.Count);
            
            return companiesList[rnd];
        }

        /// <summary>
        /// Returns company list with all companies in the desired postal and removes all companies email already sent to.
        /// </summary>
        private static List<Company> GetCompanyList()
        {
            //Find the sheet
            var firstSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

            int rows = firstSheet.Dimension.Rows;
            string[] information;

            List<Company> companiesList = new List<Company>();
            List<string> companiesSentToList = new List<string>();

            StreamReader sr = new StreamReader(sentEmailsLocation);
            //Put all companies from SentEmails txt file into list
            while (!sr.EndOfStream)
            {
                companiesSentToList.Add(sr.ReadLine());
            }
            sr.Close();

            for (int i = 2; i < rows; i++)
            {
                information = new string[13];
                for (int j = 0; j < 13; j++)
                {
                    information[j] = firstSheet.Cells[i, j + 1].Value.ToString();
                }
                //If postal is between 1000-4999
                if (Int32.Parse(information[3]) >= 1000 && Int32.Parse(information[3]) < 5000)
                {
                    //There must be an email listed before adding to list
                    if (!string.IsNullOrWhiteSpace(information[7]))
                    {
                        companiesList.Add(new Company(information[0], information[1], information[2], Int32.Parse(information[3]), information[4],
                            Int32.Parse(information[5]), information[6], information[7], information[11], Int32.Parse(information[12])));
                    }
                }
            }
            //Delete all companies from list that already have recieved email.
            foreach (var item in companiesList)
            {
                foreach (var addedItem in companiesSentToList)
                {
                    if (item.Name == addedItem)
                    {
                        companiesList.Remove(item);
                    }
                }
            }

            return companiesList;
        }



        /// <summary>
        /// Save the company to the list with sent emails.
        /// </summary>
        private static void SaveCompany(Company company)
        {
            //Check if file exists, and create file if not
            if (!File.Exists(sentEmailsLocation))
            {
                File.Create(sentEmailsLocation);
            }
            StreamWriter sw = new StreamWriter(sentEmailsLocation, true);
            //Could use company.CVR, but using company.Name for better overview when watching the list
            sw.WriteLine(company.Name);
            sw.Close();
            companiesList.Remove(company);
        }


        //.NET Core wont launch browser with process.start. This method lets you open browser with link
        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

    }
}
